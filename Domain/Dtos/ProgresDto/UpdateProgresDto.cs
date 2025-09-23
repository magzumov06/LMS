namespace Domain.Dtos.ProgresDto;

public class UpdateProgresDto
{
    public int Id { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CompletedAt { get; set; }
}