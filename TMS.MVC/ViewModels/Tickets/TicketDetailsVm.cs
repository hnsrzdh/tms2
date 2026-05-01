using TMS.MVC.Models.Tickets;

namespace TMS.MVC.ViewModels.Tickets;

public class TicketDetailsVm
{
    public Ticket Ticket { get; set; } = default!;
    public string CreatedByDisplay { get; set; } = "";
    public string? AssignedToDisplay { get; set; }

    public List<TicketMessageDto> Messages { get; set; } = new();

    public class TicketMessageDto
    {
        public TicketMessage Message { get; set; } = default!;
        public string AuthorDisplay { get; set; } = "";
        public List<TicketAttachment> Attachments { get; set; } = new();
    }
}