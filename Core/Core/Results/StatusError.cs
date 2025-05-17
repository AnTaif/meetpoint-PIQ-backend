using System.Net;

namespace Core.Results;

public class StatusError : Error
{
    public HttpStatusCode HttpStatus { get; }

    protected StatusError(string message, HttpStatusCode httpStatus) : base(message)
    {
        HttpStatus = httpStatus;
    }

    public static StatusError NotFound(string message = "") => new(message, HttpStatusCode.NotFound);

    public static StatusError BadRequest(string message = "") => new(message, HttpStatusCode.BadRequest);

    public static StatusError Conflict(string message = "") => new(message, HttpStatusCode.Conflict);
}