using StudentApi.Models;

namespace StudentApi.Repositories;

public class InMemoryStudentRepository : IStudentRepository
{
    private readonly List<Student> _students = new();
    private int _nextId = 1;
    private readonly object _lock = new();

    public InMemoryStudentRepository()
    {
        // Seed with some initial data
        _students.AddRange(new[]
        {
            new Student
            {
                Id = _nextId++,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                DateOfBirth = new DateTime(2000, 1, 15),
                Course = "Computer Science"
            },
            new Student
            {
                Id = _nextId++,
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                DateOfBirth = new DateTime(2001, 5, 22),
                Course = "Engineering"
            }
        });
    }

    public Task<IEnumerable<Student>> GetAllAsync()
    {
        lock (_lock)
        {
            return Task.FromResult<IEnumerable<Student>>(_students.ToList());
        }
    }

    public Task<Student?> GetByIdAsync(int id)
    {
        lock (_lock)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            return Task.FromResult(student);
        }
    }

    public Task<Student> AddAsync(Student student)
    {
        lock (_lock)
        {
            student.Id = _nextId++;
            _students.Add(student);
            return Task.FromResult(student);
        }
    }

    public Task<Student?> UpdateAsync(int id, Student student)
    {
        lock (_lock)
        {
            var existingStudent = _students.FirstOrDefault(s => s.Id == id);
            if (existingStudent == null)
            {
                return Task.FromResult<Student?>(null);
            }

            existingStudent.FirstName = student.FirstName;
            existingStudent.LastName = student.LastName;
            existingStudent.Email = student.Email;
            existingStudent.DateOfBirth = student.DateOfBirth;
            existingStudent.Course = student.Course;

            return Task.FromResult<Student?>(existingStudent);
        }
    }

    public Task<bool> DeleteAsync(int id)
    {
        lock (_lock)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return Task.FromResult(false);
            }

            _students.Remove(student);
            return Task.FromResult(true);
        }
    }
}
