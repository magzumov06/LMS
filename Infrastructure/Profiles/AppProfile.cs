using AutoMapper;
using Domain.Dtos.AnswerDto;
using Domain.Dtos.CourseDto;
using Domain.Dtos.DiscussionPostDto;
using Domain.Dtos.LessonDto;
using Domain.Dtos.ProgresDto;
using Domain.Dtos.QuizDto;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Profiles;

public class AppProfile :  Profile
{
    public AppProfile()
    {
        //Course AutoMapper
        CreateMap<CreateCourseDto, Course>().ReverseMap();
        CreateMap<UpdateCourseDto, Course>().ReverseMap();
        CreateMap<Course,GetCourseDto>().ReverseMap();
        
        //Answer AutoMapper
        CreateMap<CreateAnswerDto,  Answer>();
        CreateMap<UpdateAnswerDto,  Answer>();
        CreateMap<Answer,GetAnswerDto>();
        
        //DiscussionPost AutoMapper
        CreateMap<CreateDiscussionDto, DiscussionPost>();
        CreateMap<UpdateDiscussionDto, DiscussionPost>();
        CreateMap<DiscussionPost,GetDiscussionDto>();
        
        //Progress AutoMapper
        CreateMap<CreateProgresDto, Progres>();
        CreateMap<UpdateProgresDto, Progres>();
        CreateMap<Progres,GetProgresDto>();
        
        //Question AutoMapper
        CreateMap<CreateDiscussionDto, DiscussionPost>();
        CreateMap<UpdateDiscussionDto, DiscussionPost>();
        CreateMap<DiscussionPost,GetDiscussionDto>();
        
        //Quiz AutoMapper
        CreateMap<CreateQuizDto, Quiz>();
        CreateMap<UpdateQuizDto, Quiz>();
        CreateMap<Quiz,GetQuizDto>();
    }
}