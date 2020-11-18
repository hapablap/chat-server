using ChatProtocol;
using System.Net.Sockets;

namespace ChatServer.MessageHandler
{
    public interface IMessageHandler
    {
        public void Execute(Server server, TcpClient client, IMessage message);
    }
}
