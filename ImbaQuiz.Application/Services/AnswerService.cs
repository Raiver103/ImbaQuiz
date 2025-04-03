﻿using AutoMapper;
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
    public class AnswerService(IAnswerRepository _answerRepository, IMapper _mapper) : IAnswerService
    {
        public async Task<IEnumerable<AnswerDTO>> GetAllAsync(CancellationToken cancellationToken)
        {
            var answers = await _answerRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<AnswerDTO>>(answers);
        }

        public async Task<AnswerDTO> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var answer = await _answerRepository.GetByIdAsync(id, cancellationToken);
            return _mapper.Map<AnswerDTO>(answer);
        }

        public async Task<AnswerDTO> CreateAsync(AnswerDTO answerDto, CancellationToken cancellationToken)
        {
            var answer = _mapper.Map<Answer>(answerDto);
            var createdAnswer = await _answerRepository.CreateAsync(answer, cancellationToken);
            return _mapper.Map<AnswerDTO>(createdAnswer);
        }

        public async Task<AnswerDTO> UpdateAsync(int id, AnswerDTO answerDto, CancellationToken cancellationToken)
        {
            var answer = _mapper.Map<Answer>(answerDto);
            answer.Id = id;
            var updatedAnswer = await _answerRepository.UpdateAsync(answer, cancellationToken);
            return _mapper.Map<AnswerDTO>(updatedAnswer);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            await _answerRepository.DeleteAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<AnswerDTO>> GetByQuestionIdAsync(int questionId, CancellationToken cancellationToken)
        {
            var answers = await _answerRepository.GetByQuestionIdAsync(questionId, cancellationToken);
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
