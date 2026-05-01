using System.ComponentModel.DataAnnotations;

namespace TMS.MVC.Models.Tickets;

public class Ticket
{
    [Key]
    public long Id { get; set; }

    // Human-friendly code like TCK-2026-000123
    [Required, MaxLength(50)]
    public string Code { get; set; } = default!;

    [Required, MaxLength(200)]
    public string Subject { get; set; } = default!;

    public int CategoryId { get; set; }
    public TicketCategory? Category { get; set; }

    public TicketPriority Priority { get; set; } = TicketPriority.Normal;

    public TicketStatus Status { get; set; } = TicketStatus.New;

    [Required, MaxLength(450)]
    public string CreatedByUserId { get; set; } = default!;

    [MaxLength(450)]
    public string? AssignedToUserId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastReplyAt { get; set; }
    public DateTime? ClosedAt { get; set; }

    // Optional linking to entities (Havaleh / TankerTracker / etc.)
    [MaxLength(50)]
    public string? RelatedEntityType { get; set; } // e.g. "Havaleh", "TankerTracker"

    public long? RelatedEntityId { get; set; }

    // Concurrency
    [Timestamp]
    public byte[]? RowVersion { get; set; }
}