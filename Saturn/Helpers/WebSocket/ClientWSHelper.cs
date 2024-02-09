namespace Saturn.Helpers.WebSocket;

internal delegate void WSMessageReceivedEventHandler(object sender, string jsonMessage);
internal delegate void WSChatLMChangedEventHandler(object sender, int userId, string lastMessage, bool isOtherChat);

internal static class ClientWSHelper
{
    internal static event WSMessageReceivedEventHandler? MessageReceivedEvent;
    internal static event WSChatLMChangedEventHandler? ChatLMChangedEvent;

    internal static void NotifyWSMessageReceivedEvent(string jsonMessage)
    {
        MessageReceivedEvent?.Invoke(null, jsonMessage);
    }

    internal static void NotifyWSChatLMChangedEvent(int userId, string lastMessage, bool isOtherChat)
    {
        ChatLMChangedEvent?.Invoke(null, userId, lastMessage, isOtherChat);
    }
}
