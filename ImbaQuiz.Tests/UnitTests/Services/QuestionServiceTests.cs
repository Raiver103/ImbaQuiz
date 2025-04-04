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
    public class QuestionServiceTests
    {
        private readonly Mock<IQuestionRepository> _questionRepositoryMock;
        private readonly IMapper _mapper;
        private readonly QuestionService _questionService;

        public QuestionServiceTests()
        {
            _questionRepositoryMock = new Mock<IQuestionRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Question, QuestionDTO>().ReverseMap();
            });

            _mapper = config.CreateMapper();
            _questionService = new QuestionService(_questionRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnQuestions()
        {
            // Arrange
            var questions = new List<Question>
            {
                new Question { Id = 1, Text = "Question 1" },
                new Question { Id = 2, Text = "Question 2" }
            };

            _questionRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
                                   .ReturnsAsync(questions);

            // Act
            var result = await _questionService.GetAllAsync(CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnQuestion_WhenExists()
        {
            // Arrange
            var question = new Question { Id = 1, Text = "Test Question" };

            _questionRepositoryMock.Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                                   .ReturnsAsync(question);

            // Act
            var result = await _questionService.GetByIdAsync(1, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Text.Should().Be("Test Question");
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedQuestion()
        {
            // Arrange
            var questionDto = new QuestionDTO { Id = 1, Text = "New Question" };
            var question = new Question { Id = 1, Text = "New Question" };

            _questionRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Question>(), It.IsAny<CancellationToken>()))
                                   .ReturnsAsync(question);

            // Act
            var result = await _questionService.CreateAsync(questionDto, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Text.Should().Be("New Question");
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnUpdatedQuestion()
        {
            // Arrange
            var questionDto = new QuestionDTO { Id = 1, Text = "Updated Question" };
            var question = new Question { Id = 1, Text = "Updated Question" };

            _questionRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Question>(), It.IsAny<CancellationToken>()))
                                   .ReturnsAsync(question);

            // Act
            var result = await _questionService.UpdateAsync(1, questionDto, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Text.Should().Be("Updated Question");
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepositoryOnce()
        {
            // Arrange
            _questionRepositoryMock.Setup(repo => repo.DeleteAsync(1, It.IsAny<CancellationToken>()))
                                   .Returns(Task.CompletedTask)
                                   .Verifiable();

            // Act
            await _questionService.DeleteAsync(1, CancellationToken.None);

            // Assert
            _questionRepositoryMock.Verify(repo => repo.DeleteAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetByQuizIdAsync_ShouldReturnQuestions_WhenExists()
        {
            // Arrange
            var questions = new List<Question>
            {
                new Question { Id = 1, Text = "Question 1", QuizId = 10 },
                new Question { Id = 2, Text = "Question 2", QuizId = 10 }
            };

            _questionRepositoryMock.Setup(repo => repo.GetQuestionsByQuizIdAsync(10, It.IsAny<CancellationToken>()))
                                   .ReturnsAsync(questions);

            // Act
            var result = await _questionService.GetByQuizIdAsync(10, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }
    }
}
