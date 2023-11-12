using CacheSample.Shared;
using CacheSample.Shared.Responses;

namespace CacheSample.Application.Queries;

public class GetStudentByIdQuery : IRequest<CustomResult<StudentResponse>>
{
    public int Id { get; set; }
}
