using ImbaQuiz.Application.DTOs;
using ImbaQuiz.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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