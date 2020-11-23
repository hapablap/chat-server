using ChatProtocol;
using System.Net.Sockets;
using System.Text.Json;

namespace ChatServer.MessageHandler
{
    public class ChatMessageHandler : IMessageHandler
    {
        public void Execute(Server server, TcpClient client, IMessage message)
        {
            ChatMessage chatMessage = message as ChatMessage;

            User user = server.GetUsers().Find(u => u.SessionIds.Contains(chatMessage.SessionId));

            if (user != null)
            {
                chatMessage.SessionId = string.Empty;
                chatMessage.UserId = user.Id;
                string json = JsonSerializer.Serialize(chatMessage);
                byte[] msg = System.Text.Encoding.UTF8.GetBytes(json);

                foreach (TcpClient remoteClient in server.GetClients())
                {
                    remoteClient.GetStream().Write(msg, 0, msg.Length);
                }
            }
        }
    }
}
