using System.Linq;

namespace ChatServer.Extension
{
    public static class ServerExtension
    {
        public static int UserCount(this Server server)
        {
            return server.GetUsers().Count;
        }

        public static int GetNextUserId(this Server server)
        {
            var user = server.GetUsers().OrderByDescending(user => user.Id).First();
            if (user == null)
                return 1;
            else
                return user.Id + 1;
        }
    }
}
