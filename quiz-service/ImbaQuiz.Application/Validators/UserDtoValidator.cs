using FluentValidation;
using ImbaQuiz.Application.DTOs;

namespace ImbaQuiz.Application.Validators
{
    public class UserDtoValidator : AbstractValidator<UserDTO>
    {
        public UserDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Имя обязательно")
                .MaximumLength(300);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email обязателен")
                .EmailAddress().WithMessage("Некорректный формат email");
        }
    }
}