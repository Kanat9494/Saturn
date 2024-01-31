namespace Saturn.Helpers;

public delegate void MessageReceivedEventHandler(object sender, string jsonMessage);
public delegate void ChatLMChangedEventHandler(object sender, int userId, string lastMessage, bool isOtherChat);

internal static class RTMessageHelper
{
    internal static event MessageReceivedEventHandler? MessageReceivedEvent;
    internal static event ChatLMChangedEventHandler? ChatLMChangedEvent;

    internal static void NotifyMessageReceivedEvent(string jsonMessage)
    {
        MessageReceivedEvent?.Invoke(null, jsonMessage);
    }

    internal static void NotifyChatLMChangedEvent(int userId, string lastMessage, bool isOtherChat)
    {
        ChatLMChangedEvent?.Invoke(null, userId, lastMessage, isOtherChat);
    }
}
