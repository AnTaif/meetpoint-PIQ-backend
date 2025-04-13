namespace Core.Results;

public class Error
{
    public string Message { get; }

    protected Error(string message)
    {
        Message = message;
    }
    
    public static Error Failure(string message = "") => new(message);
}