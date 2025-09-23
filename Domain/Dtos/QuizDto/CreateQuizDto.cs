namespace Domain.Dtos.QuizDto;

public class CreateQuizDto
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public int CourseId { get; set; }
}