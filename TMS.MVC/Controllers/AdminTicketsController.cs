using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMS.MVC.Data;
using TMS.MVC.Infrastructure;
using TMS.MVC.Models.Tickets;
using TMS.MVC.ViewModels.Tickets;

namespace TMS.MVC.Controllers;

[Authorize(Roles = AppRoles.SystemAdmin)]
public class AdminTicketsController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<IdentityUser> _userManager;

    public AdminTicketsController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index(string? q = null, string? status = null, string? priority = null)
    {
        var query = _db.Tickets.AsNoTracking().Include(t => t.Category).AsQueryable();

        if (!string.IsNullOrWhiteSpace(q))
        {
            q = q.Trim();
            query = query.Where(t => t.Code.Contains(q) || t.Subject.Contains(q) || (t.RelatedEntityType != null && t.RelatedEntityType.Contains(q)));
        }

        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<TicketStatus>(status, true, out var st))
            query = query.Where(t => t.Status == st);

        if (!string.IsNullOrWhiteSpace(priority) && Enum.TryParse<TicketPriority>(priority, true, out var pr))
            query = query.Where(t => t.Priority == pr);

        var list = await query.OrderByDescending(t => t.Id).ToListAsync();

        ViewBag.Q = q;
        ViewBag.Status = status;
        ViewBag.Priority = priority;
        return View(list);
    }

    public async Task<IActionResult> Details(long id)
    {
        // reuse user view but admin sees internal notes too
        return RedirectToAction("Details", "Tickets", new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Assign(long id, string? assignedToUserId)
    {
        var ticket = await _db.Tickets.FirstOrDefaultAsync(t => t.Id == id);
        if (ticket == null) return NotFound();

        ticket.AssignedToUserId = string.IsNullOrWhiteSpace(assignedToUserId) ? null : assignedToUserId.Trim();
        await _db.SaveChangesAsync();

        TempData["Ok"] = "ارجاع انجام شد.";
        return RedirectToAction("Details", "Tickets", new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeStatus(long id, string status, string? note)
    {
        var ticket = await _db.Tickets.FirstOrDefaultAsync(t => t.Id == id);
        if (ticket == null) return NotFound();

        if (!Enum.TryParse<TicketStatus>(status, true, out var to))
        {
            TempData["Error"] = "وضعیت نامعتبر است.";
            return RedirectToAction("Details", "Tickets", new { id });
        }

        var from = ticket.Status;
        if (from == to)
        {
            TempData["Ok"] = "وضعیت تغییری نکرد.";
            return RedirectToAction("Details", "Tickets", new { id });
        }

        ticket.Status = to;
        if (to == TicketStatus.Closed) ticket.ClosedAt = DateTime.UtcNow;

        _db.TicketStatusHistories.Add(new TicketStatusHistory
        {
            TicketId = ticket.Id,
            FromStatus = from,
            ToStatus = to,
            ByUserId = _userManager.GetUserId(User),
            At = DateTime.UtcNow,
            Note = note
        });

        await _db.SaveChangesAsync();

        TempData["Ok"] = "وضعیت بروزرسانی شد.";
        return RedirectToAction("Details", "Tickets", new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddInternalNote(long id, string body)
    {
        var ticket = await _db.Tickets.FirstOrDefaultAsync(t => t.Id == id);
        if (ticket == null) return NotFound();

        if (string.IsNullOrWhiteSpace(body))
        {
            TempData["Error"] = "متن یادداشت الزامی است.";
            return RedirectToAction("Details", "Tickets", new { id });
        }

        _db.TicketMessages.Add(new TicketMessage
        {
            TicketId = ticket.Id,
            Body = body,
            IsInternal = true,
            CreatedByUserId = _userManager.GetUserId(User)!,
            CreatedAt = DateTime.UtcNow
        });

        await _db.SaveChangesAsync();
        TempData["Ok"] = "یادداشت داخلی ثبت شد.";
        return RedirectToAction("Details", "Tickets", new { id });
    }
}