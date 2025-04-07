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
    public class QuestionServiceTests
    {
        private readonly Mock<IQuestionRepository> _questionRepositoryMock;
        private readonly IMapper _mapper;
        private readonly QuestionService _questionService;
        private readonly Fixture _fixture;

        public QuestionServiceTests()
        {
            _questionRepositoryMock = new Mock<IQuestionRepository>();
            _fixture = new Fixture();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = config.CreateMapper();
            _questionService = new QuestionService(_questionRepositoryMock.Object, _mapper);
             
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnQuestions()
        {
            // Arrange
            var questions = _fixture.Build<Question>()
                                   .Without(q => q.Quiz)  
                                   .Without(q => q.Answers)  
                                   .CreateMany(2)
                                   .ToList();

            _questionRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(questions);

            // Act
            var result = await _questionService.GetAllAsync(CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBe(2);
             
            var resultList = result.ToList();
            for (int i = 0; i < questions.Count; i++)
            {
                resultList[i].Id.ShouldBe(questions[i].Id);
                resultList[i].Text.ShouldBe(questions[i].Text);
                resultList[i].QuizId.ShouldBe(questions[i].QuizId);
            }
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnQuestion_WhenExists()
        {
            // Arrange
            var question = _fixture.Create<Question>();

            _questionRepositoryMock.Setup(repo => repo.GetByIdAsync(question.Id, It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(question);

            // Act
            var result = await _questionService.GetByIdAsync(question.Id, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(question.Id);
            result.Text.ShouldBe(question.Text);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedQuestion()
        {
            // Arrange
            var questionDto = _fixture.Create<QuestionDTO>();
            var question = _mapper.Map<Question>(questionDto);

            _questionRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Question>(), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(question);

            // Act
            var result = await _questionService.CreateAsync(questionDto, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(question.Id);
            result.Text.ShouldBe(question.Text);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnUpdatedQuestion()
        {
            // Arrange
            var questionId = _fixture.Create<int>();
            var questionDto = _fixture.Create<QuestionDTO>();
            var question = _mapper.Map<Question>(questionDto);
            question.Id = questionId;

            _questionRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Question>(), It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(question);

            // Act
            var result = await _questionService.UpdateAsync(questionId, questionDto, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(questionId);
            result.Text.ShouldBe(questionDto.Text);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepositoryOnce()
        {
            // Arrange
            var questionId = _fixture.Create<int>();

            _questionRepositoryMock.Setup(repo => repo.DeleteAsync(questionId, It.IsAny<CancellationToken>()))
                                 .Returns(Task.CompletedTask)
                                 .Verifiable();

            // Act
            await _questionService.DeleteAsync(questionId, CancellationToken.None);

            // Assert
            _questionRepositoryMock.Verify(repo => repo.DeleteAsync(questionId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetByQuizIdAsync_ShouldReturnQuestions_WhenExists()
        {
            // Arrange
            var quizId = _fixture.Create<int>();
            var questions = _fixture.Build<Question>()
                                  .With(q => q.QuizId, quizId)
                                  .CreateMany(2)
                                  .ToList();

            _questionRepositoryMock.Setup(repo => repo.GetQuestionsByQuizIdAsync(quizId, It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(questions);

            // Act
            var result = await _questionService.GetByQuizIdAsync(quizId, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBe(2);

            // Verify all returned questions belong to the specified quiz
            var mappedResults = _mapper.Map<List<Question>>(result);
            mappedResults.All(q => q.QuizId == quizId).ShouldBeTrue();
        }
    }
}