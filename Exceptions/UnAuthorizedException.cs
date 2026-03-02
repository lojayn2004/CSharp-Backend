namespace Exceptions
{
    public class UnAuthorizedException: Exception
    {
        public UnAuthorizedException(string errorMessage): base(errorMessage)
        {
            
        }
        
    }
}