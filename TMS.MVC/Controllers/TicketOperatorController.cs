using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMS.MVC.Data;
using TMS.MVC.Models;
using TMS.MVC.ViewModels;

namespace TMS.MVC.Controllers
{
    [Authorize(Roles = "SystemAdmin,OperationsManager,OperationsUser,FinanceManager,FinanceUser")]
    public class TicketOperatorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TicketOperatorController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string? search, string? statusFilter, string? priorityFilter, int page = 1, int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize <= 0) pageSize = 10;

            var query = _context.Tickets
                .Include(x => x.CreatedByUser)
                .Include(x => x.Messages)
                    .ThenInclude(x => x.SenderUser)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(x =>
                    x.Title.Contains(search) ||
                    (x.Category != null && x.Category.Contains(search)) ||
                    x.CreatedByUser.FirstName.Contains(search) ||
                    x.CreatedByUser.LastName.Contains(search) ||
                    x.CreatedByUser.Email.Contains(search) ||
                    x.Messages.Any(m => m.Body.Contains(search)));
            }

            if (!string.IsNullOrWhiteSpace(statusFilter) && Enum.TryParse<TicketStatus>(statusFilter, out var status))
                query = query.Where(x => x.Status == status);

            if (!string.IsNullOrWhiteSpace(priorityFilter) && Enum.TryParse<TicketPriority>(priorityFilter, out var priority))
                query = query.Where(x => x.Priority == priority);

            var totalItems = await query.CountAsync();

            var tickets = await query
                .OrderByDescending(x => x.UpdatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var model = new TicketIndexViewModel
            {
                Search = search,
                StatusFilter = statusFilter,
                PriorityFilter = priorityFilter,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                Items = tickets.Select(ToIndexItem).ToList()
            };

            return View(model);
        }

        public async Task<IActionResult> Details(long id)
        {
            var ticket = await _context.Tickets
                .Include(x => x.CreatedByUser)
                .Include(x => x.Messages)
                    .ThenInclude(x => x.SenderUser)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (ticket == null) return NotFound();

            return View(ToDetailsModel(ticket, true));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reply(TicketReplyViewModel model)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrWhiteSpace(userId)) return Challenge();

            var ticket = await _context.Tickets.FirstOrDefaultAsync(x => x.Id == model.TicketId);
            if (ticket == null) return NotFound();

            if (ticket.Status == TicketStatus.Closed || ticket.Status == TicketStatus.Cancelled)
            {
                TempData["Err"] = "امکان پاسخ به تیکت بسته یا لغوشده وجود ندارد.";
                return RedirectToAction(nameof(Details), new { id = model.TicketId });
            }

            if (!ModelState.IsValid)
            {
                TempData["Err"] = "متن پاسخ معتبر نیست.";
                return RedirectToAction(nameof(Details), new { id = model.TicketId });
            }

            _context.TicketMessages.Add(new TicketMessage
            {
                TicketId = ticket.Id,
                SenderUserId = userId,
                Body = model.Body.Trim(),
                IsOperatorReply = true,
                CreatedAt = DateTime.Now
            });

            ticket.Status = TicketStatus.WaitingForUser;
            ticket.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            TempData["Ok"] = "پاسخ اپراتور ثبت شد.";
            return RedirectToAction(nameof(Details), new { id = model.TicketId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(TicketChangeStatusViewModel model)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(x => x.Id == model.TicketId);
            if (ticket == null) return NotFound();

            ticket.Status = model.Status;
            ticket.UpdatedAt = DateTime.Now;

            if (model.Status == TicketStatus.Closed || model.Status == TicketStatus.Cancelled)
            {
                ticket.ClosedAt = DateTime.Now;
                ticket.ClosedBy = User.Identity?.Name;
                ticket.CloseNote = string.IsNullOrWhiteSpace(model.Note) ? null : model.Note.Trim();
            }
            else
            {
                ticket.ClosedAt = null;
                ticket.ClosedBy = null;
                ticket.CloseNote = null;
            }

            await _context.SaveChangesAsync();

            TempData["Ok"] = "وضعیت تیکت بروزرسانی شد.";
            return RedirectToAction(nameof(Details), new { id = model.TicketId });
        }

        private static TicketIndexItemViewModel ToIndexItem(Ticket ticket)
        {
            var lastMessage = ticket.Messages.OrderByDescending(x => x.CreatedAt).FirstOrDefault();

            return new TicketIndexItemViewModel
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Category = ticket.Category,
                Priority = ticket.Priority,
                PriorityDisplay = TicketsController.GetPriorityDisplay(ticket.Priority),
                PriorityBadgeClass = TicketsController.GetPriorityBadgeClass(ticket.Priority),
                Status = ticket.Status,
                StatusDisplay = TicketsController.GetStatusDisplay(ticket.Status),
                StatusBadgeClass = TicketsController.GetStatusBadgeClass(ticket.Status),
                CreatedByName = GetUserDisplayName(ticket.CreatedByUser),
                CreatedByEmail = ticket.CreatedByUser?.Email,
                CreatedAt = ticket.CreatedAt,
                UpdatedAt = ticket.UpdatedAt,
                MessageCount = ticket.Messages.Count,
                LastMessagePreview = lastMessage == null ? null : Shorten(lastMessage.Body, 90)
            };
        }

        private static TicketDetailsViewModel ToDetailsModel(Ticket ticket, bool isOperatorPage)
        {
            return new TicketDetailsViewModel
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Category = ticket.Category,
                Priority = ticket.Priority,
                PriorityDisplay = TicketsController.GetPriorityDisplay(ticket.Priority),
                PriorityBadgeClass = TicketsController.GetPriorityBadgeClass(ticket.Priority),
                Status = ticket.Status,
                StatusDisplay = TicketsController.GetStatusDisplay(ticket.Status),
                StatusBadgeClass = TicketsController.GetStatusBadgeClass(ticket.Status),
                CreatedByName = GetUserDisplayName(ticket.CreatedByUser),
                CreatedByEmail = ticket.CreatedByUser?.Email,
                CreatedAt = ticket.CreatedAt,
                UpdatedAt = ticket.UpdatedAt,
                ClosedAt = ticket.ClosedAt,
                ClosedBy = ticket.ClosedBy,
                CloseNote = ticket.CloseNote,
                IsOperatorPage = isOperatorPage,
                CanReply = ticket.Status != TicketStatus.Closed && ticket.Status != TicketStatus.Cancelled,
                Reply = new TicketReplyViewModel { TicketId = ticket.Id },
                Messages = ticket.Messages
                    .OrderBy(x => x.CreatedAt)
                    .Select(x => new TicketMessageItemViewModel
                    {
                        Id = x.Id,
                        SenderName = GetUserDisplayName(x.SenderUser),
                        SenderEmail = x.SenderUser?.Email,
                        Body = x.Body,
                        IsOperatorReply = x.IsOperatorReply,
                        CreatedAt = x.CreatedAt
                    })
                    .ToList()
            };
        }

        private static string GetUserDisplayName(ApplicationUser? user)
        {
            if (user == null) return "-";
            var name = $"{user.FirstName ?? ""} {user.LastName ?? ""}".Trim();
            return string.IsNullOrWhiteSpace(name) ? user.Email ?? "-" : name;
        }

        private static string Shorten(string text, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(text)) return "-";
            return text.Length <= maxLength ? text : text[..maxLength] + "...";
        }
    }
}
