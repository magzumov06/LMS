namespace Domain.Dtos.QuestionDto;

public class GetQuestionDto : UpdateQuestionDto
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int QuizId { get; set; }
}