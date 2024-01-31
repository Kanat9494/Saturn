namespace Saturn.Models;

public class ObservableChatRoom : ObservableObject
{
    public ObservableChatRoom(ChatRoom chat)
    {
        ChatId = chat.ChatId;
        Title = chat.Title;
        SenderId = chat.SenderId;
        LastMessage = chat.LastMessage;
        NotReadCount = chat.NotReadCount;
        HasNotRead = chat.HasNotRead;
    }

    public int ChatId { get; set; }
    public string? Title { get; set; }
    public int SenderId { get; set; }
    private string _lastMessage;
    public string? LastMessage
    {
        get => _lastMessage;
        set => SetProperty(ref _lastMessage, value);
    }
    private int _notReadCount;
    public int NotReadCount
    {
        get => _notReadCount;
        set => SetProperty(ref _notReadCount, value);
    }
    private bool _hasNotRead;
    public bool HasNotRead
    {
        get => _hasNotRead;
        set => SetProperty(ref _hasNotRead, value);
    }
}
