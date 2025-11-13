namespace TodoBlazorApp.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ITodoRepository TodoItems { get; }
    IRepository<TodoComment> TodoComments { get; }
    IRepository<TodoAttachment> TodoAttachments { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}