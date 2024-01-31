namespace Saturn.ViewModels.Chat;

internal class ChatViewModel : BaseViewModel, IQueryAttributable
{
    public ChatViewModel(LocalMessagesService messagesService)
    {
        _messagesService = messagesService;

        Messages = new ObservableCollection<Message>();

        SendCommand = new AsyncRelayCommand(OnSend);

        _userId = 1;
    }

    private readonly LocalMessagesService _messagesService;
    public ObservableCollection<Message> Messages { get; set; }

    private int _userId;

    public ICommand SendCommand { get; set; }

    private ChatRoom _chat;
    public ChatRoom Chat
    {
        get => _chat;
        set => SetProperty(ref _chat, value);
    }
    private string _messageText;
    public string MessageText
    {
        get => _messageText;
        set => SetProperty(ref _messageText, value);
    }

    async Task InitializeChatMessages()
    {
        var messages = await _messagesService.GetChatMessages(Chat.ChatId);
        if (messages != null)
        {
            for (int i = 0; i < messages.Count; i++)
            {
                Messages.Add(messages[i]);
            }
        }
    }

    private async Task OnSend()
    {
        if (string.IsNullOrWhiteSpace(MessageText))
            return;

        var receiverId = await _messagesService.GetReceiverId(Chat.ChatId);

        var message = new Message
        {
            ReceiverId = receiverId,
            SenderId = _userId,
            Content = MessageText,
            SentDate = DateTime.Now,
            ChatId = Chat.ChatId,
        };
        var jsonMessage = JsonConvert.SerializeObject(message);
        await RTServerManager.SendMessageAsync(jsonMessage);
        await _messagesService.SaveItemAsync(message);
        Messages.Add(message);

        MessageText = string.Empty;
    }

    #region Query params
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        //ChatId = int.Parse(HttpUtility.UrlDecode(query["ChatId"]?.ToString() ?? "1"));
        Chat = query["Chat"] as ChatRoom;

        Task.Run(async () =>
        {
            await InitializeChatMessages();
        });
    }
    #endregion
}
