using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListApi.Data;
using TodoListApi.Models;
using TodoListApi.Models.Dto;

namespace TodoListApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    private readonly AppDbContext _context;

    public TodosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodos(
        [FromQuery] bool? completed = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var query = _context.Todos.AsQueryable();

        if (completed.HasValue)
            query = query.Where(t => t.IsCompleted == completed.Value);

        var todos = await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(todos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> GetTodo(int id)
    {
        var todo = await _context.Todos.FindAsync(id);
        if (todo == null) return NotFound();
        return Ok(todo);
    }

    [HttpPost]
    public async Task<ActionResult<TodoItem>> CreateTodo(CreateTodoDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
            return BadRequest("Название задачи обязательно!");

        var todo = new TodoItem { Title = dto.Title.Trim() };
        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateTodo(int id, UpdateTodoDto dto)
    {
        var todo = await _context.Todos.FindAsync(id);
        if (todo == null) return NotFound();

        if (!string.IsNullOrWhiteSpace(dto.Title))
            todo.Title = dto.Title.Trim();

        if (dto.IsCompleted.HasValue)
        {
            todo.IsCompleted = dto.IsCompleted.Value;
            todo.CompletedAt = todo.IsCompleted ? DateTime.UtcNow : null;
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodo(int id)
    {
        var todo = await _context.Todos.FindAsync(id);
        if (todo == null) return NotFound();

        _context.Todos.Remove(todo);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}