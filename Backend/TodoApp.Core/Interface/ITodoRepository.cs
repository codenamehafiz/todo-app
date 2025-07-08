using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.Core.Entities;

namespace TodoApp.Core.Interfaces
{
    public interface ITodoRepository
    {
        Task<IEnumerable<TodoItem>> GetAllAsync();
        Task<TodoItem?> GetByIdAsync(int id);
        Task<TodoItem> CreateAsync(TodoItem todoItem);
        Task<TodoItem> UpdateAsync(TodoItem todoItem);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}