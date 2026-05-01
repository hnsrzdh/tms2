using System.ComponentModel.DataAnnotations;

namespace TMS.MVC.Models.Tickets;

public class TicketCategory
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Title { get; set; } = default!;

    public bool IsActive { get; set; } = true;

    public int SortOrder { get; set; } = 0;
}