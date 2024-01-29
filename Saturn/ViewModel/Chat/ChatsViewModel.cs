namespace Saturn.ViewModel.Chat;

internal class ChatsViewModel : BaseViewModel
{
    public ChatsViewModel(LocalChatsService chatsService)
    {
        _chatsService = chatsService;
        Chats = new ObservableCollection<ChatRoom>();

        Task.Run(InitializeChats);

        ConnectToRTCServer();
    }

    private readonly LocalChatsService _chatsService;

    private int _userId;
    private int _receiverId;
    private TcpClient? _tcpClient;
    private NetworkStream? _stream;

    public ObservableCollection<ChatRoom> Chats { get; set; }

    private async Task InitializeChats()
    {
        var chats = await _chatsService.GetItemsAsync();

        if (chats != null & chats.Count > 0)
        {
            for (int i = 0; i < chats.Count; i++)
                Chats.Add(chats[i]);
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

    void SendMessage()
    {
        var message = new
        {
            SenderName = _userId.ToString(),
            ReceiverName = _receiverId.ToString(),
            Content = "Новое сообщение"
        };

        var jsonMessage = JsonConvert.SerializeObject(message);
        byte[] data = Encoding.UTF8.GetBytes(jsonMessage);
        _stream.Write(data, 0, data.Length);
    }

    async Task ReceiveMessage()
    {
        while (true)
        {
            try
            {
                byte[] data = new byte[64];
                StringBuilder builder = new StringBuilder();
                int bytes = 0;
                do
                {
                    bytes = await _stream.ReadAsync(data, 0, data.Length);
                    builder.Append(Encoding.UTF8.GetString(data, 0, bytes));

                    if (bytes == data.Length)
                        Array.Resize(ref data, data.Length * 2);
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

    private async Task HasUserChat(string json)
    {
        try
        {
            ChatRoom chat;
            var message = JsonConvert.DeserializeObject<Message>(json);
            var chatId = await _chatsService.HasUserChat(message.SenderId);

            if (chatId > 0)
            {
                chat = await _chatsService.GetItemAsync(chatId);
                chat.Title = $"Aika + {message.SenderId}";
                chat.SenderId = message.SenderId;
                chat.LastMessage = message.Content;
                chat.NotReadCount = 1;
                chat.HasNotRead = true;
                await _chatsService.SaveItemAsync(chat);
                chatId = chat.ChatId;
                if (!Chats.Any(c => c.ChatId == chatId))
                    Chats.Add(chat);
                else
                {
                    chat = Chats.FirstOrDefault(c => c.ChatId == chatId);
                    int i = Chats.IndexOf(chat);
                    Chats[i].LastMessage = message.Content;
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
                await _chatsService.SaveItemAsync(chat);
                

                if (!Chats.Any(c => c.ChatId == chatId))
                    Chats.Add(chat);
                else
                {
                    chat = Chats.FirstOrDefault(c => c.ChatId == chatId);
                    int i = Chats.IndexOf(chat);
                    Chats[i].LastMessage = message.Content;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
}
