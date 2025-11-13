using AutoMapper;
using FluentValidation;
using MediatR;
using TodoBlazorApp.Application.DTOs;
using TodoBlazorApp.Domain.Entities;
using TodoBlazorApp.Domain.Interfaces;

namespace TodoBlazorApp.Application.Features.Todos.Commands;

public record AddCommentCommand(CreateTodoCommentDto Comment, string CreatedBy) : IRequest<Result<TodoCommentDto>>;

public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, Result<TodoCommentDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateTodoCommentDto> _validator;

    public AddCommentCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CreateTodoCommentDto> validator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<Result<TodoCommentDto>> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request.Comment, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result<TodoCommentDto>.Failure(validationResult.Errors.Select(e => e.ErrorMessage).ToList());
        }

        var todoItem = await _unitOfWork.TodoItems.GetByIdAsync(request.Comment.TodoItemId);
        if (todoItem == null)
        {
            return Result<TodoCommentDto>.Failure($"المهمة رقم {request.Comment.TodoItemId} غير موجودة");
        }

        var comment = _mapper.Map<TodoComment>(request.Comment);
        comment.CreatedBy = request.CreatedBy;

        var createdComment = await _unitOfWork.TodoComments.AddAsync(comment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = _mapper.Map<TodoCommentDto>(createdComment);
        return Result<TodoCommentDto>.Success(dto);
    }
}