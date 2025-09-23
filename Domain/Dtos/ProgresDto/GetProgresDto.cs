namespace Domain.Dtos.ProgresDto;

public class GetProgresDto : UpdateProgresDto
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int LessonId { get; set; }
    public int EnrollmentId { get; set; }
}