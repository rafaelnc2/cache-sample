using CacheSample.Shared;
using CacheSample.Shared.Responses;

namespace CacheSample.Application.Commands.Students;

public class UpdateStudentCommand : IRequest<CustomResult<StudentResponse>>
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Cpf { get; set; } = "";
    public string Email { get; set; } = "";
    public string BirthDay { get; set; } = "";
}
