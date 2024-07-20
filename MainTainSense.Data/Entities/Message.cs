
namespace MainTainSense.Data
{
    public partial class Message
    {
        public int Id { get; set; }
        public int ParenttMessageId { get; set; }
        public string RecipientId { get; set; }
        public string MessageText { get; set; }
        public int IsDeletedForSender { get; set; }
        public string SenderId { get; set; }
        public int IsRead { get; set; }
        public string Subject { get; set; }
        public string CreationTime { get; set; }
        public int IsDeletedForRecipient { get; set; }

    }
}