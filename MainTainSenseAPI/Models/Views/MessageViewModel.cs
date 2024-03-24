using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models.Views;

public partial class Message : BaseViewModels
{
    public int Id { get; set; }

    public int? ParenttMessageId { get; set; }

    [Required]
    public string? RecipientId { get; set; }

    [Required(ErrorMessage = "Please provide message text.")]
    [StringLength(500, ErrorMessage = "Message should be between 2 and 500 characters long.", MinimumLength = 2)]
    public string? MessageText { get; set; }

    public int? IsDeletedForSender { get; set; }

    public string? SenderId { get; set; }

    public int? IsRead { get; set; }

    [Required(ErrorMessage = "Please provide a subject for the mesage.")]
    [StringLength(50, ErrorMessage = "Subject should be between 2 and 50 characters long.", MinimumLength = 2)]
    public string? Subject { get; set; }

    public string? CreationTime { get; set; }

    public int? IsDeletedForRecipient { get; set; }

    public virtual Message? ParenttMessage { get; set; }

    public virtual ApplicationUser? Recipient { get; set; }

    public virtual ApplicationUser? Sender { get; set; }
}
