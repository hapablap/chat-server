namespace ChatProtocol
{
    public class UserRegisterMessage : IMessage
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public int MessageId
        {
            get
            {
                return 8;
            }
            set { }
        }
    }
}
