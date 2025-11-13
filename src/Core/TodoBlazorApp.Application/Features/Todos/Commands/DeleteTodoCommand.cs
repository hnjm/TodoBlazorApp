using MediatR;
using TodoBlazorApp.Domain.Interfaces;

namespace TodoBlazorApp.Application.Features.Todos.Commands;

public record DeleteTodoCommand(int Id) : IRequest<Result<bool>>;

public class DeleteTodoCommandHandler : IRequestHandler<DeleteTodoCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTodoCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
    {
        var todoItem = await _unitOfWork.TodoItems.GetByIdAsync(request.Id);
        if (todoItem == null)
        {
            return Result<bool>.Failure($"المهمة رقم {request.Id} غير موجودة");
        }

        await _unitOfWork.TodoItems.DeleteAsync(todoItem);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}