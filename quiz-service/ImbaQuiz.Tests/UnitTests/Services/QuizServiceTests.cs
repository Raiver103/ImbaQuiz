using AutoFixture;
using AutoMapper;
using FluentValidation;
using ImbaQuiz.Application.DTOs;
using ImbaQuiz.Application.Mapping;
using ImbaQuiz.Application.Services;
using ImbaQuiz.Domain.Entities;
using ImbaQuiz.Domain.Interfaces;
using Moq;
using Shouldly; 

namespace ImbaQuiz.Tests.UnitTests.Services
{
    public class QuizServiceTests
    {
        private readonly Mock<IQuizRepository> _quizRepositoryMock;
        private readonly IMapper _mapper;
        private readonly QuizService _quizService;
        private readonly Fixture _fixture;
        private readonly Mock<IValidator<QuizDTO>> _validatorMock;    

        public QuizServiceTests()
        {
            _quizRepositoryMock = new Mock<IQuizRepository>();
            _validatorMock = new Mock<IValidator<QuizDTO>>();
            _fixture = new Fixture();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = config.CreateMapper();
            _quizService = new QuizService(_quizRepositoryMock.Object, _mapper, _validatorMock.Object);
             
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnQuizzes()
        {
            // Arrange
            var quizzes = _fixture.Build<Quiz>()
                                .Without(q => q.Questions)  
                                .CreateMany(2)
                                .ToList();

            _quizRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
                             .ReturnsAsync(quizzes);

            // Act
            var result = await _quizService.GetAllAsync(CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBe(2);
             
            var resultList = result.ToList();
            for (int i = 0; i < quizzes.Count; i++)
            {
                resultList[i].Id.ShouldBe(quizzes[i].Id);
                resultList[i].Title.ShouldBe(quizzes[i].Title);
            }
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnQuiz_WhenExists()
        {
            // Arrange
            var quiz = _fixture.Build<Quiz>()
                             .Without(q => q.Questions)
                             .Create();

            _quizRepositoryMock.Setup(repo => repo.GetByIdAsync(quiz.Id, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(quiz);

            // Act
            var result = await _quizService.GetByIdAsync(quiz.Id, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(quiz.Id);
            result.Title.ShouldBe(quiz.Title);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedQuiz()
        {
            // Arrange
            var quizDto = _fixture.Create<QuizDTO>();
            var quiz = _mapper.Map<Quiz>(quizDto);

            _quizRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Quiz>(), It.IsAny<CancellationToken>()))
                             .ReturnsAsync(quiz);

            // Act
            var result = await _quizService.CreateAsync(quizDto, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(quiz.Id);
            result.Title.ShouldBe(quiz.Title);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnUpdatedQuiz()
        {
            // Arrange
            var quizId = _fixture.Create<int>();
            var quizDto = _fixture.Create<QuizDTO>();
            var quiz = _mapper.Map<Quiz>(quizDto);
            quiz.Id = quizId;

            _quizRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Quiz>(), It.IsAny<CancellationToken>()))
                             .ReturnsAsync(quiz);

            // Act
            var result = await _quizService.UpdateAsync(quizId, quizDto, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(quizId);
            result.Title.ShouldBe(quizDto.Title);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepositoryOnce()
        {
            // Arrange
            var quizId = _fixture.Create<int>();

            _quizRepositoryMock.Setup(repo => repo.DeleteAsync(quizId, It.IsAny<CancellationToken>()))
                             .Returns(Task.CompletedTask)
                             .Verifiable();

            // Act
            await _quizService.DeleteAsync(quizId, CancellationToken.None);

            // Assert
            _quizRepositoryMock.Verify(repo => repo.DeleteAsync(quizId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}