﻿namespace Saturn.Helpers;

internal static class RTServerManager
{
    private static int _userId;
    private static int _receiverId;
    private static TcpClient? _tcpClient;
    private static NetworkStream? _stream;

    internal static void ConnectToRTCServer(int userId, int receiverId)
    {
        try
        {
            _userId = userId;
            _receiverId = receiverId;
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
        }
        catch (Exception ex)
        {

        }
    }

    internal static async Task SendMessageAsync(string jsonMessage)
    {
        byte[] data = Encoding.UTF8.GetBytes(jsonMessage);
        await _stream.WriteAsync(data, 0, data.Length);
    }

    internal static async Task ReceiveMessage()
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

                    RTMessageHelper.NotifyMessageReceivedEvent(builder.ToString());


                }
                while (_stream.DataAvailable);

            }
            catch (Exception ex)
            {
                Disconnect();
            }
        }
    }

    private static void Disconnect()
    {
        if (_stream != null)
            _stream.Close();
        if (_tcpClient != null)
            _tcpClient.Close();

        //Environment.Exit(0);
    }
}
