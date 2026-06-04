using InquiryDesk.Models;

namespace InquiryDesk.Services;

public interface IInquiryRepository
{
    IReadOnlyList<InquiryTicket> Search(TicketFilter filter);
    InquiryTicket? Find(int id);
    InquiryTicket Add(InquiryTicket ticket);
    bool Update(InquiryTicket ticket);
    bool Delete(int id);
    IReadOnlyList<SupportAgent> GetAgents();
    IReadOnlyDictionary<TicketStatus, int> GetStatusCounts();
}
