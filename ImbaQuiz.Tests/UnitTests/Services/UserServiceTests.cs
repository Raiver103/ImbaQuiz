using AutoMapper;
using FluentAssertions;
using ImbaQuiz.Application.DTOs;
using ImbaQuiz.Application.Services;
using ImbaQuiz.Domain.Entities;
using ImbaQuiz.Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImbaQuiz.Tests.UnitTests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly IMapper _mapper;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDTO>().ReverseMap();
            });

            _mapper = config.CreateMapper();
            _userService = new UserService(_userRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = "1", Name = "Alice" },
                new User { Id = "2", Name = "Bob" }
            };

            _userRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
                               .ReturnsAsync(users);

            // Act
            var result = await _userService.GetAllAsync(CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnUser_WhenExists()
        {
            // Arrange
            var user = new User { Id = "1", Name = "Alice" };

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync("1", It.IsAny<CancellationToken>()))
                               .ReturnsAsync(user);

            // Act
            var result = await _userService.GetByIdAsync("1", CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be("1");
            result.Name.Should().Be("Alice");
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedUser()
        {
            // Arrange
            var userDto = new UserDTO { Id = "1", Name = "Charlie" };
            var user = new User { Id = "1", Name = "Charlie" };

            _userRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(user);

            // Act
            var result = await _userService.CreateAsync(userDto, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be("1");
            result.Name.Should().Be("Charlie");
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnUpdatedUser()
        {
            // Arrange
            var userDto = new UserDTO { Id = "1", Name = "Updated Name" };
            var user = new User { Id = "1", Name = "Updated Name" };

            _userRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(user);

            // Act
            var result = await _userService.UpdateAsync("1", userDto, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be("1");
            result.Name.Should().Be("Updated Name");
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepositoryOnce()
        {
            // Arrange
            _userRepositoryMock.Setup(repo => repo.DeleteAsync("1", It.IsAny<CancellationToken>()))
                               .Returns(Task.CompletedTask)
                               .Verifiable();

            // Act
            await _userService.DeleteAsync("1", CancellationToken.None);

            // Assert
            _userRepositoryMock.Verify(repo => repo.DeleteAsync("1", It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
