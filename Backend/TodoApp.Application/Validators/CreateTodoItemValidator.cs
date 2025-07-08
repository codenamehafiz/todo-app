using FluentValidation;
using TodoApp.Application.DTOs;

namespace TodoApp.Application.Validators
{
    public class CreateTodoItemValidator : AbstractValidator<CreateTodoItemDto>
    {
        public CreateTodoItemValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");
        }
    }
}