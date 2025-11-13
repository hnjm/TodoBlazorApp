using AutoMapper;
using MediatR;
using TodoBlazorApp.Application.DTOs;
using TodoBlazorApp.Domain.Interfaces;

namespace TodoBlazorApp.Application.Features.Todos.Queries;

public record GetTodoByIdQuery(int Id, bool IncludeDetails = false) : IRequest<Result<TodoItemDto>>;

public class GetTodoByIdQueryHandler : IRequestHandler<GetTodoByIdQuery, Result<TodoItemDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTodoByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<TodoItemDto>> Handle(GetTodoByIdQuery request, CancellationToken cancellationToken)
    {
        var todo = request.IncludeDetails
            ? await _unitOfWork.TodoItems.GetByIdWithDetailsAsync(request.Id)
            : await _unitOfWork.TodoItems.GetByIdAsync(request.Id);

        if (todo == null)
        {
            return Result<TodoItemDto>.Failure($"المهمة رقم {request.Id} غير موجودة");
        }

        var dto = _mapper.Map<TodoItemDto>(todo);
        return Result<TodoItemDto>.Success(dto);
    }
}