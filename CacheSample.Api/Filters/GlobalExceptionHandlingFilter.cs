using CacheSample.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CacheSample.Api.Filters;

public class GlobalExceptionHandlingFilter : Attribute, IAsyncExceptionFilter
{
    public Task OnExceptionAsync(ExceptionContext context)
    {
        var logger = context.HttpContext.RequestServices.GetService<ILogger<GlobalExceptionHandlingFilter>>();

        if (logger is not null)
            logger.LogError($"Exception - {context.Exception.StackTrace}");

        return Task.Run(() =>
        {
            var result = new CustomResult<object>(System.Net.HttpStatusCode.InternalServerError, false, new List<string>() { context.Exception.Message });

            context.Result = new JsonResult(result)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        });
    }
}