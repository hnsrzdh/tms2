using System.ComponentModel.DataAnnotations;

namespace TMS.MVC.Models.Tickets;

public class TicketAttachment
{
    [Key]
    public long Id { get; set; }

    public long TicketMessageId { get; set; }
    public TicketMessage TicketMessage { get; set; } = default!;

    [Required, MaxLength(400)]
    public string FilePath { get; set; } = default!; // /uploads/tickets/{ticketId}/{messageId}/file.ext

    [MaxLength(255)]
    public string? OriginalFileName { get; set; }

    public long SizeBytes { get; set; }

    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}