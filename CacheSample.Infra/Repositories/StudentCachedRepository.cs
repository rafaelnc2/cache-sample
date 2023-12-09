using CacheSample.Core.Constants;
using CacheSample.Core.Entities;
using CacheSample.Core.Repositories;
using CacheSample.Shared.Interfaces;
using NRedisStack.Search;

namespace CacheSample.Infra.Repositories;

public class StudentCachedRepository : IStudentRepository
{
    private readonly StudentRepository _decoratedRepository;
    private readonly ICacheService _cacheService;

    public StudentCachedRepository(StudentRepository decoratedRepository, ICacheService cacheService)
    {
        _decoratedRepository = decoratedRepository;
        _cacheService = cacheService;

        var studentSchema = new Schema()
            .AddNumericField(new FieldName("$.Id", "id"))
            .AddTextField(new FieldName("$.Name", "name"))
            .AddTextField(new FieldName("$.Cpf", "cpf"))
            .AddTextField(new FieldName("$.Email", "email"))
            .AddTextField(new FieldName("$.BirthDay", "birthDay"));

        _cacheService.CreateIndex(studentSchema, CacheKeysConstants.STUDENTS_INDEX_NAME, CacheKeysConstants.STUDENTS_PREFIX);
    }

    public async Task<Student> CreateStudentAsync(Student student) =>
        await _decoratedRepository.CreateStudentAsync(student);


    public async Task<Student> UpdateStudentAsync(Student student)
    {
        await _decoratedRepository.UpdateStudentAsync(student);

        return student;
    }



    public async Task<Student?> GetStudentByIdAsync(int studentId)
    {
        Student? student = _cacheService.GetDataById<Student>(studentId);

        if (student is null)
        {
            student = await _decoratedRepository.GetStudentByIdNoTrackAsync(studentId);

            if (student is null)
                return null;

            _cacheService.SetData(student.Id, student);
        }

        return student;
    }

    public async ValueTask<IEnumerable<Student>> GetStudentsAsync()
    {
        var cachedStudents = _cacheService.GetAllData<Student>();

        if (cachedStudents is not null)
            return cachedStudents.OrderBy(item => item.Id);

        var students = await _decoratedRepository.GetStudentsAsync();

        return students;
    }

    public async ValueTask<IEnumerable<Student>> GetStudentsPaginatedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var cachedStudents = _cacheService.GetAllPaginatedData<Student>("id", pageNumber, pageSize);

        if (cachedStudents is not null)
            return cachedStudents;

        return await _decoratedRepository.GetStudentsPaginatedAsync(pageNumber, pageSize, cancellationToken);
    }
}
