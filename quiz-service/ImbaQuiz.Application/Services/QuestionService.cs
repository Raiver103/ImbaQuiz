using AutoMapper;
using FluentValidation;
using ImbaQuiz.Application.DTOs;
using ImbaQuiz.Application.Interfaces;
using ImbaQuiz.Domain.Entities;
using ImbaQuiz.Domain.Exceptions;
using ImbaQuiz.Domain.Interfaces;

namespace ImbaQuiz.Application.Services
{
    public class QuestionService(
        IQuestionRepository _questionRepository,
        IMapper _mapper,
        IValidator<QuestionDTO> _validator) : IQuestionService
    {
        public async Task<IEnumerable<QuestionDTO>> GetAllAsync(CancellationToken cancellationToken)
        {
            var questions = await _questionRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<QuestionDTO>>(questions);
        }

        public async Task<QuestionDTO> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var question = await _questionRepository.GetByIdAsync(id, cancellationToken);
            if (question is null)
            {
                throw new NotFoundException($"Quiz with id {id} not found.");
            }
            return _mapper.Map<QuestionDTO>(question);
        }

        public async Task<QuestionDTO> CreateAsync(QuestionDTO questionDto, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(questionDto, cancellationToken);

            var question = _mapper.Map<Question>(questionDto);
            var createdQuestion = await _questionRepository.CreateAsync(question, cancellationToken);
            return _mapper.Map<QuestionDTO>(createdQuestion);
        }

        public async Task<QuestionDTO> UpdateAsync(int id, QuestionDTO questionDto, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(questionDto, cancellationToken);

            var question = _mapper.Map<Question>(questionDto);
            question.Id = id;
            var updatedQuestion = await _questionRepository.UpdateAsync(question, cancellationToken);
            return _mapper.Map<QuestionDTO>(updatedQuestion);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            await _questionRepository.DeleteAsync(id, cancellationToken);
        }

        public async Task<List<QuestionDTO>> GetByQuizIdAsync(int quizId, CancellationToken cancellationToken)
        {
            var questions = await _questionRepository.GetQuestionsByQuizIdAsync(quizId, cancellationToken);
            return _mapper.Map<List<QuestionDTO>>(questions);
        }
    }
}
