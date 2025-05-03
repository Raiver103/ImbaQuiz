using FluentValidation;
using ImbaQuiz.Application.DTOs;
using ImbaQuiz.Domain.Interfaces;

public class QuizDtoValidator : AbstractValidator<QuizDTO>
{
    public QuizDtoValidator(IQuizRepository quizRepository)
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Название квиза обязательно")
            .MaximumLength(300);

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId обязателен");
    }
}
