namespace ChatProtocol
{
    public class UserListResponseMessage : IMessage
    {
        public string UserListJson { get; set; }

        public int MessageId
        {
            get
            {
                return 7;
            }
            set { }
        }
    }
}
