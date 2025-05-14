using ImbaQuiz.Application.DTOs;

namespace ImbaQuiz.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllAsync(CancellationToken cancellationToken);
        Task<UserDTO> GetByIdAsync(string id, CancellationToken cancellationToken);
        Task<UserDTO> CreateAsync(UserDTO userDto, CancellationToken cancellationToken);
        Task<UserDTO> UpdateAsync(string id, UserDTO userDto, CancellationToken cancellationToken);
        Task DeleteAsync(string id, CancellationToken cancellationToken);
    }
}