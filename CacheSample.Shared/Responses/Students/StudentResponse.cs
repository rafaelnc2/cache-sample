using CacheSample.Core.Entities;

namespace CacheSample.Shared.Responses;

public class StudentResponse
{
    public int Id { get; private set; }
    public string Name { get; private set; } = "";
    public string Cpf { get; private set; } = "";
    public string Email { get; private set; } = "";
    public string BirthDay { get; private set; } = "";


    public static implicit operator StudentResponse(Student student) =>
        new StudentResponse()
        {
            Id = student.Id,
            Name = student.Name,
            Cpf = student.Cpf,
            Email = student.Email,
            BirthDay = student.BirthDay
        };
}
