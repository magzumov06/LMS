using System.Net;
using AutoMapper;
using Domain.Dtos.QuestionDto;
using Domain.Dtos.QuizDto;
using Domain.Entities;
using Domain.Responces;
using Infrastructure.Data.DataContext;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class QuizService(DataContext context,
    IMapper mapper) : IQuizService
{
    public async Task<Response<string>> CreateQuiz(CreateQuizDto dto)
    {
        try
        {
           var quiz = mapper.Map<Quiz>(dto);
           quiz.CreatedAt = DateTime.UtcNow;
           quiz.UpdatedAt = DateTime.UtcNow;
           await context.Quizzes.AddAsync(quiz);
           await context.SaveChangesAsync();
           var created =  await context.Quizzes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == quiz.Id);
           return new Response<string>(mapper.Map<string>(created));
        }
        catch (Exception e)
        {
            return new  Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> UpdateQuiz(UpdateQuizDto dto)
    {
        try
        {
            var quiz = await context.Quizzes.FirstOrDefaultAsync(x=>x.Id == dto.Id);
            if (quiz == null) return new  Response<string>(HttpStatusCode.NotFound,"Quiz not found");
            quiz.UpdatedAt = DateTime.UtcNow;
            mapper.Map(dto, quiz);
            await context.SaveChangesAsync();
            var updated =  await context.Quizzes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == quiz.Id);
            return new Response<string>(mapper.Map<string>(updated));
        }
        catch (Exception e)
        {
            return new  Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> DeleteQuiz(int id)
    {
        try
        {
            var quiz = await context.Quizzes.FirstOrDefaultAsync(x => x.Id == id);
            if (quiz == null) return new  Response<string>(HttpStatusCode.NotFound,"Quiz not found");
            context.Quizzes.Remove(quiz);
            var res = await context.SaveChangesAsync();
            return res > 0
            ? new  Response<string>(HttpStatusCode.OK,"Quiz Deleted")
            : new  Response<string>(HttpStatusCode.NotFound,"Quiz not found");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<GetQuizDto>> GetQuizById(int id)
    {
        try
        {
            var quiz = await context.Quizzes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (quiz == null) return new Response<GetQuizDto>(HttpStatusCode.NotFound,"Quiz not found");
            return new Response<GetQuizDto>(mapper.Map<GetQuizDto>(quiz));
        }
        catch (Exception e)
        {
            return new  Response<GetQuizDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<List<GetQuizDto>>> GetQuizs()
    {
        try
        {
            var quizs = await context.Quizzes.ToListAsync();
            if(quizs.Count == 0) return new  Response<List<GetQuizDto>>(HttpStatusCode.NotFound,"Quiz not found");
            var dtos = quizs.Select(x=>new GetQuizDto()
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                CourseId = x.CourseId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
            }).ToList();
            return new  Response<List<GetQuizDto>>(dtos);
        }
        catch (Exception e)
        {
            return new  Response<List<GetQuizDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}