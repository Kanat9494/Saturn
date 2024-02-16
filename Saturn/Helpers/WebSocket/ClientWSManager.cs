using System.Net.WebSockets;

namespace Saturn.Helpers.WebSocket;

public class ClientWSManager
{
    public ClientWSManager()
    {

    }

    private ulong _userId;
    private ulong _receiverId;
    private static ClientWebSocket _clientWS;
    static string _uri = ServerConstants.WS_SERVER + $"api/Chats/ConnectTOWS?userId=";

    protected internal void ConnectToWSServer(ulong userId)
    {
        try
        {
            _userId = userId;
            _clientWS = new ClientWebSocket();


            Task.Run(async () =>
            {
                await ReceiveMessage();
            });
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
                var receiveResult = await _clientWS.ReceiveAsync(new ArraySegment<byte>(data), CancellationToken.None);
                messageBuilder.Append(Encoding.UTF8.GetString(data, 0, receiveResult.Count));
                ClientWSHelper.NotifyWSMessageReceivedEvent(messageBuilder.ToString());

                while (receiveResult.EndOfMessage == false)
                {
                    // Увеличиваем размер массива вдвое
                    Array.Resize(ref data, data.Length * 2);
                    receiveResult = await _clientWS.ReceiveAsync(new ArraySegment<byte>(data, receiveResult.Count, data.Length - receiveResult.Count), CancellationToken.None);
                    messageBuilder.Append(Encoding.UTF8.GetString(data, receiveResult.Count, data.Length - receiveResult.Count));
                    ClientWSHelper.NotifyWSMessageReceivedEvent(messageBuilder.ToString());
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

    internal static async Task SendMessageAsync(string jsonMessage)
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
}
