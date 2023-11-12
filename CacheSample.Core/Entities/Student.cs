namespace CacheSample.Core.Entities;

public class Student : Entity
{
    private List<string> _errors = new();

    public Student(string name, string cpf, string birthDay, string email)
    {
        if (Validate(name, cpf, email) is false)
            return;

        Name = name;
        Cpf = cpf;
        Email = email;
        BirthDay = birthDay;

        UpdateDate = null;
        CreateDate = DateTime.Now;
    }

    public string Name { get; private set; } = "";
    public string Cpf { get; private set; } = "";
    public string Email { get; private set; } = "";
    public string BirthDay { get; private set; } = "";


    public IReadOnlyCollection<string> Errors { get => _errors; }
    public bool IsValid { get => (_errors.Any() is false); }


    private bool Validate(string name, string cpf, string email)
    {
        if (string.IsNullOrEmpty(name) || name.Length < 3)
            _errors.Add("Nome inválido");

        if (string.IsNullOrEmpty(cpf) || cpf.Length < 11)
            _errors.Add("CPF inválido");

        if (string.IsNullOrEmpty(email) || email.Contains("@") is false)
            _errors.Add("Email inválido");

        return _errors.Any() is false;
    }

    public void Update(string name, string cpf, string birthDay, string email)
    {
        if (Validate(name, cpf, email) is false)
            return;

        Name = name;
        Cpf = cpf;
        Email = email;
        BirthDay = birthDay;

        UpdateDate = DateTime.Now;
    }
}
