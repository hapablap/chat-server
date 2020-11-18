using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace ChatServer
{
    public class Server
    {
        private int port;
        private string ipAddress;
        private string password;

        private TcpListener tcpListener;

        private List<TcpClient> clients = new List<TcpClient>();

        public Server(int port, string ipAddress)
        {
            this.port = port;
            this.ipAddress = ipAddress;
        }

        public void Start()
        {
            IPAddress localAddress = IPAddress.Parse(ipAddress);
            tcpListener = new TcpListener(localAddress, port);
            tcpListener.Start();
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

        public List<TcpClient> GetClients()
        {
            return clients;
        }

        public TcpClient AcceptTcpClient()
        {
            return tcpListener.AcceptTcpClient();
        }
    }
}
