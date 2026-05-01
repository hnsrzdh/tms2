using System.ComponentModel.DataAnnotations;

namespace TMS.MVC.Models.Tickets;

public class TicketMessage
{
    [Key]
    public long Id { get; set; }

    public long TicketId { get; set; }
    public Ticket Ticket { get; set; } = default!;

    [Required]
    public string Body { get; set; } = default!;

    // For admin internal notes (not visible to user)
    public bool IsInternal { get; set; } = false;

    [Required, MaxLength(450)]
    public string CreatedByUserId { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}