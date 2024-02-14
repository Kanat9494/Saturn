namespace Saturn.ViewModels.Chat;

internal class ChatsViewModel : BaseViewModel
{
    public ChatsViewModel(LocalChatsService chatsService, LocalMessagesService messagesService)
    {
        _chatsService = chatsService;
        _messagesService = messagesService;
        ChatCommand = new AsyncRelayCommand<ObservableChatRoom>(OnChat);
        NewChatRoomCommand = new AsyncRelayCommand(OnNewChatRoom);

        Chats = new ObservableCollection<ObservableChatRoom>();
        //RTMessageHelper.ChatLMChangedEvent += HandleChatLMChanged;
        ClientWSHelper.ChatLMChangedEvent += HandleChatLMChanged;

        Task.Run(InitializeChats);
    }

    private readonly LocalChatsService _chatsService;
    private readonly LocalMessagesService _messagesService;

    private int _userId;
    private int _receiverId;
    private TcpClient? _tcpClient;
    private NetworkStream? _stream;

    private string _title;

    public ICommand ChatCommand { get; }
    public ICommand NewChatRoomCommand { get; }

    ObservableChatRoom observableChat;
    ChatRoom chat;



    public ObservableCollection<ObservableChatRoom> Chats { get; set; }

    private async Task InitializeChats()
    {
        var chats = await _chatsService.GetItemsAsync();

        if (chats != null)
        {
            for (int i = 0; i < chats.Count; i++)
                Chats.Add(new ObservableChatRoom(chats[i]));
        }
    }

    private void HandleMessageReceived(object sender, string jsonMessage)
    {
        Task.Run(async () =>
        {
            await HasUserChat(jsonMessage);
        });
    }

    private void HandleChatLMChanged(object sender, int chatId, string lastMessage, bool isOtherChat)
    {
        observableChat = Chats.FirstOrDefault(c => c.ChatId == chatId);
        int i = Chats.IndexOf(observableChat);
        Chats[i].LastMessage = lastMessage;
        if (isOtherChat)
        {
            Chats[i].HasNotRead = true;
            Chats[i].NotReadCount++;
        }
    }

    private async Task HasUserChat(string json)
    {
        try
        {
            var message = JsonConvert.DeserializeObject<Message>(json);
            var chatId = await _chatsService.HasUserChat(message.SenderId);

            if (chatId > 0)
            {
                await CreateLocalMessage(chatId, message);
                chat = await _chatsService.GetItemAsync(chatId);
                chat.Title = $"Aika + {message.SenderId}";
                _title = chat.Title;
                chat.SenderId = message.SenderId;
                chat.LastMessage = message.Content;
                chat.NotReadCount++;
                chat.HasNotRead = true;
                await _chatsService.SaveItemAsync(chat);
                chatId = chat.ChatId;


                if (!Chats.Any(c => c.ChatId == chatId))
                    Chats.Add(new ObservableChatRoom(chat));
                else
                {
                    observableChat = Chats.FirstOrDefault(c => c.ChatId == chatId);
                    int i = Chats.IndexOf(observableChat!);
                    Chats[i].LastMessage = message.Content;
                    Chats[i].HasNotRead = true;
                    Chats[i].NotReadCount++;
                }
            }
            else
            {
                chat = new ChatRoom
                {
                    Title = $"Aika {message.SenderId}",
                    SenderId = message.SenderId,
                    LastMessage = message.Content,
                    NotReadCount = 1,
                    HasNotRead = true
                };
                _title = chat.Title;
                await _chatsService.SaveItemAsync(chat);
                chatId = await _chatsService.HasUserChat(message.SenderId);
                await CreateLocalMessage(chatId, message);


                if (!Chats.Any(c => c.ChatId == chatId))
                    Chats.Add(new ObservableChatRoom(chat));
                else
                {
                    observableChat = Chats.FirstOrDefault(c => c.ChatId == chatId);
                    int i = Chats.IndexOf(observableChat);
                    Chats[i].LastMessage = message.Content;
                    Chats[i].HasNotRead = true;
                    Chats[i].NotReadCount++;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    async Task CreateLocalMessage(int chatId, Message message)
    {
        message.ChatId = chatId;
        await _messagesService.SaveItemAsync(message);
    }

    private async Task OnChat(ObservableChatRoom? chat)
    {
        //await Shell.Current.GoToAsync($"ChatPage?Chat?{chat}");
        var navigationParameter = new ShellNavigationQueryParameters
        {
            {"Chat", chat }
        };

        await Shell.Current.GoToAsync("ChatPage", navigationParameter);
    }

    #region app lifecycle
    internal void OnAppearing()
    {
        //RTMessageHelper.MessageReceivedEvent += HandleMessageReceived;
        ClientWSHelper.MessageReceivedEvent += HandleMessageReceived;

    }

    internal void OnDisappearing()
    {
        //RTMessageHelper.MessageReceivedEvent -= HandleMessageReceived;
        ClientWSHelper.MessageReceivedEvent -= HandleMessageReceived;

    }
    #endregion

    private async Task OnNewChatRoom()
    {
        string result = await Shell.Current.DisplayPromptAsync("Новый чат", "Кому вы хотите отправить сообщение", keyboard: Keyboard.Numeric);

        if (string.IsNullOrWhiteSpace(result))
            return;

        chat = new ChatRoom
        {
            Title = $"Aika {result}",
            SenderId = AuthFields.UserId,
            ReceiverId = int.Parse(result),
            LastMessage = "",
            NotReadCount = 0,
            HasNotRead = false
        };

        await _chatsService.SaveItemAsync(chat);
        int chatId = await _chatsService.HasUserChat(chat.SenderId);
        Chats.Add(new ObservableChatRoom(chat));
        await OnChat(new ObservableChatRoom(chat));
    }
}
