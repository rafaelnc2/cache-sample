using CacheSample.Application.Queries.Students;
using CacheSample.Shared;
using CacheSample.Shared.Interfaces;
using CacheSample.Shared.Responses;

namespace CacheSample.Application.Handlers.Students;

public class GetAllStudentsHandler : IRequestHandler<GetAllStudentsQuery, CustomResult<IEnumerable<StudentResponse>>>
{
    private readonly IStudentRepository _repository;
    private readonly ICacheService _cacheService;

    public GetAllStudentsHandler(IStudentRepository repository, ICacheService cacheService)
    {
        _repository = repository;
        _cacheService = cacheService;
    }

    public async Task<CustomResult<IEnumerable<StudentResponse>>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
    {
        var result = new CustomResult<IEnumerable<StudentResponse>>();

        var students = await _cacheService.GetAsync("students",
            async () =>
            {
                var students = await _repository.GetStudentsPaginatedAsync(request.PageNumber, request.PageSize, cancellationToken);
                return students.ToList();
            },
            cancellationToken
        ) ?? Enumerable.Empty<Student>();

        List<StudentResponse> response = students.Select(student => (StudentResponse)student).ToList();

        return result.OkResponse(response);
    }



    //public async Task<CustomResult<IEnumerable<StudentResponse>>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
    //{
    //    var result = new CustomResult<IEnumerable<StudentResponse>>();

    //    var students = await _cacheService.GetAsync<IEnumerable<Student>>("students", cancellationToken);

    //    if (students is null)
    //    {
    //        students = await _repository.GetStudentsPaginatedAsync(request.PageNumber, request.PageSize, cancellationToken);

    //        await _cacheService.SetAsync("students", students, cancellationToken);
    //    }

    //    List<StudentResponse> response = students.Select(student => (StudentResponse)student).ToList();

    //    return result.OkResponse(response);
    //}
}
