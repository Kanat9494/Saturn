namespace Saturn.Helpers.WebSocket;

internal delegate void WSMessageReceivedEventHandler(object sender, string jsonMessage);
internal delegate void WSChatLMChangedEventHandler(object sender, int chatId, string lastMessage, bool isOtherChat, int senderId, int receiverId);

public class ClientWSManager
{
    public ClientWSManager()
    {
        _clientWS = new ClientWebSocket();
    }

    internal event WSMessageReceivedEventHandler? MessageReceivedEvent;
    internal event WSChatLMChangedEventHandler? ChatLMChangedEvent;

    private ulong _userId;
    private ulong _receiverId;
    private ClientWebSocket _clientWS;
    string _uri = ServerConstants.WS_SERVER + $"api/Chats/ConnectTOWS?userId=";
    protected internal bool _isConnected = false;

    protected internal void ConnectToWSServer(ulong userId)
    {
        try
        {
            _userId = userId;
            


            Task.Run(ReceiveMessage);
        }
        catch (Exception ex)
        {

        }
    }

    internal async Task ReceiveMessage()
    {
        try
        {
            await _clientWS.ConnectAsync(new Uri(_uri + _userId), CancellationToken.None);
            byte[] data = new byte[1024 * 4];

            StringBuilder messageBuilder = new StringBuilder();
            while (true)
            {
                _isConnected = true;
                var receiveResult = await _clientWS.ReceiveAsync(new ArraySegment<byte>(data), CancellationToken.None);
                messageBuilder.Append(Encoding.UTF8.GetString(data, 0, receiveResult.Count));
                NotifyWSMessageReceivedEvent(messageBuilder.ToString());

                while (receiveResult.EndOfMessage == false)
                {
                    // Увеличиваем размер массива вдвое
                    Array.Resize(ref data, data.Length * 2);
                    receiveResult = await _clientWS.ReceiveAsync(new ArraySegment<byte>(data, receiveResult.Count, data.Length - receiveResult.Count), CancellationToken.None);
                    messageBuilder.Append(Encoding.UTF8.GetString(data, receiveResult.Count, data.Length - receiveResult.Count));
                    NotifyWSMessageReceivedEvent(messageBuilder.ToString());
                    messageBuilder.Clear(); // Очищаем StringBuilder для следующей итерации
                }
                messageBuilder.Clear();
            }
        }
        catch (Exception ex)
        {
            //await Shell.Current.DisplayAlert("Ошибка", $"Не удалось подключиться к серверу: {ex.Message}", "Ок");
        }
    }

    internal async Task SendMessageAsync(string jsonMessage)
    {
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(jsonMessage);
            await _clientWS.SendAsync(new ArraySegment<byte>(data, 0, data.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }
        catch (Exception ex)
        {

        }
    }

    internal async Task DisconnectAsync()
    {
        try
        {
            if (_clientWS.State == WebSocketState.Open || _clientWS.State == WebSocketState.CloseSent)
            {
                await _clientWS.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed by client", CancellationToken.None);
                _isConnected = false;
            }
        }
        catch (Exception ex) 
        { 

        }
    }

    internal void NotifyWSMessageReceivedEvent(string jsonMessage)
    {
        MessageReceivedEvent?.Invoke(null, jsonMessage);
    }

    internal void NotifyWSChatLMChangedEvent(int chatId, string lastMessage, bool isOtherChat, int senderId, int receiverId)
    {
        ChatLMChangedEvent?.Invoke(null, chatId, lastMessage, isOtherChat, senderId, receiverId);
    }
}
