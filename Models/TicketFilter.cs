namespace InquiryDesk.Models;

public sealed class TicketFilter
{
    public string? Keyword { get; set; }
    public TicketStatus? Status { get; set; }
    public int? AgentId { get; set; }
}
