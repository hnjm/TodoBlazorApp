using AutoMapper;
using FluentValidation;
using MediatR;
using TodoBlazorApp.Application.DTOs;
using TodoBlazorApp.Domain.Interfaces;

namespace TodoBlazorApp.Application.Features.Todos.Commands;

public record UpdateTodoCommand(UpdateTodoItemDto Todo, string ModifiedBy) : IRequest<Result<TodoItemDto>>;

public class UpdateTodoCommandHandler : IRequestHandler<UpdateTodoCommand, Result<TodoItemDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<UpdateTodoItemDto> _validator;

    public UpdateTodoCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<UpdateTodoItemDto> validator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Result<TodoItemDto>> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request.Todo, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result<TodoItemDto>.Failure(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
        }

        var existingItem = await _unitOfWork.TodoItems.GetByIdAsync(request.Todo.Id);
        if (existingItem == null)
        {
            return Result<TodoItemDto>.Failure($"المهمة رقم {request.Todo.Id} غير موجودة");
        }

        _mapper.Map(request.Todo, existingItem);
        existingItem.ModifiedBy = request.ModifiedBy;

        await _unitOfWork.TodoItems.UpdateAsync(existingItem);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<TodoItemDto>(existingItem);
        return Result<TodoItemDto>.Success(dto);
    }
}