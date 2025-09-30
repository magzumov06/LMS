using Domain.Enums;

namespace Domain.Dtos.Question;

public class CreateQuestionDto
{
    public string QuestionText { get; set; }
    public QuestionType QuestionType { get; set; }
    public int QuizId { get; set; }
}