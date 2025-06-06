using ImbaQuiz.Application.DTOs;

namespace ImbaQuiz.Application.Interfaces
{
    public interface IAnswerService
    {
        Task<IEnumerable<AnswerDTO>> GetAllAsync(CancellationToken cancellationToken);
        Task<AnswerDTO> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<AnswerDTO> CreateAsync(AnswerDTO answerDto, CancellationToken cancellationToken);
        Task<AnswerDTO> UpdateAsync(int id, AnswerDTO answerDto, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<AnswerDTO>> GetByQuestionIdAsync(int questionId, CancellationToken cancellationToken);
    }


}
