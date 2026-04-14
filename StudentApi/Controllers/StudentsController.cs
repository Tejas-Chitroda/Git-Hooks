using Microsoft.AspNetCore.Mvc;
using StudentApi.Models;
using StudentApi.Repositories;

namespace StudentApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentRepository _repository;
    private readonly ILogger<StudentsController> _logger;

    public StudentsController(IStudentRepository repository, ILogger<StudentsController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    // GET: api/students
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Student>>> GetAllStudents()
    {
        _logger.LogInformation("Getting all students");
        Console.WriteLine("Getting all students");
        var students = await _repository.GetAllAsync();
        return Ok(students);
    }

    // GET: api/students/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Student>> GetStudent(int id)
    {
        _logger.LogInformation("Getting student with id: {Id}", id);
        var student = await _repository.GetByIdAsync(id);

        if (student == null)
        {
            _logger.LogWarning("Student with id {Id} not found", id);
            return NotFound(new { message = $"Student with id {id} not found" });
        }

        return Ok(student);
    }

    // POST: api/students
    [HttpPost]
    public async Task<ActionResult<Student>> CreateStudent([FromBody] Student student)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _logger.LogInformation("Creating new student");
        var createdStudent = await _repository.AddAsync(student);
        return CreatedAtAction(nameof(GetStudent), new { id = createdStudent.Id }, createdStudent);
    }

    // PUT: api/students/5
    [HttpPut("{id}")]
    public async Task<ActionResult<Student>> UpdateStudent(int id, [FromBody] Student student)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _logger.LogInformation("Updating student with id: {Id}", id);
        var updatedStudent = await _repository.UpdateAsync(id, student);

        if (updatedStudent == null)
        {
            _logger.LogWarning("Student with id {Id} not found for update", id);
            return NotFound(new { message = $"Student with id {id} not found" });
        }

        return Ok(updatedStudent);
    }

    // DELETE: api/students/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteStudent(int id)
    {
        _logger.LogInformation("Deleting student with id: {Id}", id);
        var result = await _repository.DeleteAsync(id);

        if (!result)
        {
            _logger.LogWarning("Student with id {Id} not found for deletion", id);
            return NotFound(new { message = $"Student with id {id} not found" });
        }

        return NoContent();
    }
}
