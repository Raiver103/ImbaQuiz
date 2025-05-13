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
    public class AnswerServiceTests
    {
        private readonly Mock<IAnswerRepository> _answerRepositoryMock;
        private readonly IMapper _mapper;
        private readonly AnswerService _answerService;
        private readonly Fixture _fixture;
        private readonly Mock<IValidator<AnswerDTO>> _validatorMock;    
        public AnswerServiceTests()
        {
            _answerRepositoryMock = new Mock<IAnswerRepository>();
            _validatorMock = new Mock<IValidator<AnswerDTO>>();

            _fixture = new Fixture();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = config.CreateMapper();
            _answerService = new AnswerService(_answerRepositoryMock.Object, _mapper, _validatorMock.Object);
             
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAnswers()
        {
            // Arrange
            var answers = _fixture.CreateMany<Answer>(2).ToList();

            _answerRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
                               .ReturnsAsync(answers);

            // Act
            var result = (await _answerService.GetAllAsync(CancellationToken.None)).ToList();

            // Assert
            result.ShouldNotBeNull();
            result.Count.ShouldBe(2);

            for (int i = 0; i < answers.Count; i++)
            {
                result[i].Id.ShouldBe(answers[i].Id);
                result[i].Text.ShouldBe(answers[i].Text);
                result[i].IsCorrect.ShouldBe(answers[i].IsCorrect);
                result[i].QuestionId.ShouldBe(answers[i].QuestionId);
            }
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnAnswer_WhenExists()
        {
            // Arrange
            var answer = _fixture.Create<Answer>();

            _answerRepositoryMock.Setup(repo => repo.GetByIdAsync(answer.Id, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(answer);

            // Act
            var result = await _answerService.GetByIdAsync(answer.Id, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(answer.Id);
            result.Text.ShouldBe(answer.Text);
            result.IsCorrect.ShouldBe(answer.IsCorrect);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedAnswer()
        {
            // Arrange
            var answerDto = _fixture.Create<AnswerDTO>();
            var answer = _mapper.Map<Answer>(answerDto);

            _answerRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Answer>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(answer);

            // Act
            var result = await _answerService.CreateAsync(answerDto, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(answer.Id);
            result.Text.ShouldBe(answer.Text);
            result.IsCorrect.ShouldBe(answer.IsCorrect);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnUpdatedAnswer()
        {
            // Arrange
            var answerId = _fixture.Create<int>();
            var answerDto = _fixture.Create<AnswerDTO>();
            var answer = _mapper.Map<Answer>(answerDto);
            answer.Id = answerId;

            _answerRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Answer>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(answer);

            // Act
            var result = await _answerService.UpdateAsync(answerId, answerDto, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(answerId);
            result.Text.ShouldBe(answerDto.Text);
            result.IsCorrect.ShouldBe(answerDto.IsCorrect);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepositoryOnce()
        {
            // Arrange
            var answerId = _fixture.Create<int>();

            _answerRepositoryMock.Setup(repo => repo.DeleteAsync(answerId, It.IsAny<CancellationToken>()))
                               .Returns(Task.CompletedTask)
                               .Verifiable();

            // Act
            await _answerService.DeleteAsync(answerId, CancellationToken.None);

            // Assert
            _answerRepositoryMock.Verify(repo => repo.DeleteAsync(answerId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetByQuestionIdAsync_ShouldReturnAnswersForQuestion()
        {
            // Arrange
            var questionId = _fixture.Create<int>();
            var answers = _fixture.Build<Answer>()
                                .With(a => a.QuestionId, questionId)
                                .CreateMany(2)
                                .ToList();

            _answerRepositoryMock.Setup(repo => repo.GetByQuestionIdAsync(questionId, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(answers);

            // Act
            var result = await _answerService.GetByQuestionIdAsync(questionId, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Count().ShouldBe(2);
            result.All(a => a.QuestionId == questionId).ShouldBeTrue();
        }
    }
}