using TodoBlazorApp.Domain.Entities;

namespace TodoBlazorApp.Domain.Interfaces;

public interface ITodoRepository : IRepository<TodoItem>
{
    Task<IReadOnlyList<TodoItem>> GetAllWithDetailsAsync();
    Task<TodoItem?> GetByIdWithDetailsAsync(int id);
    Task<IReadOnlyList<TodoItem>> GetByCompletionStatusAsync(bool isCompleted);
    Task<IReadOnlyList<TodoItem>> GetByCategoryAsync(string category);
    Task<IReadOnlyList<TodoItem>> GetByPriorityAsync(Priority priority);
    Task<IReadOnlyList<TodoItem>> GetOverdueAsync();
    Task<IReadOnlyList<TodoItem>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<int> GetCompletedCountAsync();
    Task<int> GetPendingCountAsync();
}