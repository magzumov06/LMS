using System.Net;
using AutoMapper;
using Domain.Dtos.CourseDto;
using Domain.Entities;
using Domain.Filter;
using Domain.Responces;
using Infrastructure.Data.DataContext;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Services;

public class CourseService(DataContext context,
    IMapper mapper) : ICourseService
{
    public async Task<Response<string>> CreateCourse(CreateCourseDto dto)
    {
        try
        {
            Log.Information("Creating new Course");
            var entity = mapper.Map<Course>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            await context.Courses.AddAsync(entity);
            await context.SaveChangesAsync();
            var created = await context.Courses.AsNoTracking().FirstAsync(c=>c.Id == entity.Id);
            return new Response<string>(mapper.Map<string>(created));
        }
        catch (Exception e)
        {
            Log.Error("Error in CreateCourse");
            return new Response<string>(HttpStatusCode.InternalServerError,e.Message);
        }
    }

    public async Task<Response<string>> UpdateCourse(UpdateCourseDto dto)
    {
        try
        {
            Log.Information("Updating Course");
            var course = await context.Courses.FirstOrDefaultAsync(x=>x.Id == dto.Id);
            if(course == null) return new Response<string>(HttpStatusCode.NotFound,"Course not found");
            course.UpdatedAt = DateTime.UtcNow;
            mapper.Map(dto, course);
            await context.SaveChangesAsync();
            var updated = await context.Courses.AsNoTracking().FirstAsync(c=>c.Id == dto.Id);
            return new Response<string>(mapper.Map<string>(updated));
        }
        catch (Exception e)
        {
            Log.Error("Error in UpdateCourse");
            return new Response<string>(HttpStatusCode.InternalServerError,e.Message);
        }
    }

    public async Task<Response<string>> DeleteCourse(int id)
    {
        try
        {
            Log.Information("Deleting Course");
            var deletedCourse = await context.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if(deletedCourse == null) return new Response<string>(HttpStatusCode.NotFound,"Course not found");
            deletedCourse.IsDeleted = true;
            var res = await context.SaveChangesAsync();
            if (res > 0)
            {
                Log.Information("Course Deleted");
            }
            else
            {
                Log.Fatal("Course could not be deleted");
            }
            return res > 0
                ? new Response<string>(HttpStatusCode.OK,"Course deleted")
                : new Response<string>(HttpStatusCode.NotFound,"Course not deleted");
        }
        catch (Exception e)
        {
            Log.Error("Error in DeleteCourse");
            return new Response<string>(HttpStatusCode.InternalServerError,e.Message);
        }
    }

    public async Task<Response<GetCourseDto>> GetCourseById(int id)
    {
        try
        {
            Log.Information("Getting Course");
            var course = await context.Courses.AsNoTracking().FirstOrDefaultAsync(c=>c.Id == id);
            if(course == null) return new Response<GetCourseDto>(HttpStatusCode.NotFound,"Course not found");
            return new Response<GetCourseDto>(mapper.Map<GetCourseDto>(course));
        }
        catch (Exception e)
        {
            Log.Error("Error in GetCourseById");
            return new Response<GetCourseDto>(HttpStatusCode.InternalServerError,e.Message);
        }
    }

    public async Task<PaginationResponse<List<GetCourseDto>>> GetCourses(CourseFilter filter)
    {
        try
        {
            Log.Information("Getting Courses");
            var query = context.Courses.AsQueryable();
            if (filter.Id.HasValue)
            {
                query = query.Where(x => x.Id == filter.Id.Value);
            }

            if (!string.IsNullOrEmpty(filter.Title))
            {
                query = query.Where(x => x.Title.Contains(filter.Title));
            }

            if (!string.IsNullOrEmpty(filter.Description))
            {
                query = query.Where(x => x.Description.Contains(filter.Description));
            }

            if (filter.Category.HasValue)
            {
                query = query.Where(x => x.Category == filter.Category.Value);
            }

            if (filter.Price.HasValue)
            {
                query = query.Where(x => x.Price == filter.Price.Value);
            }

            if (filter.IsFree.HasValue)
            {
                query = query.Where(x => x.IsFree == filter.IsFree.Value);
            }

            if (filter.CreatedBy.HasValue)
            {
                query = query.Where(x => x.CreatedBy == filter.CreatedBy.Value);
            }
            query = query.Where(x=> x.IsDeleted == false);
            var total =  await query.CountAsync();
            var skip = (filter.PageNumber - 1) * filter.PageSize;
            var courses = await query.Skip(skip).Take(filter.PageSize).ToListAsync();
            if(courses.Count == 0) return new PaginationResponse<List<GetCourseDto>>(HttpStatusCode.NotFound,"No courses");
            var dtos = courses.Select(x=> new GetCourseDto()
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Category = x.Category,
                Price = x.Price,
                IsFree = x.IsFree,
                CreatedBy = x.CreatedBy,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            }).ToList();
            return new PaginationResponse<List<GetCourseDto>>(dtos, total, filter.PageNumber, filter.PageSize);
            
        }
        catch (Exception e)
        {
            Log.Error("Error in GetCourses");
            return new PaginationResponse<List<GetCourseDto>>(HttpStatusCode.InternalServerError,e.Message);
        }
    }
}