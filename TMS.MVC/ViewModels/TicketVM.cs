using System.ComponentModel.DataAnnotations;
using TMS.MVC.Models;

namespace TMS.MVC.ViewModels
{
    public class TicketIndexViewModel
    {
        public string? Search { get; set; }
        public string? StatusFilter { get; set; }
        public string? PriorityFilter { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalItems { get; set; }
        public int TotalPages => PageSize <= 0 ? 1 : (int)Math.Ceiling((double)TotalItems / PageSize);
        public int RowNumberStart => ((Page - 1) * PageSize) + 1;
        public List<TicketIndexItemViewModel> Items { get; set; } = new();
    }

    public class TicketIndexItemViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; } = "";
        public string? Category { get; set; }
        public TicketPriority Priority { get; set; }
        public string PriorityDisplay { get; set; } = "";
        public string PriorityBadgeClass { get; set; } = "bg-secondary";
        public TicketStatus Status { get; set; }
        public string StatusDisplay { get; set; } = "";
        public string StatusBadgeClass { get; set; } = "bg-secondary";
        public string? CreatedByName { get; set; }
        public string? CreatedByEmail { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int MessageCount { get; set; }
        public string? LastMessagePreview { get; set; }
    }

    public class TicketCreateViewModel
    {
        [Required(ErrorMessage = "عنوان تیکت الزامی است.")]
        [MaxLength(250, ErrorMessage = "عنوان تیکت نمی‌تواند بیشتر از ۲۵۰ کاراکتر باشد.")]
        [Display(Name = "عنوان تیکت")]
        public string Title { get; set; } = "";

        [MaxLength(100, ErrorMessage = "دسته‌بندی نمی‌تواند بیشتر از ۱۰۰ کاراکتر باشد.")]
        [Display(Name = "دسته‌بندی")]
        public string? Category { get; set; }

        [Display(Name = "اولویت")]
        public TicketPriority Priority { get; set; } = TicketPriority.Normal;

        [Required(ErrorMessage = "متن پیام الزامی است.")]
        [MaxLength(4000, ErrorMessage = "متن پیام نمی‌تواند بیشتر از ۴۰۰۰ کاراکتر باشد.")]
        [Display(Name = "شرح درخواست")]
        public string Body { get; set; } = "";
    }

    public class TicketReplyViewModel
    {
        public long TicketId { get; set; }

        [Required(ErrorMessage = "متن پاسخ الزامی است.")]
        [MaxLength(4000, ErrorMessage = "متن پاسخ نمی‌تواند بیشتر از ۴۰۰۰ کاراکتر باشد.")]
        [Display(Name = "متن پاسخ")]
        public string Body { get; set; } = "";
    }

    public class TicketChangeStatusViewModel
    {
        public long TicketId { get; set; }

        [Display(Name = "وضعیت")]
        public TicketStatus Status { get; set; }

        [MaxLength(1000, ErrorMessage = "توضیح نمی‌تواند بیشتر از ۱۰۰۰ کاراکتر باشد.")]
        [Display(Name = "توضیح")]
        public string? Note { get; set; }
    }

    public class TicketDetailsViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; } = "";
        public string? Category { get; set; }
        public TicketPriority Priority { get; set; }
        public string PriorityDisplay { get; set; } = "";
        public string PriorityBadgeClass { get; set; } = "bg-secondary";
        public TicketStatus Status { get; set; }
        public string StatusDisplay { get; set; } = "";
        public string StatusBadgeClass { get; set; } = "bg-secondary";
        public string? CreatedByName { get; set; }
        public string? CreatedByEmail { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public string? ClosedBy { get; set; }
        public string? CloseNote { get; set; }
        public bool CanReply { get; set; }
        public bool IsOperatorPage { get; set; }
        public TicketReplyViewModel Reply { get; set; } = new();
        public List<TicketMessageItemViewModel> Messages { get; set; } = new();
    }

    public class TicketMessageItemViewModel
    {
        public long Id { get; set; }
        public string SenderName { get; set; } = "";
        public string? SenderEmail { get; set; }
        public string Body { get; set; } = "";
        public bool IsOperatorReply { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
