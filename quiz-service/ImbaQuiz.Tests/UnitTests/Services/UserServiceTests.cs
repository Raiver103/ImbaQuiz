using AutoFixture;
using AutoMapper;
using ImbaQuiz.Application.DTOs;
using ImbaQuiz.Application.Mapping;
using ImbaQuiz.Application.Services;
using ImbaQuiz.Domain.Entities;
using ImbaQuiz.Domain.Interfaces;
using Moq;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ImbaQuiz.Tests.UnitTests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly IMapper _mapper;
        private readonly UserService _userService;
        private readonly Fixture _fixture;

        public UserServiceTests()
        { 
            _fixture = new Fixture();
             
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
             
            _userRepositoryMock = new Mock<IUserRepository>();
             
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = config.CreateMapper();
             
            _userService = new UserService(_userRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnUsers()
        {
            // Arrange 
            var users = _fixture.CreateMany<User>(2).ToList();

            _userRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
                               .ReturnsAsync(users);

            // Act
            var result = await _userService.GetAllAsync(CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBe(2);
            var resultList = result.ToList();
            for (int i = 0; i < users.Count; i++)
            {
                resultList[i].Id.ShouldBe(users[i].Id);
                resultList[i].Name.ShouldBe(users[i].Name);
            }
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnUser_WhenExists()
        {
            // Arrange 
            var user = _fixture.Create<User>();

            _userRepositoryMock.Setup(repo => repo.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(user);

            // Act
            var result = await _userService.GetByIdAsync(user.Id, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(user.Id);
            result.Name.ShouldBe(user.Name);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedUser()
        {
            // Arrange  
            var userDto = _fixture.Create<UserDTO>();
            var user = _mapper.Map<User>(userDto);

            _userRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(user);

            // Act
            var result = await _userService.CreateAsync(userDto, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(user.Id);
            result.Name.ShouldBe(user.Name);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnUpdatedUser()
        {
            // Arrange 
            var userId = _fixture.Create<string>();
            var userDto = _fixture.Create<UserDTO>();
            var user = _mapper.Map<User>(userDto);
            user.Id = userId;

            _userRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(user);

            // Act
            var result = await _userService.UpdateAsync(userId, userDto, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(userId);
            result.Name.ShouldBe(userDto.Name);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepositoryOnce()
        {
            // Arrange 
            var userId = _fixture.Create<string>();

            _userRepositoryMock.Setup(repo => repo.DeleteAsync(userId, It.IsAny<CancellationToken>()))
                               .Returns(Task.CompletedTask)
                               .Verifiable();

            // Act
            await _userService.DeleteAsync(userId, CancellationToken.None);

            // Assert
            _userRepositoryMock.Verify(repo => repo.DeleteAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
