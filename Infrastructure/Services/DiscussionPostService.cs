using System.Net;
using AutoMapper;
using Domain.Dtos.DiscussionPostDto;
using Domain.Entities;
using Domain.Responces;
using Infrastructure.Data.DataContext;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class DiscussionPostService(DataContext context,
    IMapper mapper) : IDiscussionPostService
{
    public async Task<Response<string>> Create(CreateDiscussionDto dto)
    {
        try
        {
            var post = mapper.Map<DiscussionPost>(dto);
            post.CreatedAt = DateTime.UtcNow;
            post.UpdatedAt = DateTime.UtcNow;
            await context.DiscussionPosts.AddAsync(post);
            await context.SaveChangesAsync();
            var created = await context.DiscussionPosts.AsNoTracking().FirstAsync(x => x.Id == post.Id);
            return new Response<string>(mapper.Map<string>(created));
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> Update(UpdateDiscussionDto dto)
    {
        try
        {
            var update = await context.DiscussionPosts.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if (update == null) return new Response<string>(HttpStatusCode.NotFound, "Discussion post not found");
            update.UpdatedAt = DateTime.UtcNow;
            mapper.Map(dto, update);
            await context.SaveChangesAsync();
            var updated = await context.DiscussionPosts.AsNoTracking().FirstAsync(x => x.Id == dto.Id);
            return new Response<string>(mapper.Map<string>(updated));
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> Delete(int id)
    {
        try
        {
            var delete = await context.DiscussionPosts.FirstOrDefaultAsync(x => x.Id == id);
            if (delete == null) return new Response<string>(HttpStatusCode.NotFound,"Discussion post not found");
            context.DiscussionPosts.Remove(delete);
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Response<string>(HttpStatusCode.OK, "Discussion post deleted")
                : new Response<string>(HttpStatusCode.BadRequest, "Discussion post not deleted");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<GetDiscussionDto>> GetById(int id)
    {
        try
        {
            var res = await context.DiscussionPosts.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if(res == null) return new Response<GetDiscussionDto>(HttpStatusCode.NotFound,"Discussion post not found");
            return new Response<GetDiscussionDto>(mapper.Map<GetDiscussionDto>(res));
        }
        catch (Exception e)
        {
            return new Response<GetDiscussionDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<List<GetDiscussionDto>>> Get()
    {
        try
        {
            var discussion =  await context.DiscussionPosts.ToListAsync();
            if(discussion.Count == 0) return new Response<List<GetDiscussionDto>>(HttpStatusCode.NotFound,"Discussion post not found");
            var dtos = discussion.Select(x => new GetDiscussionDto()
            {
                Id = x.Id,
                Content = x.Content,
                CourseId = x.CourseId,
                UserId = x.UserId,
                ParentId = x.ParentId,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            }).ToList();
            return new Response<List<GetDiscussionDto>>(dtos);
        }
        catch (Exception e)
        {
            return new Response<List<GetDiscussionDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}