using FluentValidation;
using TodoBlazorApp.Application.DTOs;

namespace TodoBlazorApp.Application.Validators;

public class CreateTodoItemValidator : AbstractValidator<CreateTodoItemDto>
{
    public CreateTodoItemValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("العنوان مطلوب")
            .MaximumLength(200).WithMessage("العنوان يجب أن لا يتجاوز 200 حرف")
            .MinimumLength(3).WithMessage("العنوان يجب أن يكون 3 أحرف على الأقل");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("الوصف يجب أن لا يتجاوز 2000 حرف");

        RuleFor(x => x.Priority)
            .IsInEnum().WithMessage("الأولوية غير صالحة");

        RuleFor(x => x.Category)
            .MaximumLength(50).WithMessage("التصنيف يجب أن لا يتجاوز 50 حرف");

        RuleFor(x => x.Tags)
            .MaximumLength(200).WithMessage("الوسوم يجب أن لا تتجاوز 200 حرف");

        RuleFor(x => x.DueDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .When(x => x.DueDate.HasValue)
            .WithMessage("تاريخ الاستحقاق يجب أن يكون في المستقبل");

        RuleFor(x => x.EstimatedHours)
            .GreaterThan(0)
            .When(x => x.EstimatedHours.HasValue)
            .WithMessage("الساعات المقدرة يجب أن تكون أكبر من صفر");
    }
}

public class UpdateTodoItemValidator : AbstractValidator<UpdateTodoItemDto>
{
    public UpdateTodoItemValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("معرف المهمة غير صالح");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("العنوان مطلوب")
            .MaximumLength(200).WithMessage("العنوان يجب أن لا يتجاوز 200 حرف")
            .MinimumLength(3).WithMessage("العنوان يجب أن يكون 3 أحرف على الأقل");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("الوصف يجب أن لا يتجاوز 2000 حرف");

        RuleFor(x => x.Priority)
            .IsInEnum().WithMessage("الأولوية غير صالحة");

        RuleFor(x => x.Category)
            .MaximumLength(50).WithMessage("التصنيف يجب أن لا يتجاوز 50 حرف");

        RuleFor(x => x.Tags)
            .MaximumLength(200).WithMessage("الوسوم يجب أن لا تتجاوز 200 حرف");

        RuleFor(x => x.ActualHours)
            .GreaterThan(0)
            .When(x => x.ActualHours.HasValue)
            .WithMessage("الساعات الفعلية يجب أن تكون أكبر من صفر");
    }
}

public class CreateTodoCommentValidator : AbstractValidator<CreateTodoCommentDto>
{
    public CreateTodoCommentValidator()
    {
        RuleFor(x => x.TodoItemId)
            .GreaterThan(0).WithMessage("معرف المهمة غير صالح");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("المحتوى مطلوب")
            .MaximumLength(1000).WithMessage("المحتوى يجب أن لا يتجاوز 1000 حرف");
    }
}