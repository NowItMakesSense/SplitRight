namespace SplitRight.Domain.Exceptions
{
    public class OwnershipViolationException : Exception
    {
        public OwnershipViolationException(string message) : base(message)
        {
        }
    }
}
