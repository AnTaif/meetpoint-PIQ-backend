using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Core.Results;

public static class MvcResultExtensions
{
    public static ActionResult ToActionResult<T>(
        this Result<T> result, ControllerBase thisController, Func<T, ActionResult>? onValue = null)
    {
        if (result.IsFailure)
        {
            return result.Error is StatusError httpError
                ? HandleStatusError(httpError, thisController)
                : HandleAsBadRequest(result.Error, thisController);
        }

        return onValue?.Invoke(result.Value) ?? thisController.Ok(result.Value);
    }

    public static ActionResult ToActionResult(this Result result, ControllerBase thisController)
    {
        if (result.IsFailure)
        {
            return result.Error is StatusError httpError
                ? HandleStatusError(httpError, thisController)
                : HandleAsBadRequest(result.Error, thisController);
        }

        return thisController.NoContent();
    }

    private static ActionResult HandleStatusError(StatusError error, ControllerBase thisController)
    {
        return thisController.StatusCode((int)error.HttpStatus, error.Message);
    }

    private static ActionResult HandleAsBadRequest(Error error, ControllerBase thisController)
    {
        return thisController.StatusCode((int)HttpStatusCode.BadRequest, error.Message);
    }
}