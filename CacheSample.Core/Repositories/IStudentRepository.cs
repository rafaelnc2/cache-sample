﻿using CacheSample.Core.Entities;

namespace CacheSample.Core.Repositories;

public interface IStudentRepository
{
    public Task<Student> CreateStudentAsync(Student student);
    public Task<Student> UpdateStudentAsync(Student student);



    Task<Student?> GetStudentByIdAsync(int studentId);
    ValueTask<IEnumerable<Student>> GetStudentsAsync();
    ValueTask<IEnumerable<Student>> GetStudentsPaginatedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
}
