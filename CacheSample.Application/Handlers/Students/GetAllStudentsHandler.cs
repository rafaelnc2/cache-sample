using CacheSample.Application.Queries.Students;
using CacheSample.Shared;
using CacheSample.Shared.Responses;

namespace CacheSample.Application.Handlers.Students;

public class GetAllStudentsHandler : IRequestHandler<GetAllStudentsQuery, CustomResult<IEnumerable<StudentResponse>>>
{
    private readonly IStudentRepository _repository;

    public GetAllStudentsHandler(IStudentRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomResult<IEnumerable<StudentResponse>>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
    {
        var result = new CustomResult<IEnumerable<StudentResponse>>();

        var students = await _repository.GetStudentsPaginatedAsync(request.PageNumber, request.PageSize, cancellationToken);

        if (students is null)
            return result.NotFoundResponse();

        List<StudentResponse> response = students.Select(student => (StudentResponse)student).ToList();

        return result.OkResponse(response);
    }
}
