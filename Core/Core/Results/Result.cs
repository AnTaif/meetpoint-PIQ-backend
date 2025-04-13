namespace Core.Results;

public class Result<T>
{
    private readonly T? value;
    
    public T Value => value!;

    private readonly Error? error;
 
    public Error Error => error!;

    public bool IsFailure => error != null;

    protected Result(T value)
    {
        this.value = value;
    }

    protected Result(Error error)
    {
        this.error = error;
    }
    
    public static Result<T> Success(T value) => new(value);

    public static implicit operator Result<T>(T value)
    {
        return new Result<T>(value);
    }

    public static implicit operator Result<T>(Error error)
    {
        return new Result<T>(error);
    }
}

public class Result
{
    private readonly Error? error;

    public Error Error => error!;
    
    public bool IsFailure => error != null;
    
    protected Result()
    {
    }

    protected Result(Error error)
    {
        this.error = error;
    }
    
    public static Result Success() => new();

    public static implicit operator Result(Error error)
    {
        return new Result(error);
    }
}