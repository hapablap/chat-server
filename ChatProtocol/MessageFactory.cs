﻿using System.Text.Json;

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
                case 5:
                    return JsonSerializer.Deserialize<UserCountMessage>(json);
                case 6:
                    return JsonSerializer.Deserialize<UserListRequestMessage>(json);
                case 7:
                    return JsonSerializer.Deserialize<UserListResponseMessage>(json);
                case 8:
                    return JsonSerializer.Deserialize<UserRegisterMessage>(json);
                case 9:
                    return JsonSerializer.Deserialize<UserRegisterResponseMessage>(json);
            }

            return null;
        }
    }
}
