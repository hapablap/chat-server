using ChatProtocol;
using System.Linq;
using System.Net.Sockets;
using System.Text.Json;

namespace ChatServer.MessageHandler
{
    public class DisconnectMessageHandler : IMessageHandler
    {
        public void Execute(Server server, TcpClient client, IMessage message)
        {
            DisconnectMessage disconnectMessage = message as DisconnectMessage;

            User user = server.GetUsers().Find(u => u.SessionIds.Contains(disconnectMessage.SessionId));

            if(user != null)
            {
                user.SessionIds.Remove(disconnectMessage.SessionId);
                server.RemoveClient(client);

                if (user.SessionIds.Count == 0)
                {
                    // Send user count to all clients (broadcast)
                    UserCountMessage userCountMessage = new UserCountMessage
                    {
                        UserCount = server.GetUsers().Count,
                        UserOnlineCount = server.GetUsers().Count(u => u.SessionIds.Count > 0)
                    };

                    string userCountMessageJson = JsonSerializer.Serialize(userCountMessage);
                    byte[] userCountMessageBytes = System.Text.Encoding.UTF8.GetBytes(userCountMessageJson);

                    foreach (TcpClient remoteClient in server.GetClients())
                    {
                        remoteClient.GetStream().Write(userCountMessageBytes, 0, userCountMessageBytes.Length);
                    }
                }
            }
        }
    }
}
