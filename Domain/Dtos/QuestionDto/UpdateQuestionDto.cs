using Domain.Enums;

namespace Domain.Dtos.QuestionDto;

public class UpdateQuestionDto
{
    public int Id { get; set; }
    public string QuestionText { get; set; }
    public QuestionType QuestionType { get; set; }
}