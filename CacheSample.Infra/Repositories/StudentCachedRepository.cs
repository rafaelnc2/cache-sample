using CacheSample.Core.Entities;
using CacheSample.Core.Repositories;
using CacheSample.Infra.DataAccess.EFCore.Context;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace CacheSample.Infra.Repositories;

public class StudentCachedRepository : IStudentRepository
{
    private readonly StudentRepository _decoratedRepository;
    private readonly IDistributedCache _distributedCache;
    private readonly DataContext _ctx;

    public StudentCachedRepository(StudentRepository decoratedRepository, IDistributedCache distributedCache, DataContext ctx)
    {
        _decoratedRepository = decoratedRepository;
        _distributedCache = distributedCache;
        _ctx = ctx;
    }

    public async Task<Student> CreateStudentAsync(Student student) =>
        await _decoratedRepository.CreateStudentAsync(student);

    public async Task<Student> UpdateStudentAsync(Student student) =>
        await _decoratedRepository.UpdateStudentAsync(student);



    public async Task<Student?> GetStudentByIdAsync(int studentId)
    {
        var key = $"student-{studentId}";

        await _distributedCache.RemoveAsync(key);

        Student? student = await GetStudentByIdNoTrackAsync(studentId);

        if (student is null)
            return student;

        _ctx.Set<Student>().Attach(student);

        return student;
    }


    public async Task<Student?> GetStudentByIdNoTrackAsync(int studentId)
    {
        var key = $"student-{studentId}";

        var cachedStudent = await _distributedCache.GetStringAsync(key);

        if (string.IsNullOrEmpty(cachedStudent) is false)
            return JsonSerializer.Deserialize<Student>(cachedStudent);

        Student? student = await _decoratedRepository.GetStudentByIdNoTrackAsync(studentId);

        if (student is null)
            return student;

        await _distributedCache.SetStringAsync(key, JsonSerializer.Serialize(student));

        return student;
    }

    public async Task<IEnumerable<Student>> GetStudentsAsync()
    {
        var key = "students";

        var cachedStudents = await _distributedCache.GetStringAsync(key);

        var students = Enumerable.Empty<Student>();

        if (string.IsNullOrEmpty(cachedStudents) is false)
            return JsonSerializer.Deserialize<IEnumerable<Student>>(cachedStudents) ?? students;

        students = await _decoratedRepository.GetStudentsAsync();

        if (students.Any())
            await _distributedCache.SetStringAsync(key, JsonSerializer.Serialize(students));

        return students;
    }

    public async Task<IEnumerable<Student>> GetStudentsPaginatedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var key = $"students-page-{pageNumber}-size-{pageSize}";

        var cachedStudents = await _distributedCache.GetStringAsync(key);

        var students = Enumerable.Empty<Student>();

        if (string.IsNullOrEmpty(cachedStudents) is false)
            return JsonSerializer.Deserialize<IEnumerable<Student>>(cachedStudents) ?? students;

        students = await _decoratedRepository.GetStudentsPaginatedAsync(pageNumber, pageSize, cancellationToken);

        if (students.Any())
            await _distributedCache.SetStringAsync(key, JsonSerializer.Serialize(students));

        return students;
    }
}
