using CacheSample.Shared;
using CacheSample.Shared.Responses;

namespace CacheSample.Application.Queries.Students;

public class GetAllStudentsQuery : IRequest<CustomResult<IEnumerable<StudentResponse>>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
