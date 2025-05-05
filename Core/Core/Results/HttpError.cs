using System.Net;

namespace Core.Results;

public class HttpError : Error
{
    public HttpStatusCode HttpStatus { get; }

    protected HttpError(string message, HttpStatusCode httpStatus) : base(message)
    {
        HttpStatus = httpStatus;
    }

    public static HttpError NotFound(string message = "") => new(message, HttpStatusCode.NotFound);

    public static HttpError BadRequest(string message = "") => new(message, HttpStatusCode.BadRequest);

    public static HttpError Conflict(string message = "") => new(message, HttpStatusCode.Conflict);
}