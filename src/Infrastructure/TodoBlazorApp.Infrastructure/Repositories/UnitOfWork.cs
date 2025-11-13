using TodoBlazorApp.Domain.Entities;
using TodoBlazorApp.Domain.Interfaces;
using TodoBlazorApp.Infrastructure.Data;

namespace TodoBlazorApp.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private ITodoRepository? _todoItems;
    private IRepository<TodoComment>? _todoComments;
    private IRepository<TodoAttachment>? _todoAttachments;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public ITodoRepository TodoItems =>
        _todoItems ??= new TodoRepository(_context);

    public IRepository<TodoComment> TodoComments =>
        _todoComments ??= new Repository<TodoComment>(_context);

    public IRepository<TodoAttachment> TodoAttachments =>
        _todoAttachments ??= new Repository<TodoAttachment>(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}