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
    public class AnswerServiceTests
    {
        private readonly Mock<IAnswerRepository> _answerRepositoryMock;
        private readonly IMapper _mapper;
        private readonly AnswerService _answerService;

        public AnswerServiceTests()
        {
            _answerRepositoryMock = new Mock<IAnswerRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Answer, AnswerDTO>().ReverseMap();
            });

            _mapper = config.CreateMapper();
            _answerService = new AnswerService(_answerRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAnswers()
        {
            // Arrange
            var answers = new List<Answer>
            {
                new Answer { Id = 1, Text = "Answer 1", IsCorrect = true, QuestionId = 1 },
                new Answer { Id = 2, Text = "Answer 2", IsCorrect = false, QuestionId = 1 }
            };

            _answerRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(answers);

            // Act
            var result = await _answerService.GetAllAsync(CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnAnswer_WhenExists()
        {
            // Arrange
            var answer = new Answer { Id = 1, Text = "Answer 1", IsCorrect = true, QuestionId = 1 };

            _answerRepositoryMock.Setup(repo => repo.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(answer);

            // Act
            var result = await _answerService.GetByIdAsync(1, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Text.Should().Be("Answer 1");
            result.IsCorrect.Should().BeTrue();
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedAnswer()
        {
            // Arrange
            var answerDto = new AnswerDTO { Id = 1, Text = "Answer 1", IsCorrect = true, QuestionId = 1 };
            var answer = new Answer { Id = 1, Text = "Answer 1", IsCorrect = true, QuestionId = 1 };

            _answerRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Answer>(), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(answer);

            // Act
            var result = await _answerService.CreateAsync(answerDto, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Text.Should().Be("Answer 1");
            result.IsCorrect.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnUpdatedAnswer()
        {
            // Arrange
            var answerDto = new AnswerDTO { Id = 1, Text = "Updated Answer", IsCorrect = false, QuestionId = 1 };
            var answer = new Answer { Id = 1, Text = "Updated Answer", IsCorrect = false, QuestionId = 1 };

            _answerRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Answer>(), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(answer);

            // Act
            var result = await _answerService.UpdateAsync(1, answerDto, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Text.Should().Be("Updated Answer");
            result.IsCorrect.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepositoryOnce()
        {
            // Arrange
            _answerRepositoryMock.Setup(repo => repo.DeleteAsync(1, It.IsAny<CancellationToken>()))
                                 .Returns(Task.CompletedTask)
                                 .Verifiable();

            // Act
            await _answerService.DeleteAsync(1, CancellationToken.None);

            // Assert
            _answerRepositoryMock.Verify(repo => repo.DeleteAsync(1, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetByQuestionIdAsync_ShouldReturnAnswersForQuestion()
        {
            // Arrange
            var answers = new List<Answer>
            {
                new Answer { Id = 1, Text = "Answer 1", IsCorrect = true, QuestionId = 1 },
                new Answer { Id = 2, Text = "Answer 2", IsCorrect = false, QuestionId = 1 }
            };

            _answerRepositoryMock.Setup(repo => repo.GetByQuestionIdAsync(1, It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(answers);

            // Act
            var result = await _answerService.GetByQuestionIdAsync(1, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }
    }
}
