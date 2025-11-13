using AutoMapper;
using MediatR;
using TodoBlazorApp.Application.DTOs;
using TodoBlazorApp.Domain.Interfaces;

namespace TodoBlazorApp.Application.Features.Todos.Queries;

public record GetAllTodosQuery(bool IncludeDetails = false) : IRequest<Result<IEnumerable<TodoItemDto>>>;

public class GetAllTodosQueryHandler : IRequestHandler<GetAllTodosQuery, Result<IEnumerable<TodoItemDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllTodosQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<TodoItemDto>>> Handle(GetAllTodosQuery request, CancellationToken cancellationToken)
    {
        var todos = request.IncludeDetails
            ? await _unitOfWork.TodoItems.GetAllWithDetailsAsync()
            : await _unitOfWork.TodoItems.GetAllAsync();

        var dtos = _mapper.Map<IEnumerable<TodoItemDto>>(todos);
        return Result<IEnumerable<TodoItemDto>>.Success(dtos);
    }
}