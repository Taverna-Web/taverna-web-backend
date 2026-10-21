namespace Domain.Exceptions;

public class InputException : DomainException
{
    public InputException(string message) : base(message)
    {
    }

    public InputException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
