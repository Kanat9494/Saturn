namespace Saturn.ViewModels.Chat;

internal class ChatViewModel : BaseViewModel, IQueryAttributable
{
    public ChatViewModel(LocalMessagesService messagesService, LocalChatsService chatsService)
    {
        _messagesService = messagesService;
        _chatsService = chatsService;


        Messages = new ObservableCollection<Message>();

        SendCommand = new AsyncRelayCommand(OnSend);

        _userId = 1;
    }

    private readonly LocalMessagesService _messagesService;
    private readonly LocalChatsService _chatsService;
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
        Chat.HasNotRead = false;
        Chat.NotReadCount = 0;
        await _chatsService.SaveItemAsync(Chat);
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


        var message = new Message
        {
            ReceiverId = Chat.SenderId,
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

    private void HandleMessageReceived(object sender, string jsonMessage)
    {
        Task.Run(async () =>
        {
            await HasUserMessage(jsonMessage);
        });
    }

    async Task CreateLocalMessage(Message message)
    {
        await _messagesService.SaveItemAsync(message);
        Debug.WriteLine(message.MessageId + "  " + message.ChatId + " userId: " + message.ReceiverId);
    }

    async Task HasUserMessage(string jsonMessage)
    {
        var message = JsonConvert.DeserializeObject<Message>(jsonMessage);
        var chatId = await _chatsService.HasUserChat(message.SenderId);
        message.ChatId = chatId;
        try
        {
            await _messagesService.SaveItemAsync(message);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    internal void OnApearing()
    {
        RTServerManager.MessageReceivedEvent += HandleMessageReceived;
    }

    internal void OnDisappearing()
    {
        RTServerManager.MessageReceivedEvent -= HandleMessageReceived;
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
