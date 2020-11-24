using ChatProtocol;
using System.Net.Sockets;
using System.Text.Json;

namespace ChatServer.MessageHandler
{
    public class ChatMessageHandler : IMessageHandler
    {
        public void Execute(Server server, TcpClient client, IMessage message)
        {
            var chatMessage = message as ChatMessage;

            var user = server.GetUsers().Find(u => u.SessionIds.Contains(chatMessage.SessionId));

            if (user != null)
            {
                chatMessage.SessionId = string.Empty;
                chatMessage.UserId = user.Id;
                var json = JsonSerializer.Serialize(chatMessage);
                var msg = System.Text.Encoding.UTF8.GetBytes(json);

                foreach (var remoteClient in server.GetClients())
                {
                    remoteClient.GetStream().Write(msg, 0, msg.Length);
                }
            }
        }
    }
}
