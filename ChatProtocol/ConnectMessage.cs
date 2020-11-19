namespace ChatProtocol
{
    public class ConnectMessage : IMessage
    {
        public string ServerPassword { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public int MessageId
        {
            get
            {
                return 2;
            }
            set { }
        }
    }
}
