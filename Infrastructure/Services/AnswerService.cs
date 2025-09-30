using System.Net;
using AutoMapper;
using Domain.Dtos.AnswerDto;
using Domain.Entities;
using Domain.Responces;
using Infrastructure.Data.DataContext;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Services;

public class AnswerService(DataContext context,
    IMapper mapper) : IAnswerService
{
    public async Task<Response<string>> CreateAnswer(CreateAnswerDto dto)
    {
        try
        {
            Log.Information("Creating a new answer");
            var answer = mapper.Map<Answer>(dto);
            answer.CreatedAt = DateTime.UtcNow;
            answer.UpdatedAt = DateTime.UtcNow;
            await context.Answers.AddAsync(answer);
            await context.SaveChangesAsync();
            var created = await context.Answers.AsNoTracking().FirstOrDefaultAsync(a => a.Id == answer.Id);
            return new Response<string>(mapper.Map<string>(created));
        }
        catch (Exception e)
        {
            Log.Error("Error in CreateAnswer");
            return new Response<string>(HttpStatusCode.InternalServerError,e.Message);
        }
    }

    public async Task<Response<string>> UpdateAnswer(UpdateAnswerDto dto)
    {
        try
        {
            Log.Information("Updating a new answer");
            var updated = await context.Answers.FirstOrDefaultAsync(a => a.Id == dto.Id);
            if (updated == null) return new Response<string>(HttpStatusCode.NotFound, "Answer not found");
            updated.UpdatedAt = DateTime.UtcNow;
            mapper.Map(dto, updated);
            await context.SaveChangesAsync();
            var updatedDto = await context.Answers.AsNoTracking().FirstOrDefaultAsync(a => a.Id == dto.Id);
            return new Response<string>(mapper.Map<string>(updatedDto));
        }
        catch (Exception e)
        {
            Log.Error("Error in UpdateAnswer");
            return new Response<string>(HttpStatusCode.InternalServerError,e.Message);
        }
    }

    public async Task<Response<string>> DeleteAnswer(int answerId)
    {
        try
        {
            Log.Information("DeleteAnswer ");
            var deleteAnswer = await context.Answers.FirstOrDefaultAsync(x => x.Id == answerId);
            if (deleteAnswer == null) return new Response<string>(HttpStatusCode.NotFound, "Answer not found");
            context.Answers.Remove(deleteAnswer);
            var res = await context.SaveChangesAsync();
            if (res > 0)
            {
                Log.Information("Answer deleted successfully");
            }
            else
            {
                Log.Fatal("Answer not deleted");
            }
            return res > 0
                ? new Response<string>(HttpStatusCode.OK,"Answer deleted")
                : new Response<string>(HttpStatusCode.BadRequest,"Answer not deleted");
        }
        catch (Exception e)
        {
            Log.Error("Error in DeleteAnswer");
            return new Response<string>(HttpStatusCode.InternalServerError,e.Message);
        }
    }

    public async Task<Response<GetAnswerDto>> GetAnswerById(int answerId)
    {
        try
        {
            Log.Information("GetAnswerById called");
            var answer = await context.Answers.AsNoTracking().FirstOrDefaultAsync(a => a.Id == answerId);
            if (answer == null) return new Response<GetAnswerDto>(HttpStatusCode.NotFound, "Answer not found");
            return new Response<GetAnswerDto>(mapper.Map<GetAnswerDto>(answer));
        }
        catch (Exception e)
        {
            Log.Error("Error in GetAnswerById");
            return new Response<GetAnswerDto>(HttpStatusCode.InternalServerError,e.Message);
        }
    }

    public async Task<Response<List<GetAnswerDto>>> GetAnswers()
    {
        try
        {
            Log.Information("Get Answers");
            var getAnswers = await context.Answers.ToListAsync();
            if(getAnswers.Count == 0) return new Response<List<GetAnswerDto>>(HttpStatusCode.NotFound,"Answer not found");
            var dtos = getAnswers.Select(x => new GetAnswerDto()
            {
                Id = x.Id,
                AnswerText = x.AnswerText,
                IsCorrect = x.IsCorrect,
                QuestionId = x.QuestionId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            }).ToList();
            return new Response<List<GetAnswerDto>>(dtos);
        }
        catch (Exception e)
        {
            Log.Error("Error in GetAnswers");
            return new Response<List<GetAnswerDto>>(HttpStatusCode.InternalServerError,e.Message);
        }
    }
}