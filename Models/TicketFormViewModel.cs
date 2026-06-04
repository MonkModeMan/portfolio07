namespace InquiryDesk.Models;

public sealed class TicketFormViewModel
{
    public required InquiryTicket Ticket { get; init; }
    public required IReadOnlyList<SupportAgent> Agents { get; init; }
}
