namespace Saturn.Models;

public class ChatRoom
{
    [PrimaryKey, AutoIncrement]
    public int ChatId { get; set; }
    public string Title { get; set; }
    public int UserId { get; set; }
    public string LastMessage { get; set; }
}
