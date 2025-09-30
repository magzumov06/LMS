namespace Domain.Dtos.LessonDto;

public class GetLessonDto 
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string? VideoUrl { get; set; }
    public string FileUrl { get; set; }
    public int OrderIndex { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int CourseId { get; set; }
}