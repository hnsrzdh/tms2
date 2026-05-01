using System.ComponentModel.DataAnnotations;

namespace TMS.MVC.Models.Tickets;

public class TicketStatusHistory
{
    [Key]
    public long Id { get; set; }

    public long TicketId { get; set; }
    public Ticket Ticket { get; set; } = default!;

    public TicketStatus FromStatus { get; set; }
    public TicketStatus ToStatus { get; set; }

    [MaxLength(450)]
    public string? ByUserId { get; set; }

    public DateTime At { get; set; } = DateTime.UtcNow;

    [MaxLength(1000)]
    public string? Note { get; set; }
}