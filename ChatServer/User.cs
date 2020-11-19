using System.Collections.Generic;

namespace ChatServer
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public List<string> SessionIds = new List<string>();
    }
}
