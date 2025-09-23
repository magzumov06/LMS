using System.Net;
using AutoMapper;
using Domain.Dtos.ProgresDto;
using Domain.Entities;
using Domain.Responces;
using Infrastructure.Data.DataContext;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class ProgresService(DataContext context,
    IMapper mapper) : IProgresService 
{
    public async Task<Response<string>> CreateProgres(CreateProgresDto dto)
    {
        try
        {
            var progres = mapper.Map<Progres>(dto);
            progres.CreatedAt = DateTime.UtcNow;
            progres.UpdatedAt = DateTime.UtcNow;
            await context.Progresses.AddAsync(progres);
            await context.SaveChangesAsync();
            var created = await context.Enrollments.AsNoTracking().FirstAsync(x => x.Id == progres.Id);
            return new Response<string>(mapper.Map<string>(created));
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> UpdateProgres(UpdateProgresDto dto)
    {
        try
        {
            var progres = await context.Progresses.FirstOrDefaultAsync(x=>x.Id == dto.Id);
            if(progres == null) return new Response<string>(HttpStatusCode.NotFound,"Progres not found");
            progres.UpdatedAt = DateTime.UtcNow;
            mapper.Map(dto, progres);
            await context.SaveChangesAsync();
            var update = await context.Progresses.AsNoTracking().FirstOrDefaultAsync(x => x.Id == dto.Id);
            return new Response<string>(mapper.Map<string>(update));
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> DeleteProgres(int id)
    {
        try
        {
            var deleteProgres = await context.Progresses.FirstOrDefaultAsync(x => x.Id == id);
            if(deleteProgres == null) return new Response<string>(HttpStatusCode.NotFound,"Progres not found");
            context.Progresses.Remove(deleteProgres);
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Response<string>(HttpStatusCode.OK,"Progres deleted successfully")
                : new Response<string>(HttpStatusCode.BadRequest,"Progres not deleted");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<GetProgresDto>> GetProgresById(int id)
    {
        try
        {
            var progres = await context.Progresses.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if(progres == null) return new Response<GetProgresDto>(HttpStatusCode.NotFound,"Progress not found");
            return new Response<GetProgresDto>(mapper.Map<GetProgresDto>(progres));
        }
        catch (Exception e)
        {
            return new Response<GetProgresDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<List<GetProgresDto>>> GetAllProgress()
    {
        try
        {
            var progress = await context.Progresses.ToListAsync();
            if(progress.Count == 0) return new Response<List<GetProgresDto>>(HttpStatusCode.NotFound,"Progres not found");
            var dtos = progress.Select(x=> new GetProgresDto()
            {
                Id = x.Id,
                IsCompleted = x.IsCompleted,
                CompletedAt = x.CompletedAt,
                LessonId = x.LessonId,
                EnrollmentId = x.EnrollmentId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
            }).ToList();
            return new Response<List<GetProgresDto>>(dtos);
        }
        catch (Exception e)
        {
            return new Response<List<GetProgresDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}