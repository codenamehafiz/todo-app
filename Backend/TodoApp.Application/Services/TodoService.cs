using TodoApp.Application.DTOs;
using TodoApp.Application.Interfaces;
using TodoApp.Core.Common;
using TodoApp.Core.Entities;
using TodoApp.Core.Interfaces;

namespace TodoApp.Application.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _todoRepository;

        public TodoService(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<Result<IEnumerable<TodoItemDto>>> GetAllAsync()
        {
            var todos = await _todoRepository.GetAllAsync();
            var todoDtos = todos.Select(MapToDto);
            return Result<IEnumerable<TodoItemDto>>.Success(todoDtos);
        }

        public async Task<Result<TodoItemDto>> GetByIdAsync(int id)
        {
            var todo = await _todoRepository.GetByIdAsync(id);
            if (todo == null)
                return Result<TodoItemDto>.Failure("Todo item not found");

            return Result<TodoItemDto>.Success(MapToDto(todo));
        }

        public async Task<Result<TodoItemDto>> CreateAsync(CreateTodoItemDto createDto)
        {
            if (string.IsNullOrWhiteSpace(createDto.Title))
                return Result<TodoItemDto>.Failure("Title is required");

            var todoItem = new TodoItem
            {
                Title = createDto.Title.Trim(),
                Description = createDto.Description?.Trim(),
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow
            };

            var createdTodo = await _todoRepository.CreateAsync(todoItem);
            return Result<TodoItemDto>.Success(MapToDto(createdTodo));
        }

        public async Task<Result<TodoItemDto>> UpdateAsync(int id, UpdateTodoItemDto updateDto)
        {
            if (string.IsNullOrWhiteSpace(updateDto.Title))
                return Result<TodoItemDto>.Failure("Title is required");

            var existingTodo = await _todoRepository.GetByIdAsync(id);
            if (existingTodo == null)
                return Result<TodoItemDto>.Failure("Todo item not found");

            existingTodo.Title = updateDto.Title.Trim();
            existingTodo.Description = updateDto.Description?.Trim();
            existingTodo.IsCompleted = updateDto.IsCompleted;
            existingTodo.UpdatedAt = DateTime.UtcNow;

            var updatedTodo = await _todoRepository.UpdateAsync(existingTodo);
            return Result<TodoItemDto>.Success(MapToDto(updatedTodo));
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            var exists = await _todoRepository.ExistsAsync(id);
            if (!exists)
                return Result<bool>.Failure("Todo item not found");

            var deleted = await _todoRepository.DeleteAsync(id);
            return Result<bool>.Success(deleted);
        }

        private static TodoItemDto MapToDto(TodoItem todoItem)
        {
            return new TodoItemDto
            {
                Id = todoItem.Id,
                Title = todoItem.Title,
                Description = todoItem.Description,
                IsCompleted = todoItem.IsCompleted,
                CreatedAt = todoItem.CreatedAt,
                UpdatedAt = todoItem.UpdatedAt
            };
        }
    }
}