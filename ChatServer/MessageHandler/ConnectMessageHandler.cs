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

            // @TODO Benutzer authentifizieren

            bool authenticated = true;
            if (server.HasPassword())
            {
                authenticated = server.CheckPassword(connectMessage.ServerPassword);
            }

            if (authenticated)
            {
                server.AddClient(client);
                Console.WriteLine("Client connected.");
            }

            ConnectResponseMessage connectResponseMessage = new ConnectResponseMessage();
            connectResponseMessage.Success = authenticated;
            string json = JsonSerializer.Serialize(connectResponseMessage);
            byte[] msg = System.Text.Encoding.UTF8.GetBytes(json);
            client.GetStream().Write(msg, 0, msg.Length);
        }
    }
}
