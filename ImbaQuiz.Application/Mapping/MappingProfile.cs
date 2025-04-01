using AutoMapper;
using ImbaQuiz.Application.DTOs;
using ImbaQuiz.Domain.Entities; 

namespace ImbaQuiz.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        { 
            CreateMap<User, UserDTO>().ReverseMap(); 
            CreateMap<Quiz, QuizDTO>().ReverseMap(); 
            CreateMap<Question, QuestionDTO>().ReverseMap(); 
            CreateMap<Answer, AnswerDTO>().ReverseMap();
        }
    }
}
