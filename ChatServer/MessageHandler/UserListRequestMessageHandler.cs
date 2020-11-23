using ChatProtocol;
using System;
using System.Collections.Generic;
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
            UserListRequestMessage userListRequestMessage = message as UserListRequestMessage;

            User user = server.GetUsers().Find(u => u.SessionIds.Contains(userListRequestMessage.SessionId));

            if(user != null)
            {
                var query = from u in server.GetUsers() select new { u.Id, u.Username };
                var userList = query.ToList();
                string userListJson = JsonSerializer.Serialize(userList);
                UserListResponseMessage userListResponseMessage = new UserListResponseMessage
                {
                    UserListJson = userListJson
                };

                string messageJson = JsonSerializer.Serialize(userListResponseMessage);
                byte[] byteMessage = Encoding.UTF8.GetBytes(messageJson);
                client.GetStream().Write(byteMessage, 0, byteMessage.Length);
            }
        }
    }
}
