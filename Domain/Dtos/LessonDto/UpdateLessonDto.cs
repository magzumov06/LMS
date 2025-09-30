using Microsoft.AspNetCore.Http;

namespace Domain.Dtos.LessonDto;

public class UpdateLessonDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public IFormFile? Video { get; set; }
    public int OrderIndex { get; set; }
}