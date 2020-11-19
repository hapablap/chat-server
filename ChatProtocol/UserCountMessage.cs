namespace ChatProtocol
{
    public class UserCountMessage : IMessage
    {
        public int UserCount { get; set; }
        public int UserOnlineCount { get; set; }

        public int MessageId
        {
            get
            {
                return 5;
            }
            set { }
        }
    }
}
