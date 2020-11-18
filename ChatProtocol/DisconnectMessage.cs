namespace ChatProtocol
{
    public class DisconnectMessage : IMessage
    {
        public int MessageId
        {
            get
            {
                return 3;
            }
            set { }
        }
    }
}
