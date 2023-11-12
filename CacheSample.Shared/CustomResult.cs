using System.Net;

namespace CacheSample.Shared;

public class CustomResult<T>
{
    public HttpStatusCode StatusCode { get; init; }
    public bool Success { get; init; }
    public T Data { get; init; }
    public IEnumerable<string> Errors { get; init; } = Enumerable.Empty<string>();

    public CustomResult()
    {

    }

    public CustomResult(HttpStatusCode statusCode, bool success)
    {
        StatusCode = statusCode;
        Success = success;
    }

    public CustomResult(HttpStatusCode statusCode, bool sucesso, T data) : this(statusCode, sucesso) => Data = data;

    public CustomResult(HttpStatusCode statusCode, bool sucesso, IEnumerable<string> erros) : this(statusCode, sucesso) =>
        Errors = erros;

    public CustomResult(HttpStatusCode statusCode, bool sucesso, T data, IEnumerable<string> erros) : this(statusCode, sucesso, data) =>
        Errors = erros;

    public CustomResult<T> OkResponse(T data) => new CustomResult<T>(HttpStatusCode.OK, true, data);

    public CustomResult<T> CreatedResponse(T data) => new CustomResult<T>(HttpStatusCode.Created, true, data);

    public CustomResult<T> NotFoundResponse() => new CustomResult<T>(HttpStatusCode.NotFound, true);

    public CustomResult<T> BadRequestResponse(IEnumerable<string> erros) => new CustomResult<T>(HttpStatusCode.BadRequest, false, erros);

    public CustomResult<T> BadRequestResponse(string erro) => new CustomResult<T>(HttpStatusCode.BadRequest, false, new List<string> { erro });

    //public CustomResult<T> OkdResponse() => new CustomResult<T>(HttpStatusCode.OK, true);
    //public CustomResult<T> NoContentResponse() => new CustomResult<T>(HttpStatusCode.NoContent, true);
}
