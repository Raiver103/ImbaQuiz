using AutoMapper;
using ImbaQuiz.Application.DTOs;
using ImbaQuiz.Application.Interfaces;
using ImbaQuiz.Domain.Entities;
using ImbaQuiz.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ImbaQuiz.Application.Services
{
    public class UserService : IUserService
    { 
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDTO>> GetAllAsync(CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public async Task<UserDTO> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(id, cancellationToken);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> CreateAsync(UserDTO userDto, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(userDto);
            var createdUser = await _userRepository.CreateAsync(user, cancellationToken);
            return _mapper.Map<UserDTO>(createdUser);
        }

        public async Task<UserDTO> UpdateAsync(string id, UserDTO userDto, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(userDto);
            user.Id = id; 
            var updatedUser = await _userRepository.UpdateAsync(user, cancellationToken);
            return _mapper.Map<UserDTO>(updatedUser);
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken)
        {
            await _userRepository.DeleteAsync(id, cancellationToken);
        }
    } 
}
