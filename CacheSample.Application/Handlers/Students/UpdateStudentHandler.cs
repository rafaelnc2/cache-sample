using CacheSample.Application.Commands.Students;
using CacheSample.Shared;
using CacheSample.Shared.Interfaces;
using CacheSample.Shared.Responses;

namespace CacheSample.Application.Handlers.Students;

public class UpdateStudentHandler : IRequestHandler<UpdateStudentCommand, CustomResult<StudentResponse>>
{
    private readonly IStudentRepository _studentRepository;
    private readonly ICacheService _cacheService;

    public UpdateStudentHandler(IStudentRepository studentRepository, ICacheService cacheService)
    {
        _studentRepository = studentRepository;
        _cacheService = cacheService;
    }

    public async Task<CustomResult<StudentResponse>> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
    {
        var result = new CustomResult<StudentResponse>();

        Student? student = await _studentRepository.GetStudentByIdAsync(request.Id);

        if (student is null)
            return result.BadRequestResponse("Student not found");

        student.Update(request.Name, request.Cpf, request.BirthDay, request.Email);

        if (student.IsValid is false)
            return result.BadRequestResponse(student.Errors);

        await _studentRepository.UpdateStudentAsync(student);

        StudentResponse response = student;

        _cacheService.SetData(response.Id, response);

        return result.OkResponse(response);
    }
}
