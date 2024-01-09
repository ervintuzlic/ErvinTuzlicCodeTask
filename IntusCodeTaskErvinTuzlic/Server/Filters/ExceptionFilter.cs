using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using IntusCodeTaskErvinTuzlic.Shared.DTO;

namespace IntusCodeTaskErvinTuzlic.Server.Filters;

public class ExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ExceptionFilter> _logger;

    public ExceptionFilter(ILogger<ExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case ArgumentException aex:
                {
                    context.Result = new BadRequestObjectResult(new InvalidRequestResponse { Message = aex.Message });
                    _logger.LogWarning(aex, $"[{context.RouteData.Values["controller"]}:{context.RouteData.Values["action"]}]");
                    break;
                }
            default:
                {
                    context.Result = new ObjectResult(new InvalidRequestResponse { Message = "Internal Server Error." }) { StatusCode = (int)HttpStatusCode.InternalServerError };
                    _logger.LogError(context.Exception, $"[{context.RouteData.Values["controller"]}:{context.RouteData.Values["action"]}] - Internal Server Error");
                    break;
                }
        }

        context.ExceptionHandled = true;
    }
}

