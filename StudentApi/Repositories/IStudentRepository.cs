using StudentApi.Models;

namespace StudentApi.Repositories;

public interface IStudentRepository
{
    Task<IEnumerable<Student>> GetAllAsync();
    Task<Student?> GetByIdAsync(int id);
    Task<Student> AddAsync(Student student);
    Task<Student?> UpdateAsync(int id, Student student);
    Task<bool> DeleteAsync(int id);
}
