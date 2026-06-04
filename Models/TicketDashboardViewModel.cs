namespace InquiryDesk.Models;

public sealed class TicketDashboardViewModel
{
    public required IReadOnlyList<InquiryTicket> Tickets { get; init; }
    public required IReadOnlyList<SupportAgent> Agents { get; init; }
    public required TicketFilter Filter { get; init; }
    public required IReadOnlyDictionary<TicketStatus, int> StatusCounts { get; init; }
}
