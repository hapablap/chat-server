namespace ChatProtocol
{
    public class UserRegisterResponseMessage : IMessage
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public int MessageId
        {
            get
            {
                return 9;
            }
            set { }
        }
    }
}
