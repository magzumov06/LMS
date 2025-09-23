namespace Domain.Dtos.QuizDto;

public class UpdateQuizDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
}