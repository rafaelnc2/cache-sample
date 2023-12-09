using CacheSample.Core.Entities;
using CacheSample.Core.Repositories;
using CacheSample.Infra.DataAccess.EFCore.Context;
using Microsoft.EntityFrameworkCore;

namespace CacheSample.Infra.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly DataContext _ctx;

    public StudentRepository(DataContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<Student> CreateStudentAsync(Student student)
    {
        await _ctx.Student.AddAsync(student);

        await _ctx.SaveChangesAsync();

        return student;
    }

    public async Task<Student> UpdateStudentAsync(Student student)
    {
        _ctx.Student.Update(student);

        await _ctx.SaveChangesAsync();

        return student;
    }



    public async Task<Student?> GetStudentByIdAsync(int studentId) =>
        await _ctx.Student.FirstOrDefaultAsync(x => x.Id == studentId);

    public async Task<Student?> GetStudentByIdNoTrackAsync(int studentId) =>
        await _ctx.Student.AsNoTracking().FirstOrDefaultAsync(x => x.Id == studentId);

    public async ValueTask<IEnumerable<Student>> GetStudentsAsync() =>
        await _ctx.Student.AsNoTracking().ToListAsync() ?? Enumerable.Empty<Student>();

    public async ValueTask<IEnumerable<Student>> GetStudentsPaginatedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        if (pageNumber > 0)
            pageNumber = (pageNumber - 1) * pageSize;

        return await _ctx.Student
            .AsNoTracking()
            .Skip(pageNumber).Take(pageSize)
            .ToListAsync(cancellationToken) ?? Enumerable.Empty<Student>();
    }
}
