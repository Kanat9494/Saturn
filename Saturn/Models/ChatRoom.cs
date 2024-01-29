namespace Saturn.Models;

public class ChatRoom
{
    [PrimaryKey, AutoIncrement]
    public int ChatId { get; set; }
    public string? Title { get; set; }
    public int SenderId { get; set; }
    public string? LastMessage { get; set; }
    public int NotReadCount { get; set; } = 0;
    public bool HasNotRead { get; set; } = false;
}
