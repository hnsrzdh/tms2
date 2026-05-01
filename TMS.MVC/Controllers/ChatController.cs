using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMS.MVC.Data;
using TMS.MVC.Models;
using TMS.MVC.ViewModels;

namespace TMS.MVC.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ChatController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> Index(long assignmentId)
        {
            var assignment = await _context.TractorAssignments
                .Include(a => a.Tractor)
                .Include(a => a.DriverProfile)
                    .ThenInclude(d => d.ApplicationUser)
                .FirstOrDefaultAsync(a => a.Id == assignmentId);

            if (assignment == null) return NotFound();

            var messages = await _context.ChatMessages
                .Where(m => m.TractorAssignmentId == assignmentId)
                .OrderByDescending(m => m.SentDate)
                .Take(100)
                .ToListAsync();

            var model = new ChatViewModel
            {
                AssignmentId = assignmentId,
                TractorPlateNumber = assignment.Tractor.PolicePlateNumber,
                DriverName = assignment.DriverProfile?.ApplicationUser.FullName ?? "-",
                Messages = messages.Select(m => new ChatMessageViewModel
                {
                    Id = m.Id,
                    SenderName = m.SenderName,
                    SenderRole = m.SenderRole,
                    Message = m.Message,
                    FilePath = m.FilePath,
                    FileName = m.FileName,
                    SentDate = m.SentDate,
                    IsRead = m.IsRead
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromForm] SendMessageInputModel model)
        {
            var message = new ChatMessage
            {
                TractorAssignmentId = model.AssignmentId,
                SenderId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "",
                SenderName = User.Identity?.Name ?? "Unknown",
                SenderRole = User.IsInRole("Driver") ? "Driver" :
                             User.IsInRole("Admin") ? "Admin" : "Operator",
                Message = model.Message,
                SentDate = DateTime.Now
            };

            // اگر فایل ضمیمه شده
            if (model.File != null && model.File.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.File.FileName);
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "chat");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.File.CopyToAsync(stream);
                }

                message.FilePath = $"/uploads/chat/{fileName}";
                message.FileName = model.File.FileName;
                message.FileContentType = model.File.ContentType;
                message.FileSize = model.File.Length;
            }

            _context.ChatMessages.Add(message);
            await _context.SaveChangesAsync();

            return Json(new { success = true, messageId = message.Id });
        }

        [HttpGet]
        public async Task<IActionResult> GetNewMessages(long assignmentId, long lastMessageId = 0)
        {
            var messages = await _context.ChatMessages
                .Where(m => m.TractorAssignmentId == assignmentId && m.Id > lastMessageId)
                .OrderBy(m => m.SentDate)
                .Select(m => new ChatMessageViewModel
                {
                    Id = m.Id,
                    SenderName = m.SenderName,
                    SenderRole = m.SenderRole,
                    Message = m.Message,
                    FilePath = m.FilePath,
                    FileName = m.FileName,
                    SentDate = m.SentDate,
                    IsRead = m.IsRead
                })
                .ToListAsync();

            return Json(messages);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsRead(long messageId)
        {
            var message = await _context.ChatMessages.FindAsync(messageId);
            if (message != null)
            {
                message.IsRead = true;
                message.ReadDate = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            return Json(new { success = true });
        }
    }
}