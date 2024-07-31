namespace ChattingHub.Models
{
    public class ChatMessage
    {
        public string Id { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Text { get; set; }
        public long Timestamp { get; set; }
        public bool IsCurrentUser { get; set; } // True if message is from the current user
    }

}
