using MediatR;
//using TodoBlazorApp.Application.Common;
using TodoBlazorApp.Application.DTOs;

namespace TodoBlazorApp.Application.Features.Todos.Queries;

public record GetOverdueTodosQuery() : IRequest<Result<IEnumerable<TodoItemDto>>>;