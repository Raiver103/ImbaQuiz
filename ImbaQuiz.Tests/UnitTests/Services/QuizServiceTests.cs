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
    public class QuizServiceTests
    {
        private readonly Mock<IQuizRepository> _quizRepositoryMock;
        private readonly IMapper _mapper;
        private readonly QuizService _quizService;

        public QuizServiceTests()
        {
            _quizRepositoryMock = new Mock<IQuizRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Quiz, QuizDTO>().ReverseMap();
            });

            _mapper = config.CreateMapper();
            _quizService = new QuizService(_quizRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnQuizzes()
        {
            // Arrange
            var quizzes = new List<Quiz>
            {
                new Quiz { Id = 1, Title = "Math Quiz" },
                new Quiz { Id = 2, Title = "Science Quiz" }
            };

            _quizRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
                               .ReturnsAsync(quizzes);

            // Act
            var result = await _quizService.GetAllAsync(CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnQuiz_WhenExists()
        {
            // Arrange
            var quiz = new Quiz { Id = 1, Title = "Math Quiz" };

            _quizRepositoryMock.Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(quiz);

            // Act
            var result = await _quizService.GetByIdAsync(1, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Title.Should().Be("Math Quiz");
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedQuiz()
        {
            // Arrange
            var quizDto = new QuizDTO { Id = 1, Title = "History Quiz" };
            var quiz = new Quiz { Id = 1, Title = "History Quiz" };

            _quizRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Quiz>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(quiz);

            // Act
            var result = await _quizService.CreateAsync(quizDto, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Title.Should().Be("History Quiz");
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnUpdatedQuiz()
        {
            // Arrange
            var quizDto = new QuizDTO { Id = 1, Title = "Updated Quiz" };
            var quiz = new Quiz { Id = 1, Title = "Updated Quiz" };

            _quizRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Quiz>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(quiz);

            // Act
            var result = await _quizService.UpdateAsync(1, quizDto, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Title.Should().Be("Updated Quiz");
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepositoryOnce()
        {
            // Arrange
            _quizRepositoryMock.Setup(repo => repo.DeleteAsync(1, It.IsAny<CancellationToken>()))
                               .Returns(Task.CompletedTask)
                               .Verifiable();

            // Act
            await _quizService.DeleteAsync(1, CancellationToken.None);

            // Assert
            _quizRepositoryMock.Verify(repo => repo.DeleteAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
