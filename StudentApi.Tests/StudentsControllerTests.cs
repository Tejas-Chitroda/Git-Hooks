using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using StudentApi.Controllers;
using StudentApi.Models;
using StudentApi.Repositories;

namespace StudentApi.Tests;

public class StudentsControllerTests
{
    private readonly Mock<IStudentRepository> _mockRepository;
    private readonly Mock<ILogger<StudentsController>> _mockLogger;
    private readonly StudentsController _controller;

    public StudentsControllerTests()
    {
        _mockRepository = new Mock<IStudentRepository>();
        _mockLogger = new Mock<ILogger<StudentsController>>();
        _controller = new StudentsController(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllStudents_ShouldReturnOkWithStudents()
    {
        // Arrange
        var students = new List<Student>
        {
            new Student { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com", Course = "CS" },
            new Student { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane@example.com", Course = "Engineering" }
        };
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(students);

        // Act
        var result = await _controller.GetAllStudents();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedStudents = Assert.IsAssignableFrom<IEnumerable<Student>>(okResult.Value);
        Assert.Equal(2, returnedStudents.Count());
    }

    [Fact]
    public async Task GetStudent_WithValidId_ShouldReturnOkWithStudent()
    {
        // Arrange
        var student = new Student { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com", Course = "CS" };
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(student);

        // Act
        var result = await _controller.GetStudent(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedStudent = Assert.IsType<Student>(okResult.Value);
        Assert.Equal(1, returnedStudent.Id);
        Assert.Equal("John", returnedStudent.FirstName);
    }

    [Fact]
    public async Task GetStudent_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Student?)null);

        // Act
        var result = await _controller.GetStudent(999);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task CreateStudent_WithValidStudent_ShouldReturnCreatedAtAction()
    {
        // Arrange
        var newStudent = new Student
        {
            FirstName = "Alice",
            LastName = "Johnson",
            Email = "alice@example.com",
            DateOfBirth = new DateTime(2002, 3, 10),
            Course = "Mathematics"
        };
        var createdStudent = new Student
        {
            Id = 3,
            FirstName = "Alice",
            LastName = "Johnson",
            Email = "alice@example.com",
            DateOfBirth = new DateTime(2002, 3, 10),
            Course = "Mathematics"
        };
        _mockRepository.Setup(r => r.AddAsync(newStudent)).ReturnsAsync(createdStudent);

        // Act
        var result = await _controller.CreateStudent(newStudent);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnedStudent = Assert.IsType<Student>(createdAtActionResult.Value);
        Assert.Equal(3, returnedStudent.Id);
        Assert.Equal("Alice", returnedStudent.FirstName);
    }

    [Fact]
    public async Task UpdateStudent_WithValidId_ShouldReturnOkWithUpdatedStudent()
    {
        // Arrange
        var updatedStudent = new Student
        {
            FirstName = "John Updated",
            LastName = "Doe Updated",
            Email = "john.updated@example.com",
            DateOfBirth = new DateTime(2000, 1, 15),
            Course = "Computer Engineering"
        };
        var returnedStudent = new Student
        {
            Id = 1,
            FirstName = "John Updated",
            LastName = "Doe Updated",
            Email = "john.updated@example.com",
            DateOfBirth = new DateTime(2000, 1, 15),
            Course = "Computer Engineering"
        };
        _mockRepository.Setup(r => r.UpdateAsync(1, updatedStudent)).ReturnsAsync(returnedStudent);

        // Act
        var result = await _controller.UpdateStudent(1, updatedStudent);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var student = Assert.IsType<Student>(okResult.Value);
        Assert.Equal("John Updated", student.FirstName);
    }

    [Fact]
    public async Task UpdateStudent_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var updatedStudent = new Student
        {
            FirstName = "Test",
            LastName = "User",
            Email = "test@example.com",
            DateOfBirth = DateTime.Now,
            Course = "Test"
        };
        _mockRepository.Setup(r => r.UpdateAsync(999, updatedStudent)).ReturnsAsync((Student?)null);

        // Act
        var result = await _controller.UpdateStudent(999, updatedStudent);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task DeleteStudent_WithValidId_ShouldReturnNoContent()
    {
        // Arrange
        _mockRepository.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteStudent(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteStudent_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        _mockRepository.Setup(r => r.DeleteAsync(999)).ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteStudent(999);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}
