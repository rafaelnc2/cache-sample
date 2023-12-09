using CacheSample.Application.Queries;
using CacheSample.Shared;
using CacheSample.Shared.Responses;

namespace CacheSample.Application.Handlers.Students;

public class GetStudentByIdHandler : IRequestHandler<GetStudentByIdQuery, CustomResult<StudentResponse>>
{
    private readonly IStudentRepository _repository;

    public GetStudentByIdHandler(IStudentRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomResult<StudentResponse>> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
    {
        var result = new CustomResult<StudentResponse>();

        if (request.Id <= 0)
            return result.BadRequestResponse("Invalid Id");

        var student = await _repository.GetStudentByIdAsync(request.Id);

        if (student is null)
            return result.NotFoundResponse();

        StudentResponse response = student;

        return result.OkResponse(response);
    }
}
