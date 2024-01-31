namespace Saturn.ViewModels.Chat;

internal class ChatsViewModel : BaseViewModel
{
    public ChatsViewModel(LocalChatsService chatsService, LocalMessagesService messagesService)
    {
        _chatsService = chatsService;
        _messagesService = messagesService;
        ChatCommand = new AsyncRelayCommand<ObservableChatRoom>(OnChat);

        Chats = new ObservableCollection<ObservableChatRoom>();
        AuthFields.UserId = 1;

        Task.Run(InitializeChats);

        //ConnectToRTCServer();
        //RTServerManager.ConnectToRTCServer(1, 54);
    }

    private readonly LocalChatsService _chatsService;
    private readonly LocalMessagesService _messagesService;

    private int _userId;
    private int _receiverId;
    private TcpClient? _tcpClient;
    private NetworkStream? _stream;

    private string _title;

    public ICommand ChatCommand { get; set; }

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

    void ConnectToRTCServer()
    {
        try
        {
            _userId = 1;
            _receiverId = 2;

            _tcpClient = new TcpClient();

            _tcpClient.Connect(ServerConstants.RTC_HOST, ServerConstants.RTC_PORT);
            _stream = _tcpClient.GetStream();

            var message = new
            {
                SenderId = _userId,
                ReceiverName = _receiverId,
                Content = "Подключение"
            };

            var jsonMessage = JsonConvert.SerializeObject(message);
            byte[] data = Encoding.UTF8.GetBytes(jsonMessage);
            _stream.Write(data, 0, data.Length);



            Task receiveTask = new Task(async () => await ReceiveMessage());
            receiveTask.Start();

            //Thread receivedThread = new Thread(ReceiveMessage);
            //receivedThread.Start();
        }
        catch (Exception ex)
        {

        }
    }

    async Task ReceiveMessage()
    {
        byte[] data = new byte[64];

        while (true)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                int bytes = 0;
                do
                {
                    bytes = await _stream.ReadAsync(data, 0, data.Length);
                    builder.Append(Encoding.UTF8.GetString(data, 0, bytes));

                    if (bytes == data.Length)
                        Array.Resize(ref data, data.Length * 2);

                    Debug.WriteLine(builder);
                }
                while (_stream.DataAvailable);

                await HasUserChat(builder.ToString());
                //Debug.WriteLine(c);
            }
            catch (Exception ex)
            {
                Disconnect();
            }
        }
    }

    private void Disconnect()
    {
        if (_stream != null)
            _stream.Close();
        if (_tcpClient != null)
            _tcpClient.Close();

        //Environment.Exit(0);
    }

    private void HandleMessageReceived(object sender, string jsonMessage)
    {
        Task.Run(async () =>
        {
            await HasUserChat(jsonMessage);
        });
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

    internal void OnAppearing()
    {
        RTServerManager.MessageReceivedEvent += HandleMessageReceived;

    }

    internal void OnDisappearing()
    {
        RTServerManager.MessageReceivedEvent -= HandleMessageReceived;

    }
}
