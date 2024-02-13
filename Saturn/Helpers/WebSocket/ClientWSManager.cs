using System.Net.WebSockets;

namespace Saturn.Helpers.WebSocket;

internal class ClientWSManager
{
    private static ulong _userId;
    private static ulong _receiverId;
    private static ClientWebSocket _clientWS;
    static string _uri = ServerConstants.WS_SERVER + $"api/Chats/ConnectTOWS?userId=";

    internal static void ConnectToWSServer(ulong userId)
    {
        try
        {
            _userId = userId;
            _clientWS = new ClientWebSocket();


            Task.Run(async () =>
            {
                await ReceiveMessage(_userId);
            });
        }
        catch (Exception ex)
        {

        }
    }

    internal static async Task ReceiveMessage(ulong userId)
    {
        try
        {
            await _clientWS.ConnectAsync(new Uri(_uri + userId), CancellationToken.None);
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
