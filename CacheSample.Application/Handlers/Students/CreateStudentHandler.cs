using CacheSample.Application.Commands.Students;
using CacheSample.Shared;
using CacheSample.Shared.Interfaces;
using CacheSample.Shared.Responses;

namespace CacheSample.Application.Handlers.Students;

public class CreateStudentHandler : IRequestHandler<CreateStudentCommand, CustomResult<StudentResponse>>
{
    private readonly IStudentRepository _studentRepository;
    private readonly ICacheService _cacheService;


    public CreateStudentHandler(IStudentRepository studentRepository, ICacheService cacheService)
    {
        _studentRepository = studentRepository;
        _cacheService = cacheService;
    }

    public async Task<CustomResult<StudentResponse>> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        var result = new CustomResult<StudentResponse>();

        var student = new Student(request.Name, request.Cpf, request.BirthDay, request.Email);

        if (student.IsValid is false)
            return result.BadRequestResponse(student.Errors);

        await _studentRepository.CreateStudentAsync(student);

        StudentResponse response = student;

        _cacheService.SetData(response.Id, response);

        return result.CreatedResponse(response);
    }
}
