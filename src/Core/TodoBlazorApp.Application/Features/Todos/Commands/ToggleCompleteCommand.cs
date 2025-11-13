using AutoMapper;
using MediatR;
using TodoBlazorApp.Application.DTOs;
using TodoBlazorApp.Domain.Interfaces;

namespace TodoBlazorApp.Application.Features.Todos.Commands;

public record ToggleCompleteCommand(int Id, string ModifiedBy) : IRequest<Result<TodoItemDto>>;

public class ToggleCompleteCommandHandler : IRequestHandler<ToggleCompleteCommand, Result<TodoItemDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ToggleCompleteCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<TodoItemDto>> Handle(ToggleCompleteCommand request, CancellationToken cancellationToken)
    {
        var todoItem = await _unitOfWork.TodoItems.GetByIdAsync(request.Id);
        if (todoItem == null)
        {
            return Result<TodoItemDto>.Failure($"المهمة رقم {request.Id} غير موجودة");
        }

        if (todoItem.IsCompleted)
        {
            todoItem.MarkAsIncomplete(request.ModifiedBy);
        }
        else
        {
            todoItem.MarkAsCompleted(request.ModifiedBy);
        }

        await _unitOfWork.TodoItems.UpdateAsync(todoItem);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<TodoItemDto>(todoItem);
        return Result<TodoItemDto>.Success(dto);
    }
}