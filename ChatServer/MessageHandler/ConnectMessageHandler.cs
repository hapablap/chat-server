using ChatProtocol;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Text.Json;

namespace ChatServer.MessageHandler
{
    public class ConnectMessageHandler : IMessageHandler
    {
        public void Execute(Server server, TcpClient client, IMessage message)
        {
            ConnectMessage connectMessage = message as ConnectMessage;

            bool authenticatedServerPassword = true;
            if (server.HasPassword())
            {
                authenticatedServerPassword = server.CheckPassword(connectMessage.ServerPassword);
            }

            User user = server.GetUsers().Find(u => u.Username == connectMessage.Username && u.Password == connectMessage.Password);
            bool authenticatedUser = (user != null);

            bool authenticated = authenticatedServerPassword && authenticatedUser;
            ConnectResponseMessage connectResponseMessage = new ConnectResponseMessage();
            if (authenticated)
            {
                string sessionId = Guid.NewGuid().ToString();
                user.SessionIds.Add(sessionId); 
                connectResponseMessage.SessionId = sessionId;
                server.AddClient(client);
                Console.WriteLine("Client connected.");

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

            connectResponseMessage.Success = authenticated;

            string json = JsonSerializer.Serialize(connectResponseMessage);
            byte[] msg = System.Text.Encoding.UTF8.GetBytes(json);
            client.GetStream().Write(msg, 0, msg.Length);
        }
    }
}
