using CacheSample.Core.Entities;
using CacheSample.Core.Repositories;
using CacheSample.Infra.DataAccess.EFCore.Context;
using CacheSample.Shared.Interfaces;

namespace CacheSample.Infra.Repositories;

public class StudentCachedRepository : IStudentRepository
{
    private const string HASHKEY = "hashStudents";

    private readonly StudentRepository _decoratedRepository;
    private readonly ICacheService _cacheService;
    private readonly DataContext _ctx;

    public StudentCachedRepository(StudentRepository decoratedRepository, ICacheService cacheService, DataContext ctx)
    {
        _decoratedRepository = decoratedRepository;
        _cacheService = cacheService;
        _ctx = ctx;
    }

    public async Task<Student> CreateStudentAsync(Student student)
    {
        student = await _decoratedRepository.CreateStudentAsync(student);

        await _cacheService.SetDataAsync(HASHKEY, student.Id, student);

        return student;
    }

    public async Task<Student> UpdateStudentAsync(Student student)
    {
        await _decoratedRepository.UpdateStudentAsync(student);

        await _cacheService.SetDataAsync(HASHKEY, student.Id, student);

        return student;
    }



    public async Task<Student?> GetStudentByIdAsync(int studentId)
    {
        Student? student = await _cacheService.GetDataByIdAsync<Student>(HASHKEY, studentId);

        if (student is null)
            student = await GetStudentByIdNoTrackAsync(studentId);

        if (student is null)
            return null;

        _ctx.Set<Student>().Attach(student);

        return student;
    }


    public async Task<Student?> GetStudentByIdNoTrackAsync(int studentId)
    {
        Student? student = await _cacheService.GetDataByIdAsync<Student>(HASHKEY, studentId);

        if (student is null)
        {
            student = await _decoratedRepository.GetStudentByIdNoTrackAsync(studentId);

            if (student is null)
                return null;

            await _cacheService.SetDataAsync(HASHKEY, student.Id, student);
        }

        return student;
    }

    public async Task<IEnumerable<Student>> GetStudentsAsync()
    {
        var cachedStudents = await _cacheService.GetAllDataAsync<Student>(HASHKEY);

        if (cachedStudents is not null)
            return cachedStudents;

        var students = await _decoratedRepository.GetStudentsAsync();

        return students;
    }

    public async Task<IEnumerable<Student>> GetStudentsPaginatedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var cachedStudents = await _cacheService.GetAllDataAsync<Student>(HASHKEY);

        var students = Enumerable.Empty<Student>();

        if (cachedStudents is not null)
            return cachedStudents;

        students = await _decoratedRepository.GetStudentsPaginatedAsync(pageNumber, pageSize, cancellationToken);

        //if (students.Any())
        //await _cacheService.SetDataAsync<IEnumerable<Student>>(key, students, TimeSpan.FromMinutes(30));

        return new List<Student>();
    }
}
