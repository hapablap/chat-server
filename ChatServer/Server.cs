using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

namespace ChatServer
{
    public class Server
    {
        const string USERS_FILE = "users.json";

        private int port;
        private string ipAddress;
        private string password;

        private TcpListener tcpListener;

        private List<TcpClient> clients = new List<TcpClient>();
        private List<User> users = new List<User>();

        public int UsernameMinLength = 3;
        public int PasswordMinLength = 3;

        public Server(int port, string ipAddress)
        {
            this.port = port;
            this.ipAddress = ipAddress;
        }

        public void Start()
        {
            var localAddress = IPAddress.Parse(ipAddress);
            tcpListener = new TcpListener(localAddress, port);
            tcpListener.Start();

            var userJson = File.ReadAllText(USERS_FILE);
            users = JsonSerializer.Deserialize<List<User>>(userJson);
        }

        public bool HasPassword()
        {
            return !string.IsNullOrEmpty(password);
        }

        public void SetPassword(string password)
        {
            this.password = password;
        }

        public bool CheckPassword(string password)
        {
            return this.password == password;
        }

        public void Stop()
        {
            foreach (var client in clients)
            {
                client.Close();
            }
            tcpListener.Stop();
        }

        public void AddClient(TcpClient client)
        {
            clients.Add(client);
        }

        public void RemoveClient(TcpClient client)
        {
            clients.Remove(client);
        }

        public List<TcpClient> GetClients()
        {
            return clients;
        }

        public void AddUser(User user)
        {
            users.Add(user);
        }

        public List<User> GetUsers()
        {
            return users;
        }

        public void SaveUsers()
        {
            var userJson = JsonSerializer.Serialize(users);
            File.WriteAllText(USERS_FILE, userJson);
        }

        public TcpClient AcceptTcpClient()
        {
            return tcpListener.AcceptTcpClient();
        }
    }
}
