namespace Domain.Dtos.ProgresDto;

public class CreateProgresDto
{
    public bool IsCompleted { get; set; }
    public DateTime CompletedAt { get; set; }
    public int LessonId { get; set; }
    public int EnrollmentId { get; set; }
}