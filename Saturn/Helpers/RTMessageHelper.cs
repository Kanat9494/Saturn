namespace Saturn.Helpers;

public delegate void MessageReceivedEventHandler(object sender, string jsonMessage);

internal static class RTMessageHelper
{
    internal static event MessageReceivedEventHandler? MessageReceivedEvent;

    internal static void NotifyMessageReceivedEvent(string jsonMessage)
    {
        MessageReceivedEvent?.Invoke(null, jsonMessage);
    }
}
