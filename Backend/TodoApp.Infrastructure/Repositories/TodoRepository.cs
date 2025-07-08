using Microsoft.EntityFrameworkCore;
using TodoApp.Core.Entities;
using TodoApp.Core.Interfaces;
using TodoApp.Infrastructure.Data;

namespace TodoApp.Infrastructure.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoDbContext _context;

        public TodoRepository(TodoDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TodoItem>> GetAllAsync()
        {
            return await _context.TodoItems
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<TodoItem?> GetByIdAsync(int id)
        {
            return await _context.TodoItems
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<TodoItem> CreateAsync(TodoItem todoItem)
        {
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();
            return todoItem;
        }

        public async Task<TodoItem> UpdateAsync(TodoItem todoItem)
        {
            _context.TodoItems.Update(todoItem);
            await _context.SaveChangesAsync();
            return todoItem;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
                return false;

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.TodoItems.AnyAsync(t => t.Id == id);
        }
    }
}