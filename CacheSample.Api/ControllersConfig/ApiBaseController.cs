using MediatR;

namespace CacheSample.Api.ControllersConfig;

public abstract class ApiBaseController<T> : ApiResponseController where T : ApiBaseController<T>
{
    private ILogger<T>? _logger;
    private IMediator? _mediator;

    protected ILogger<T> Logger => _logger ??= HttpContext.RequestServices.GetRequiredService<ILogger<T>>();
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
}
