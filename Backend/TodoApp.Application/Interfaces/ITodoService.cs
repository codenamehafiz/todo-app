using TodoApp.Application.DTOs;
using TodoApp.Core.Common;

namespace TodoApp.Application.Interfaces
{
    public interface ITodoService
    {
        Task<Result<IEnumerable<TodoItemDto>>> GetAllAsync();
        Task<Result<TodoItemDto>> GetByIdAsync(int id);
        Task<Result<TodoItemDto>> CreateAsync(CreateTodoItemDto createDto);
        Task<Result<TodoItemDto>> UpdateAsync(int id, UpdateTodoItemDto updateDto);
        Task<Result<bool>> DeleteAsync(int id);
    }
}