using CacheSample.Shared;
using Microsoft.AspNetCore.Mvc;

namespace CacheSample.Api.ControllersConfig;

public abstract class ApiResponseController : ControllerBase
{
    protected JsonResult ApiResult<T>(CustomResult<T> data)
    {
        return new JsonResult(data)
        {
            StatusCode = (int)data.StatusCode
        };
    }
}
