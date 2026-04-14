using StudentApi.Models;
using StudentApi.Repositories;

namespace StudentApi.Tests;

public class InMemoryStudentRepositoryTests
{
    private InMemoryStudentRepository CreateRepository()
    {
        return new InMemoryStudentRepository();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllStudents()
    {
        // Arrange
        var repository = CreateRepository();

        // Act
        var students = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(students);
        Assert.Equal(2, students.Count()); // Default seed data
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldReturnStudent()
    {
        // Arrange
        var repository = CreateRepository();

        // Act
        var student = await repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(student);
        Assert.Equal(1, student.Id);
        Assert.Equal("John", student.FirstName);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        var repository = CreateRepository();

        // Act
        var student = await repository.GetByIdAsync(999);

        // Assert
        Assert.Null(student);
    }

    [Fact]
    public async Task AddAsync_ShouldAddStudentAndAssignId()
    {
        // Arrange
        var repository = CreateRepository();
        var newStudent = new Student
        {
            FirstName = "Alice",
            LastName = "Johnson",
            Email = "alice.johnson@example.com",
            DateOfBirth = new DateTime(2002, 3, 10),
            Course = "Mathematics"
        };

        // Act
        var addedStudent = await repository.AddAsync(newStudent);

        // Assert
        Assert.NotNull(addedStudent);
        Assert.True(addedStudent.Id > 0);
        Assert.Equal("Alice", addedStudent.FirstName);

        var allStudents = await repository.GetAllAsync();
        Assert.Equal(3, allStudents.Count());
    }

    [Fact]
    public async Task UpdateAsync_WithValidId_ShouldUpdateStudent()
    {
        // Arrange
        var repository = CreateRepository();
        var updatedData = new Student
        {
            FirstName = "John Updated",
            LastName = "Doe Updated",
            Email = "john.updated@example.com",
            DateOfBirth = new DateTime(2000, 1, 15),
            Course = "Computer Engineering"
        };

        // Act
        var result = await repository.UpdateAsync(1, updatedData);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("John Updated", result.FirstName);
        Assert.Equal("Doe Updated", result.LastName);
        Assert.Equal("john.updated@example.com", result.Email);
    }

    [Fact]
    public async Task UpdateAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        var repository = CreateRepository();
        var updatedData = new Student
        {
            FirstName = "Test",
            LastName = "User",
            Email = "test@example.com",
            DateOfBirth = DateTime.Now,
            Course = "Test"
        };

        // Act
        var result = await repository.UpdateAsync(999, updatedData);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_ShouldReturnTrueAndRemoveStudent()
    {
        // Arrange
        var repository = CreateRepository();

        // Act
        var result = await repository.DeleteAsync(1);

        // Assert
        Assert.True(result);
        
        var student = await repository.GetByIdAsync(1);
        Assert.Null(student);
        
        var allStudents = await repository.GetAllAsync();
        Assert.Single(allStudents);
    }

    [Fact]
    public async Task DeleteAsync_WithInvalidId_ShouldReturnFalse()
    {
        // Arrange
        var repository = CreateRepository();

        // Act
        var result = await repository.DeleteAsync(999);

        // Assert
        Assert.False(result);
    }
}