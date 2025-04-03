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
    public class QuizService(IQuizRepository _quizRepository, IMapper _mapper) : IQuizService
    { 
        public async Task<IEnumerable<QuizDTO>> GetAllAsync()
        {
            var quizzes = await _quizRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<QuizDTO>>(quizzes);
        }

        public async Task<QuizDTO> GetByIdAsync(int id)
        {
            var quiz = await _quizRepository.GetByIdAsync(id);
            return _mapper.Map<QuizDTO>(quiz);
        }

        public async Task<QuizDTO> CreateAsync(QuizDTO quizDto)
        {
            var quiz = _mapper.Map<Quiz>(quizDto);
            var createdQuiz = await _quizRepository.CreateAsync(quiz);
            return _mapper.Map<QuizDTO>(createdQuiz);
        }

        public async Task<QuizDTO> UpdateAsync(int id, QuizDTO quizDto)
        {
            var quiz = _mapper.Map<Quiz>(quizDto);
            quiz.Id = id; 
            var updatedQuiz = await _quizRepository.UpdateAsync(quiz);
            return _mapper.Map<QuizDTO>(updatedQuiz);
        }

        public async Task DeleteAsync(int id)
        {
            await _quizRepository.DeleteAsync(id);
        }
    }  
}
