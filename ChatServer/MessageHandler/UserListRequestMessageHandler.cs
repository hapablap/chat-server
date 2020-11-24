using ChatProtocol;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace ChatServer.MessageHandler
{
    public class UserListRequestMessageHandler : IMessageHandler
    {
        public void Execute(Server server, TcpClient client, IMessage message)
        {
            var userListRequestMessage = message as UserListRequestMessage;

            var user = server.GetUsers().Find(u => u.SessionIds.Contains(userListRequestMessage.SessionId));

            if(user != null)
            {
                var query = from u in server.GetUsers() select new { u.Id, u.Username };
                var userList = query.ToList();
                var userListJson = JsonSerializer.Serialize(userList);
                var userListResponseMessage = new UserListResponseMessage
                {
                    UserListJson = userListJson
                };

                var messageJson = JsonSerializer.Serialize(userListResponseMessage);
                var byteMessage = Encoding.UTF8.GetBytes(messageJson);
                client.GetStream().Write(byteMessage, 0, byteMessage.Length);
            }
        }
    }
}
