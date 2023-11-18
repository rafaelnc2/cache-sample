using CacheSample.Core.Entities;
using CacheSample.Core.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace CacheSample.Infra.Repositories;

public class StudentCacheRepository : IStudentRepository
{
    private readonly StudentRepository _decoratedRepository;
    private readonly IMemoryCache _memoryCache;

    public StudentCacheRepository(StudentRepository decoratedRepository, IMemoryCache memoryCache)
    {
        _decoratedRepository = decoratedRepository;
        _memoryCache = memoryCache;
    }

    public async Task<Student> CreateStudentAsync(Student student) =>
        await _decoratedRepository.CreateStudentAsync(student);

    public async Task<Student> UpdateStudentAsync(Student student) =>
        await _decoratedRepository.UpdateStudentAsync(student);



    public async Task<Student?> GetStudentByIdAsync(int studentId) =>
        await _decoratedRepository.GetStudentByIdNoTrackAsync(studentId);

    public Task<Student?> GetStudentByIdNoTrackAsync(int studentId)
    {
        var key = $"student-{studentId}";

        return _memoryCache.GetOrCreateAsync(
            key,
            entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));
                return _decoratedRepository.GetStudentByIdAsync(studentId);
            }
        );
    }

    public async Task<IEnumerable<Student>> GetStudentsAsync()
    {
        var key = "students";

        var students = await _memoryCache.GetOrCreateAsync(
            key,
            async entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));
                return await _decoratedRepository.GetStudentsAsync();
            }
        );

        return students ?? Enumerable.Empty<Student>();
    }

    public async Task<IEnumerable<Student>> GetStudentsPaginatedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken) =>
        await _decoratedRepository.GetStudentsPaginatedAsync(pageNumber, pageSize, cancellationToken);
}
