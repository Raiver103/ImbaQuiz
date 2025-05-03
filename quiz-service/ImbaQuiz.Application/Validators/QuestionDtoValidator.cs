using FluentValidation;
using ImbaQuiz.Application.DTOs;

public class QuestionDtoValidator : AbstractValidator<QuestionDTO>
{
    public QuestionDtoValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Текст вопроса обязателен") 
            .MaximumLength(300);

        RuleFor(x => x.QuizId)
            .GreaterThan(0).WithMessage("Некорректный ID квиза");
    }
}
