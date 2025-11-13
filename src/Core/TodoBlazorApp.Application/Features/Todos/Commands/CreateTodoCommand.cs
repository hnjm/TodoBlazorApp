using AutoMapper;
using FluentValidation;
using MediatR;
using TodoBlazorApp.Application.DTOs;
using TodoBlazorApp.Domain.Entities;
using TodoBlazorApp.Domain.Interfaces;

namespace TodoBlazorApp.Application.Features.Todos.Commands;

public record CreateTodoCommand(CreateTodoItemDto Todo, string CreatedBy) : IRequest<Result<TodoItemDto>>;

public class CreateTodoCommandHandler : IRequestHandler<CreateTodoCommand, Result<TodoItemDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateTodoItemDto> _validator;

    public CreateTodoCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CreateTodoItemDto> validator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Result<TodoItemDto>> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request.Todo, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result<TodoItemDto>.Failure(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
        }

        var todoItem = _mapper.Map<TodoItem>(request.Todo);
        todoItem.CreatedBy = request.CreatedBy;

        var createdItem = await _unitOfWork.TodoItems.AddAsync(todoItem);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<TodoItemDto>(createdItem);
        return Result<TodoItemDto>.Success(dto);
    }
}