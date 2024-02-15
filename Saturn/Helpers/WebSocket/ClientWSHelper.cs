namespace Saturn.Helpers.WebSocket;

internal delegate void WSMessageReceivedEventHandler(object sender, string jsonMessage);
internal delegate void WSChatLMChangedEventHandler(object sender, int chatId, string lastMessage, bool isOtherChat, int senderId, int receiverId);

internal static class ClientWSHelper
{
    internal static event WSMessageReceivedEventHandler? MessageReceivedEvent;
    internal static event WSChatLMChangedEventHandler? ChatLMChangedEvent;

    internal static void NotifyWSMessageReceivedEvent(string jsonMessage)
    {
        MessageReceivedEvent?.Invoke(null, jsonMessage);
    }

    internal static void NotifyWSChatLMChangedEvent(int chatId, string lastMessage, bool isOtherChat, int senderId, int receiverId)
    {
        ChatLMChangedEvent?.Invoke(null, chatId, lastMessage, isOtherChat, senderId, receiverId);
    }
}
