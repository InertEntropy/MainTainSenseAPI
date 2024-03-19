using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class Message
{
    public int Id { get; set; }

    public int? ParenttMessageId { get; set; }

    [Required]
    public string? RecipientId { get; set; }

    public string? MessageText { get; set; }

    public int? IsDeletedForSender { get; set; }

    public string? SenderId { get; set; }

    public int? IsRead { get; set; }

    public string? Subject { get; set; }

    public string? CreationTime { get; set; }

    public int? IsDeletedForRecipient { get; set; }

    public virtual ICollection<Message> InverseParenttMessage { get; set; } = [];

    public virtual Message? ParenttMessage { get; set; }

    public virtual ApplicationUser? Recipient { get; set; }

   public virtual ApplicationUser? Sender { get; set; }
}
