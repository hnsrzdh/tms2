using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMS.MVC.Models
{
    public enum TicketPriority
    {
        Low = 0,
        Normal = 1,
        High = 2,
        Urgent = 3
    }

    public enum TicketStatus
    {
        Open = 0,
        WaitingForOperator = 1,
        WaitingForUser = 2,
        Closed = 3,
        Cancelled = 4
    }

    public class Ticket
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "وارد کردن عنوان الزامی است.")]
        [MaxLength(250)]
        public string Title { get; set; } = null!;

        [MaxLength(100)]
        public string? Category { get; set; }

        public TicketPriority Priority { get; set; } = TicketPriority.Normal;

        public TicketStatus Status { get; set; } = TicketStatus.Open;

        [Required(ErrorMessage = "انتخاب کاربر ثبت‌کننده الزامی است.")]
        public string CreatedByUserId { get; set; } = null!;

        public ApplicationUser CreatedByUser { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public DateTime? ClosedAt { get; set; }

        [MaxLength(256)]
        public string? ClosedBy { get; set; }

        [MaxLength(1000)]
        public string? CloseNote { get; set; }

        public ICollection<TicketMessage> Messages { get; set; } = new List<TicketMessage>();
    }

    public class TicketMessage
    {
        public long Id { get; set; }

        public long TicketId { get; set; }
        public Ticket Ticket { get; set; } = null!;

        [Required(ErrorMessage = "انتخاب کاربر فرستنده الزامی است.")]
        public string SenderUserId { get; set; } = null!;

        public ApplicationUser SenderUser { get; set; } = null!;

        [Required(ErrorMessage = "وارد کردن شرح درخواست الزامی است.")]
        [MaxLength(4000)]
        public string Body { get; set; } = null!;

        public bool IsOperatorReply { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
