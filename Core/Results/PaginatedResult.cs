namespace Core.Results;

public class PaginatedResult<T> : Result<PaginatedResponse<T>>
{
    protected PaginatedResult(PaginatedResponse<T> value) : base(value)
    {
    }

    protected PaginatedResult(Error error) : base(error)
    {
    }

    public static implicit operator PaginatedResult<T>(PaginatedResponse<T> value)
    {
        return new PaginatedResult<T>(value);
    }
    
    public static implicit operator PaginatedResult<T>(Error error)
    {
        return new PaginatedResult<T>(error);
    }
}