using MediatR;
using TodoBlazorApp.Application.DTOs;
using TodoBlazorApp.Domain.Enums;
using TodoBlazorApp.Domain.Interfaces;

namespace TodoBlazorApp.Application.Features.Todos.Queries;

public record GetTodoStatsQuery : IRequest<Result<TodoStatsDto>>;

public class GetTodoStatsQueryHandler : IRequestHandler<GetTodoStatsQuery, Result<TodoStatsDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTodoStatsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<TodoStatsDto>> Handle(GetTodoStatsQuery request, CancellationToken cancellationToken)
    {
        var allTodos = await _unitOfWork.TodoItems.GetAllAsync();
        var completed = await _unitOfWork.TodoItems.GetCompletedCountAsync();
        var pending = await _unitOfWork.TodoItems.GetPendingCountAsync();
        var overdue = (await _unitOfWork.TodoItems.GetOverdueAsync()).Count;
        var total = completed + pending;

        var byPriority = allTodos
            .GroupBy(t => t.Priority)
            .ToDictionary(g => g.Key, g => g.Count());

        var byCategory = allTodos
            .Where(t => !string.IsNullOrEmpty(t.Category))
            .GroupBy(t => t.Category!)
            .ToDictionary(g => g.Key, g => g.Count());

        var stats = new TodoStatsDto
        {
            TotalCount = total,
            CompletedCount = completed,
            PendingCount = pending,
            OverdueCount = overdue,
            CompletionPercentage = total > 0 ? Math.Round((double)completed / total * 100, 2) : 0,
            ByPriority = byPriority,
            ByCategory = byCategory
        };

        return Result<TodoStatsDto>.Success(stats);
    }
}