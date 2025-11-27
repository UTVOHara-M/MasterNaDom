namespace TodoListApi.Models.Dto;

public record CreateTodoDto(string Title);

public record UpdateTodoDto(string? Title = null, bool? IsCompleted = null);