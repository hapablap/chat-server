using ChatProtocol;
using System;
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
                user.SessionId = Guid.NewGuid().ToString();
                connectResponseMessage.SessionId = user.SessionId;
                server.AddClient(client);
                Console.WriteLine("Client connected.");
            }

            connectResponseMessage.Success = authenticated;

            string json = JsonSerializer.Serialize(connectResponseMessage);
            byte[] msg = System.Text.Encoding.UTF8.GetBytes(json);
            client.GetStream().Write(msg, 0, msg.Length);
        }
    }
}
