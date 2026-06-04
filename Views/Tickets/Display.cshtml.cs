using InquiryDesk.Models;

namespace InquiryDesk.Views.Tickets;

public static class Display
{
    public static string Status(TicketStatus status) => status switch
    {
        TicketStatus.New => "新規",
        TicketStatus.Assigned => "割り当て済み",
        TicketStatus.InProgress => "対応中",
        TicketStatus.WaitingForCustomer => "依頼者確認待ち",
        TicketStatus.Resolved => "解決済み",
        TicketStatus.Closed => "クローズ",
        _ => status.ToString()
    };

    public static string Priority(TicketPriority priority) => priority switch
    {
        TicketPriority.Low => "低",
        TicketPriority.Normal => "通常",
        TicketPriority.High => "高",
        TicketPriority.Critical => "緊急",
        _ => priority.ToString()
    };
}
