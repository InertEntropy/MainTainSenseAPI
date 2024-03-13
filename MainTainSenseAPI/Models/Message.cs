using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MainTainSenseAPI.Models;

public partial class Message
{
    public int MessagesId { get; set; }

    public int? ParenttMessageId { get; set; }

    [Required]
    public int? RecipientId { get; set; }

    public string? MessageText { get; set; }

    public int? IsDeletedForSender { get; set; }

    public int? SenderId { get; set; }

    public int? IsRead { get; set; }

    public string? Subject { get; set; }

    public string? CreationTime { get; set; }

    public int? IsDeletedForRecipient { get; set; }

    public virtual ICollection<Message> InverseParenttMessage { get; set; } = new List<Message>();

    public virtual Message? ParenttMessage { get; set; }

    [Required]
    public virtual User? Recipient { get; set; }

    [Required]
    public virtual User? Sender { get; set; }
}
