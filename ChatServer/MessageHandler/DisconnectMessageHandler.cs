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
            var disconnectMessage = message as DisconnectMessage;

            var user = server.GetUsers().Find(u => u.SessionIds.Contains(disconnectMessage.SessionId));

            if(user != null)
            {
                user.SessionIds.Remove(disconnectMessage.SessionId);
                server.RemoveClient(client);

                if (user.SessionIds.Count == 0)
                {
                    // Send user count to all clients (broadcast)
                    var userCountMessage = new UserCountMessage
                    {
                        UserCount = server.GetUsers().Count,
                        UserOnlineCount = server.GetUsers().Count(u => u.SessionIds.Count > 0)
                    };

                    var userCountMessageJson = JsonSerializer.Serialize(userCountMessage);
                    var userCountMessageBytes = System.Text.Encoding.UTF8.GetBytes(userCountMessageJson);

                    foreach (var remoteClient in server.GetClients())
                    {
                        remoteClient.GetStream().Write(userCountMessageBytes, 0, userCountMessageBytes.Length);
                    }
                }
            }
        }
    }
}
