namespace Cardex.Model.Dtos;

public class StudySessionDto
{
    public int Id { get; set; }
    public int DeckId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public int TotalCards { get; set; }
}
