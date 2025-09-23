namespace Domain.Dtos.QuizDto;

public class GetQuizDto : UpdateQuizDto
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int CourseId { get; set; }
}