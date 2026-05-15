namespace Cardex.DAL.Entities;

public class StudySessionEntity
{
    public int Id { get; set; }
    public int DeckId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public int TotalCards { get; set; }
    public int CurrentCardIndex { get; set; }

    public DeckEntity Deck { get; set; } = null!;
    public ICollection<CardResultEntity> CardResults { get; set; } = new List<CardResultEntity>();
}
