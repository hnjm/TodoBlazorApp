using MediatR;
//using Microsoft.EntityFrameworkCore;
//using TodoBlazorApp.Application.Common;
using TodoBlazorApp.Application.DTOs;
using TodoBlazorApp.Domain.Interfaces;

namespace TodoBlazorApp.Application.Features.Todos.Queries;

public class GetOverdueTodosQueryHandler : IRequestHandler<GetOverdueTodosQuery, Result<IEnumerable<TodoItemDto>>>
{
    private readonly ITodoRepository _todoRepository;

    public GetOverdueTodosQueryHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public async Task<Result<IEnumerable<TodoItemDto>>> Handle(GetOverdueTodosQuery request, CancellationToken cancellationToken)
    {
        var todos = await _todoRepository.GetAllAsync();
        
        var overdueTodos = todos
            .Where(t => !t.IsCompleted && t.DueDate.HasValue && t.DueDate.Value.Date < DateTime.UtcNow.Date)
            .Select(t => new TodoItemDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                IsCompleted = t.IsCompleted,
                CompletedDate = t.CompletedDate,
                Priority = t.Priority,
                Category = t.Category,
                Tags = t.Tags,
                DueDate = t.DueDate,
                AssignedTo = t.AssignedTo,
                EstimatedHours = t.EstimatedHours,
                ActualHours = t.ActualHours,
                CreatedBy = t.CreatedBy,
                CreatedDate = t.CreatedDate,
                ModifiedBy = t.ModifiedBy,
                ModifiedDate = t.ModifiedDate,
                IsOverdue = !t.IsCompleted && t.DueDate.HasValue && t.DueDate.Value < DateTime.UtcNow,
                Comments = new List<TodoCommentDto>(),
                Attachments = new List<TodoAttachmentDto>()
            })
            .OrderBy(t => t.DueDate)
            .ToList();

        return Result<IEnumerable<TodoItemDto>>.Success(overdueTodos);
    }
}