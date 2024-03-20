using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models.Views;

public partial class Message : BaseViewModels
{
    public int Id { get; set; }

    public int? ParenttMessageId { get; set; }

    [Required]
    public string? RecipientId { get; set; }

    [Required(ErrorMessage = "Please provide message text.")]
    public string? MessageText { get; set; }

    public int? IsDeletedForSender { get; set; }

    public string? SenderId { get; set; }

    public int? IsRead { get; set; }

    [Required(ErrorMessage = "Please provide a name for the asset.")]
    [StringLength(50, ErrorMessage = "Asset Name should be between 2 and 50 characters long.", MinimumLength = 2)]
    public string? Subject { get; set; }

    public string? CreationTime { get; set; }

    public int? IsDeletedForRecipient { get; set; }

    public virtual Message? ParenttMessage { get; set; }

    public virtual ApplicationUser? Recipient { get; set; }

   public virtual ApplicationUser? Sender { get; set; }
}
