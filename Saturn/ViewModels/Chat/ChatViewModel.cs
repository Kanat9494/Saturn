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

    private int _chatId;
    public int ChatId
    {
        get => _chatId;
        set => SetProperty(ref _chatId, value);
    }
    private string _title;
    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }
    private string _messageText;
    public string MessageText
    {
        get => _messageText;
        set => SetProperty(ref _messageText, value);
    }

    async Task InitializeChatMessages()
    {
        var messages = await _messagesService.GetChatMessages(ChatId);
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

        var message = new Message
        {
            ReceiverId = 54,
            SenderId = _userId,
            Content = MessageText,
            SentDate = DateTime.Now,
            ChatId = this.ChatId,
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
        ChatId = int.Parse(HttpUtility.UrlDecode(query["ChatId"]?.ToString() ?? "1"));
        Title = HttpUtility.UrlDecode(query["Title"]?.ToString() ?? "");

        Task.Run(async () =>
        {
            await InitializeChatMessages();
        });
    }
    #endregion
}
