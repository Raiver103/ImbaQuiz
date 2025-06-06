using FluentValidation;
using ImbaQuiz.Application.DTOs;

namespace ImbaQuiz.Application.Validators
{
    public class AnswerDtoValidator : AbstractValidator<AnswerDTO>
    {
        public AnswerDtoValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Ответ не может быть пустым")
                .MaximumLength(300);

            RuleFor(x => x.QuestionId)
                .GreaterThan(0).WithMessage("Некорректный ID вопроса");
        }
    }
}