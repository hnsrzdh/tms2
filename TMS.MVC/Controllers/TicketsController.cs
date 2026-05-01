using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMS.MVC.Data;
using TMS.MVC.Infrastructure;
using TMS.MVC.Models.Tickets;
using TMS.MVC.ViewModels.Tickets;

namespace TMS.MVC.Controllers;

[Authorize]
public class TicketsController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IWebHostEnvironment _env;

    public TicketsController(ApplicationDbContext db, UserManager<IdentityUser> userManager, IWebHostEnvironment env)
    {
        _db = db;
        _userManager = userManager;
        _env = env;
    }

    public async Task<IActionResult> MyTickets(string? q = null, string? status = null)
    {
        var userId = _userManager.GetUserId(User)!;

        var query = _db.Tickets.AsNoTracking().Where(t => t.CreatedByUserId == userId);

        if (!string.IsNullOrWhiteSpace(q))
        {
            q = q.Trim();
            query = query.Where(t => t.Code.Contains(q) || t.Subject.Contains(q));
        }

        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<TicketStatus>(status, true, out var st))
        {
            query = query.Where(t => t.Status == st);
        }

        var list = await query
            .Include(t => t.Category)
            .OrderByDescending(t => t.Id)
            .ToListAsync();

        ViewBag.Q = q;
        ViewBag.Status = status;
        return View(list);
    }

    public async Task<IActionResult> Create(string? relatedType = null, long? relatedId = null)
    {
        ViewBag.Categories = await _db.TicketCategories.AsNoTracking()
            .Where(c => c.IsActive)
            .OrderBy(c => c.SortOrder).ThenBy(c => c.Title)
            .ToListAsync();

        var vm = new TicketCreateVm
        {
            RelatedEntityType = relatedType,
            RelatedEntityId = relatedId
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TicketCreateVm vm)
    {
        var userId = _userManager.GetUserId(User)!;

        if (!await _db.TicketCategories.AnyAsync(c => c.Id == vm.CategoryId && c.IsActive))
            ModelState.AddModelError(nameof(vm.CategoryId), "دسته‌بندی نامعتبر است.");

        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await _db.TicketCategories.AsNoTracking()
                .Where(c => c.IsActive)
                .OrderBy(c => c.SortOrder).ThenBy(c => c.Title)
                .ToListAsync();
            return View(vm);
        }

        var now = DateTime.UtcNow;
        var year = now.Year;

        // code generation: TCK-YYYY-XXXXXX
        // Use sequential Id after save; so we create placeholder then update.
        // Use a unique temporary code to avoid violating the unique index on Code
        var ticket = new Ticket
        {
            Code = $"TMP-{Guid.NewGuid():N}",
            Subject = vm.Subject.Trim(),
            CategoryId = vm.CategoryId,
            Priority = (TicketPriority)vm.Priority,
            Status = TicketStatus.New,
            CreatedByUserId = userId,
            CreatedAt = now,
            LastReplyAt = now,
            RelatedEntityType = string.IsNullOrWhiteSpace(vm.RelatedEntityType) ? null : vm.RelatedEntityType.Trim(),
            RelatedEntityId = vm.RelatedEntityId
        };

        _db.Tickets.Add(ticket);
        await _db.SaveChangesAsync();

        ticket.Code = $"TCK-{year}-{ticket.Id:000000}";
        _db.Tickets.Update(ticket);

        var firstMsg = new TicketMessage
        {
            TicketId = ticket.Id,
            Body = vm.Body,
            IsInternal = false,
            CreatedByUserId = userId,
            CreatedAt = now
        };
        _db.TicketMessages.Add(firstMsg);

        _db.TicketStatusHistories.Add(new TicketStatusHistory
        {
            TicketId = ticket.Id,
            FromStatus = TicketStatus.New,
            ToStatus = TicketStatus.New,
            ByUserId = userId,
            At = now,
            Note = "ایجاد تیکت"
        });

        await _db.SaveChangesAsync();

        TempData["Ok"] = "تیکت ثبت شد.";
        return RedirectToAction(nameof(Details), new { id = ticket.Id });
    }

    public async Task<IActionResult> Details(long id)
    {
        var userId = _userManager.GetUserId(User)!;

        var ticket = await _db.Tickets
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (ticket == null) return NotFound();

        // User can only see their own ticket unless admin
        var isAdmin = User.IsInRole(AppRoles.SystemAdmin);
        if (!isAdmin && ticket.CreatedByUserId != userId) return Forbid();

        var messages = await _db.TicketMessages.AsNoTracking()
            .Where(m => m.TicketId == ticket.Id)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();

        var msgIds = messages.Select(m => m.Id).ToList();
        var atts = await _db.TicketAttachments.AsNoTracking()
            .Where(a => msgIds.Contains(a.TicketMessageId))
            .ToListAsync();

        // map user display (simple)
        var userIds = messages.Select(m => m.CreatedByUserId).Append(ticket.CreatedByUserId).Distinct().ToList();
        if (!string.IsNullOrWhiteSpace(ticket.AssignedToUserId)) userIds.Add(ticket.AssignedToUserId);

        var users = await _db.Users.AsNoTracking()
            .Where(u => userIds.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id, u => u.UserName ?? u.Email ?? u.Id);

        string disp(string uid) => users.TryGetValue(uid, out var d) ? d : uid;

        var vm = new TicketDetailsVm
        {
            Ticket = ticket,
            CreatedByDisplay = disp(ticket.CreatedByUserId),
            AssignedToDisplay = ticket.AssignedToUserId == null ? null : disp(ticket.AssignedToUserId),
            Messages = messages
                .Where(m => isAdmin || !m.IsInternal) // hide internal notes for non-admin
                .Select(m => new TicketDetailsVm.TicketMessageDto
                {
                    Message = m,
                    AuthorDisplay = disp(m.CreatedByUserId),
                    Attachments = atts.Where(a => a.TicketMessageId == m.Id).ToList()
                })
                .ToList()
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddMessage(long id, string body, List<IFormFile>? files)
    {
        var userId = _userManager.GetUserId(User)!;

        var ticket = await _db.Tickets.FirstOrDefaultAsync(t => t.Id == id);
        if (ticket == null) return NotFound();

        var isAdmin = User.IsInRole(AppRoles.SystemAdmin);
        if (!isAdmin && ticket.CreatedByUserId != userId) return Forbid();

        if (string.IsNullOrWhiteSpace(body))
        {
            TempData["Error"] = "متن پیام الزامی است.";
            return RedirectToAction(nameof(Details), new { id });
        }

        // status changes: if user replies while WaitingOnUser -> Open
        if (!isAdmin && ticket.Status == TicketStatus.WaitingOnUser)
        {
            await AddStatusHistoryAsync(ticket, TicketStatus.WaitingOnUser, TicketStatus.Open, userId, "پاسخ کاربر");
            ticket.Status = TicketStatus.Open;
        }
        else if (isAdmin && ticket.Status == TicketStatus.New)
        {
            await AddStatusHistoryAsync(ticket, TicketStatus.New, TicketStatus.Open, userId, "اولین پاسخ پشتیبانی");
            ticket.Status = TicketStatus.Open;
        }

        var msg = new TicketMessage
        {
            TicketId = ticket.Id,
            Body = body,
            IsInternal = false,
            CreatedByUserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        _db.TicketMessages.Add(msg);
        ticket.LastReplyAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(); // to get msg.Id

        if (files != null && files.Count > 0)
        {
            foreach (var f in files.Where(x => x != null && x.Length > 0))
            {
                var relDir = $"/uploads/tickets/{ticket.Id}/{msg.Id}";
                var absDir = Path.Combine(_env.WebRootPath, "uploads", "tickets", ticket.Id.ToString(), msg.Id.ToString());
                Directory.CreateDirectory(absDir);

                var safeName = $"{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid():N}{Path.GetExtension(f.FileName)}";
                var absPath = Path.Combine(absDir, safeName);

                await using (var stream = System.IO.File.Create(absPath))
                    await f.CopyToAsync(stream);

                _db.TicketAttachments.Add(new TicketAttachment
                {
                    TicketMessageId = msg.Id,
                    FilePath = $"{relDir}/{safeName}",
                    OriginalFileName = f.FileName,
                    SizeBytes = f.Length,
                    UploadedAt = DateTime.UtcNow
                });
            }
            await _db.SaveChangesAsync();
        }

        TempData["Ok"] = "پیام ثبت شد.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [Authorize(Roles = AppRoles.SystemAdmin)]
    public async Task<IActionResult> Index(string? q = null, string? status = null, long? categoryId = null)
    {
        var query = _db.Tickets
            .AsNoTracking()
            .Include(t => t.Category)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            q = q.Trim();
            query = query.Where(t =>
                t.Code.Contains(q) ||
                t.Subject.Contains(q) ||
                (t.CreatedByUserId != null && t.CreatedByUserId.Contains(q)) ||
                (t.AssignedToUserId != null && t.AssignedToUserId.Contains(q)));
        }

        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<TicketStatus>(status, true, out var st))
        {
            query = query.Where(t => t.Status == st);
        }

        if (categoryId.HasValue)
        {
            query = query.Where(t => t.CategoryId == categoryId.Value);
        }

        var list = await query
            .OrderByDescending(t => t.Id)
            .ToListAsync();

        // Optional: categories for a dropdown filter in the view
        ViewBag.Categories = await _db.TicketCategories.AsNoTracking()
            .Where(c => c.IsActive)
            .OrderBy(c => c.SortOrder).ThenBy(c => c.Title)
            .ToListAsync();

        ViewBag.Q = q;
        ViewBag.Status = status;
        ViewBag.CategoryId = categoryId;

        return View(list);
    }
    private async Task AddStatusHistoryAsync(Ticket ticket, TicketStatus from, TicketStatus to, string byUserId, string? note)
    {
        _db.TicketStatusHistories.Add(new TicketStatusHistory
        {
            TicketId = ticket.Id,
            FromStatus = from,
            ToStatus = to,
            ByUserId = byUserId,
            At = DateTime.UtcNow,
            Note = note
        });
        await Task.CompletedTask;
    }
}