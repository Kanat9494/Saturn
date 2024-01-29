namespace Saturn.Models;

public class Message
{
    [PrimaryKey, AutoIncrement]
    public int MessageId { get; set; }
    public int SenderId { get; set; }
    public int RecieverId { get; set; }
    public string Content { get; set; }
    public DateTime SentDate { get; set; }
    public int ChatId { get; set; }
}
