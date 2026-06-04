using InquiryDesk.Models;

namespace InquiryDesk.Services;

public sealed class InMemoryInquiryRepository : IInquiryRepository
{
    private readonly List<SupportAgent> _agents =
    [
        new() { Id = 1, Name = "佐藤 花子", Department = "情報システム" },
        new() { Id = 2, Name = "田中 健", Department = "総務" },
        new() { Id = 3, Name = "鈴木 明", Department = "経理システム" }
    ];

    private readonly List<InquiryTicket> _tickets =
    [
        new()
        {
            Id = 1,
            Title = "勤怠システムにログインできない",
            Description = "パスワード再設定後もログインに失敗します。至急確認をお願いします。",
            RequesterName = "山田 太郎",
            RequesterEmail = "taro.yamada@example.com",
            Priority = TicketPriority.High,
            Status = TicketStatus.InProgress,
            AssignedAgentId = 1,
            CreatedAt = DateTime.Today.AddDays(-2).AddHours(9),
            UpdatedAt = DateTime.Today.AddDays(-1).AddHours(16)
        },
        new()
        {
            Id = 2,
            Title = "経費精算の承認ルート確認",
            Description = "新しい部署に異動後、承認者が旧部署のまま表示されます。",
            RequesterName = "伊藤 美咲",
            RequesterEmail = "misaki.ito@example.com",
            Priority = TicketPriority.Normal,
            Status = TicketStatus.Assigned,
            AssignedAgentId = 3,
            CreatedAt = DateTime.Today.AddDays(-1).AddHours(11),
            UpdatedAt = DateTime.Today.AddDays(-1).AddHours(13)
        },
        new()
        {
            Id = 3,
            Title = "社内ポータルの記事公開依頼",
            Description = "来週公開予定のお知らせをポータルに掲載したいです。",
            RequesterName = "高橋 亮",
            RequesterEmail = "ryo.takahashi@example.com",
            Priority = TicketPriority.Low,
            Status = TicketStatus.New,
            CreatedAt = DateTime.Today.AddHours(10),
            UpdatedAt = DateTime.Today.AddHours(10)
        }
    ];

    private readonly object _lock = new();
    private int _nextId = 4;

    public IReadOnlyList<InquiryTicket> Search(TicketFilter filter)
    {
        lock (_lock)
        {
            IEnumerable<InquiryTicket> query = _tickets;

            if (!string.IsNullOrWhiteSpace(filter.Keyword))
            {
                var keyword = filter.Keyword.Trim();
                query = query.Where(ticket =>
                    ticket.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    ticket.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    ticket.RequesterName.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            }

            if (filter.Status is not null)
            {
                query = query.Where(ticket => ticket.Status == filter.Status);
            }

            if (filter.AgentId is not null)
            {
                query = query.Where(ticket => ticket.AssignedAgentId == filter.AgentId);
            }

            return query
                .OrderByDescending(ticket => ticket.Priority)
                .ThenByDescending(ticket => ticket.UpdatedAt)
                .Select(Clone)
                .ToList();
        }
    }

    public InquiryTicket? Find(int id)
    {
        lock (_lock)
        {
            var ticket = _tickets.FirstOrDefault(item => item.Id == id);
            return ticket is null ? null : Clone(ticket);
        }
    }

    public InquiryTicket Add(InquiryTicket ticket)
    {
        lock (_lock)
        {
            ticket.Id = _nextId++;
            ticket.CreatedAt = DateTime.Now;
            ticket.UpdatedAt = DateTime.Now;
            _tickets.Add(Clone(ticket));
            return Clone(ticket);
        }
    }

    public bool Update(InquiryTicket ticket)
    {
        lock (_lock)
        {
            var existing = _tickets.FirstOrDefault(item => item.Id == ticket.Id);
            if (existing is null)
            {
                return false;
            }

            existing.Title = ticket.Title;
            existing.Description = ticket.Description;
            existing.RequesterName = ticket.RequesterName;
            existing.RequesterEmail = ticket.RequesterEmail;
            existing.Priority = ticket.Priority;
            existing.Status = ticket.Status;
            existing.AssignedAgentId = ticket.AssignedAgentId;
            existing.UpdatedAt = DateTime.Now;

            return true;
        }
    }

    public bool Delete(int id)
    {
        lock (_lock)
        {
            var ticket = _tickets.FirstOrDefault(item => item.Id == id);
            return ticket is not null && _tickets.Remove(ticket);
        }
    }

    public IReadOnlyList<SupportAgent> GetAgents()
    {
        return _agents;
    }

    public IReadOnlyDictionary<TicketStatus, int> GetStatusCounts()
    {
        lock (_lock)
        {
            return Enum.GetValues<TicketStatus>()
                .ToDictionary(status => status, status => _tickets.Count(ticket => ticket.Status == status));
        }
    }

    private static InquiryTicket Clone(InquiryTicket ticket)
    {
        return new InquiryTicket
        {
            Id = ticket.Id,
            Title = ticket.Title,
            Description = ticket.Description,
            RequesterName = ticket.RequesterName,
            RequesterEmail = ticket.RequesterEmail,
            Priority = ticket.Priority,
            Status = ticket.Status,
            AssignedAgentId = ticket.AssignedAgentId,
            CreatedAt = ticket.CreatedAt,
            UpdatedAt = ticket.UpdatedAt
        };
    }
}
