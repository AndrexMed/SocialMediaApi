namespace SocialMedia.Core.Exceptions
{
    public class BusinessException : Exception
    {
        public int MyProperty { get; set; }

        public BusinessException()
        {
            
        }

        public BusinessException(string message) : base(message)
        {
            
        }
    }
}
