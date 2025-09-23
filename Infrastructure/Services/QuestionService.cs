using System.Net;
using AutoMapper;
using Domain.Dtos.QuestionDto;
using Domain.Entities;
using Domain.Responces;
using Infrastructure.Data.DataContext;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class QuestionService(DataContext context,
    IMapper mapper) : IQuestionService
{
    public async Task<Response<string>> CreateQuestion(CreateQuestionDto dto)
    {
        try
        {
           var question =  mapper.Map<Question>(dto);
           question.CreatedAt = DateTime.UtcNow;
           question.UpdatedAt = DateTime.UtcNow;
           await context.Questions.AddAsync(question);
           await context.SaveChangesAsync();
           var created =  await context.Questions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == question.Id);
           return new Response<string>(mapper.Map<string>(created));
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> UpdateQuestion(UpdateQuestionDto dto)
    {
        try
        {
            var question = await context.Questions.FirstOrDefaultAsync(x=>x.Id == dto.Id);
            if (question == null) return new Response<string>(HttpStatusCode.NotFound,"Question not found");
            question.UpdatedAt = DateTime.UtcNow;
            mapper.Map(dto, question);
            await context.SaveChangesAsync();
            var updated =  await context.Questions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == question.Id);
            return new Response<string>(mapper.Map<string>(updated));
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> DeleteQuestion(int id)
    {
        try
        {
            var question = await context.Questions.FirstOrDefaultAsync(x => x.Id == id);
            if (question == null) return new Response<string>(HttpStatusCode.NotFound,"Question not found");
            context.Questions.Remove(question);
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Response<string>(HttpStatusCode.OK, "Question successfully deleted")
                : new Response<string>(HttpStatusCode.BadRequest, "Question could not be deleted");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<GetQuestionDto>> GetQuestionById(int id)
    {
        try
        {
            var  question = await context.Questions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (question == null) return new Response<GetQuestionDto>(HttpStatusCode.NotFound,"Question not found");
            return new Response<GetQuestionDto>(mapper.Map<GetQuestionDto>(question));
        }
        catch (Exception e)
        {
            return new Response<GetQuestionDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<List<GetQuestionDto>>> GetQuestions()
    {
        try
        {
            var questions = await context.Questions.ToListAsync();
            if(questions.Count == 0) return new Response<List<GetQuestionDto>>(HttpStatusCode.NotFound,"Questions not found");
            var dtos = questions.Select(x=> new GetQuestionDto()
            {
                Id = x.Id,
                QuestionText = x.QuestionText,
                QuestionType = x.QuestionType,
                QuizId = x.QuizId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
            }).ToList();
            return new Response<List<GetQuestionDto>>(dtos);
        }
        catch (Exception e)
        {
            return new Response<List<GetQuestionDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}