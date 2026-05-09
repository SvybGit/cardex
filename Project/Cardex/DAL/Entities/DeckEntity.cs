namespace Cardex.DAL.Entities;

public class DeckEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? CategoryId { get; set; }
    public DateTime CreatedAt { get; set; }
    public CategoryEntity? Category { get; set; }
    public ICollection<CardEntity> Cards { get; set; } = new List<CardEntity>();
    public ICollection<StudySessionEntity> StudySessions { get; set; } = new List<StudySessionEntity>();
}
