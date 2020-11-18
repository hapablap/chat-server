using System.Text.Json;

namespace ChatProtocol
{
    public static class MessageFactory
    {
        public static IMessage GetMessage(int messageId, string json)
        {
            switch (messageId)
            {
                case 1:
                    return JsonSerializer.Deserialize<ChatMessage>(json);
                case 2:
                    return JsonSerializer.Deserialize<ConnectMessage>(json);
                case 3:
                    return JsonSerializer.Deserialize<DisconnectMessage>(json);
                case 4:
                    return JsonSerializer.Deserialize<ConnectResponseMessage>(json);
            }

            return null;
        }
    }
}
