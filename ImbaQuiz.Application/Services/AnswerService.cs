using AutoMapper;
using ImbaQuiz.Application.DTOs;
using ImbaQuiz.Application.Interfaces;
using ImbaQuiz.Domain.Entities;
using ImbaQuiz.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImbaQuiz.Application.Services
{
    public class AnswerService : IAnswerService
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly IMapper _mapper;

        public AnswerService(IAnswerRepository answerRepository, IMapper mapper)
        {
            _answerRepository = answerRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AnswerDTO>> GetAllAsync()
        {
            var answers = await _answerRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<AnswerDTO>>(answers);
        }

        public async Task<AnswerDTO> GetByIdAsync(int id)
        {
            var answer = await _answerRepository.GetByIdAsync(id);
            return _mapper.Map<AnswerDTO>(answer);
        }

        public async Task<AnswerDTO> CreateAsync(AnswerDTO answerDto)
        {
            var answer = _mapper.Map<Answer>(answerDto);
            var createdAnswer = await _answerRepository.CreateAsync(answer);
            return _mapper.Map<AnswerDTO>(createdAnswer);
        }

        public async Task<AnswerDTO> UpdateAsync(int id, AnswerDTO answerDto)
        {
            var answer = _mapper.Map<Answer>(answerDto);
            answer.Id = id; // Устанавливаем ID для обновления
            var updatedAnswer = await _answerRepository.UpdateAsync(answer);
            return _mapper.Map<AnswerDTO>(updatedAnswer);
        }

        public async Task DeleteAsync(int id)
        {
            await _answerRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<AnswerDTO>> GetByQuestionIdAsync(int questionId)
        {
            // Реализация получения всех ответов для конкретного вопроса
            var answers = await _answerRepository.GetByQuestionIdAsync(questionId);
            return answers.Select(a => new AnswerDTO
            {
                Id = a.Id,
                Text = a.Text,
                IsCorrect = a.IsCorrect,
                QuestionId = a.QuestionId
            });
        }
    }
}
