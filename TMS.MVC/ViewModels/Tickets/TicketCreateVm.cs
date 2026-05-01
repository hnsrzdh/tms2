using System.ComponentModel.DataAnnotations;

namespace TMS.MVC.ViewModels.Tickets;

public class TicketCreateVm
{
    [Required, MaxLength(200)]
    public string Subject { get; set; } = default!;

    [Required]
    public string Body { get; set; } = default!;

    [Required]
    public int CategoryId { get; set; }

    [Required]
    public int Priority { get; set; } = 2; // Normal

    // optional linking
    public string? RelatedEntityType { get; set; }
    public long? RelatedEntityId { get; set; }
}