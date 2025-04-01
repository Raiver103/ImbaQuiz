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
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IMapper _mapper;

        public QuestionService(IQuestionRepository questionRepository, IMapper mapper)
        {
            _questionRepository = questionRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<QuestionDTO>> GetAllAsync()
        {
            var questions = await _questionRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<QuestionDTO>>(questions);
        }

        public async Task<QuestionDTO> GetByIdAsync(int id)
        {
            var question = await _questionRepository.GetByIdAsync(id);
            return _mapper.Map<QuestionDTO>(question);
        }

        public async Task<QuestionDTO> CreateAsync(QuestionDTO questionDto)
        {
            var question = _mapper.Map<Question>(questionDto);
            var createdQuestion = await _questionRepository.CreateAsync(question);
            return _mapper.Map<QuestionDTO>(createdQuestion);
        }

        public async Task<QuestionDTO> UpdateAsync(int id, QuestionDTO questionDto)
        {
            var question = _mapper.Map<Question>(questionDto);
            question.Id = id; // Устанавливаем ID для обновления
            var updatedQuestion = await _questionRepository.UpdateAsync(question);
            return _mapper.Map<QuestionDTO>(updatedQuestion);
        }

        public async Task DeleteAsync(int id)
        {
            await _questionRepository.DeleteAsync(id);
        }

        public async Task<List<QuestionDTO>> GetByQuizIdAsync(int quizId)
        {
            var questions = await _questionRepository.GetQuestionsByQuizIdAsync(quizId);
            return _mapper.Map<List<QuestionDTO>>(questions);  // Используем AutoMapper для маппинга в DTO
        }

    }
}
