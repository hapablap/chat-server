using ChatProtocol;
using ChatServer.Extension;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace ChatServer.MessageHandler
{
    public class UserRegisterMessageHandler : IMessageHandler
    {
        private string errorMessage;

        public void Execute(Server server, TcpClient client, IMessage message)
        {
            var userRegisterMessage = message as UserRegisterMessage;

            var userRegisterResponseMessage = new UserRegisterResponseMessage
            {
                Success = false
            };

            if(IsValid(server, userRegisterMessage.Username, userRegisterMessage.Password))
            {
                var user = new User
                {
                    Id = server.GetNextUserId(),
                    Username = userRegisterMessage.Username.Trim(),
                    Password = userRegisterMessage.Password,
                    SessionIds = new List<string>()
                };
                server.AddUser(user);
                server.AddClient(client);
                server.SaveUsers();

                userRegisterResponseMessage.Success = true;
            }
            else
            {
                userRegisterResponseMessage.ErrorMessage = errorMessage;
            }

            var messageJson = JsonSerializer.Serialize(userRegisterResponseMessage);
            var byteMessage = Encoding.UTF8.GetBytes(messageJson);
            client.GetStream().Write(byteMessage, 0, byteMessage.Length);
        }

        private bool IsValid(Server server, string username, string password)
        {
            var stringBuilder = new StringBuilder();

            if(!username.IsOneWord())
            {
                stringBuilder.AppendLine("Username must have one word.");
            }

            if(password.Length < server.PasswordMinLength)
            {
                stringBuilder.AppendLine($"Password length must be at least {server.PasswordMinLength}.");
            }

            if (username.Length < server.UsernameMinLength)
            {
                stringBuilder.AppendLine($"Username length must be at least {server.UsernameMinLength}.");
            }

            if (server.GetUsers().Exists(u => u.Username == username))
            {
                stringBuilder.AppendLine("Username already exists.");
            }

            errorMessage = stringBuilder.ToString();

            return errorMessage.Length == 0;
        }
    }
}
