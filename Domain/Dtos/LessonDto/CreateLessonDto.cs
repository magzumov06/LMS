using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.Dtos.LessonDto;

public class CreateLessonDto
{
    [Required]
    public required string Title { get; set; }
    public string? Content { get; set; }
    public IFormFile? Video { get; set; }
    public int OrderIndex { get; set; }
    public int CourseId { get; set; }
}