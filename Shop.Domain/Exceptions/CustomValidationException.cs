namespace Shop.Domain.Exceptions;

public class CustomValidationException : Exception
{
    public CustomValidationException(string message) : base(message) { }
}