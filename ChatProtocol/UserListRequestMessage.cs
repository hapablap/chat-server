namespace ChatProtocol
{
    public class UserListRequestMessage : IMessage
    {
        public string SessionId { get; set; }

        public int MessageId
        {
            get
            {
                return 6;
            }
            set { }
        }
    }
}
