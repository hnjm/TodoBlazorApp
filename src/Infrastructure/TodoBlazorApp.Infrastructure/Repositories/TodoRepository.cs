using Microsoft.EntityFrameworkCore;
using TodoBlazorApp.Domain.Entities;
using TodoBlazorApp.Domain.Enums;
using TodoBlazorApp.Domain.Interfaces;
using TodoBlazorApp.Infrastructure.Data;

namespace TodoBlazorApp.Infrastructure.Repositories;

public class TodoRepository : Repository<TodoItem>, ITodoRepository
{
    public TodoRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<TodoItem>> GetAllWithDetailsAsync()
    {
        return await _dbSet
            .Include(t => t.Comments)
            .Include(t => t.Attachments)
            .OrderByDescending(t => t.Priority)
            .ThenByDescending(t => t.CreatedDate)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<TodoItem?> GetByIdWithDetailsAsync(int id)
    {
        return await _dbSet
            .Include(t => t.Comments.OrderByDescending(c => c.CreatedDate))
            .Include(t => t.Attachments)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IReadOnlyList<TodoItem>> GetByCompletionStatusAsync(bool isCompleted)
    {
        return await _dbSet
            .Where(t => t.IsCompleted == isCompleted)
            .OrderByDescending(t => t.Priority)
            .ThenByDescending(t => t.CreatedDate)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IReadOnlyList<TodoItem>> GetByCategoryAsync(string category)
    {
        return await _dbSet
            .Where(t => t.Category == category)
            .OrderByDescending(t => t.Priority)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IReadOnlyList<TodoItem>> GetByPriorityAsync(Priority priority)
    {
        return await _dbSet
            .Where(t => t.Priority == priority)
            .OrderByDescending(t => t.CreatedDate)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IReadOnlyList<TodoItem>> GetOverdueAsync()
    {
        var now = DateTime.UtcNow;
        return await _dbSet
            .Where(t => !t.IsCompleted && t.DueDate.HasValue && t.DueDate.Value < now)
            .OrderBy(t => t.DueDate)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IReadOnlyList<TodoItem>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Where(t => t.CreatedDate >= startDate && t.CreatedDate <= endDate)
            .OrderByDescending(t => t.CreatedDate)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<int> GetCompletedCountAsync()
    {
        return await _dbSet.CountAsync(t => t.IsCompleted);
    }

    public async Task<int> GetPendingCountAsync()
    {
        return await _dbSet.CountAsync(t => !t.IsCompleted);
    }
}