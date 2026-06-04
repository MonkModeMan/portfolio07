using System.ComponentModel.DataAnnotations;

namespace InquiryDesk.Models;

public sealed class InquiryTicket
{
    public int Id { get; set; }

    [Required(ErrorMessage = "件名を入力してください。")]
    [StringLength(80, ErrorMessage = "件名は80文字以内で入力してください。")]
    [Display(Name = "件名")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "問い合わせ内容を入力してください。")]
    [StringLength(1000, ErrorMessage = "問い合わせ内容は1000文字以内で入力してください。")]
    [Display(Name = "問い合わせ内容")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "依頼者名を入力してください。")]
    [StringLength(40, ErrorMessage = "依頼者名は40文字以内で入力してください。")]
    [Display(Name = "依頼者")]
    public string RequesterName { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "メールアドレスの形式で入力してください。")]
    [Display(Name = "メールアドレス")]
    public string? RequesterEmail { get; set; }

    [Display(Name = "優先度")]
    public TicketPriority Priority { get; set; } = TicketPriority.Normal;

    [Display(Name = "ステータス")]
    public TicketStatus Status { get; set; } = TicketStatus.New;

    [Display(Name = "担当者")]
    public int? AssignedAgentId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
