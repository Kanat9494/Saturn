namespace Saturn.Models;

public class Message
{
    public int MessageId { get; set; }
    public int SenderId { get; set; }
    public int RecieverId { get; set; }
    public string Content { get; set; }
    public DateTime SentDate { get; set; }
}
