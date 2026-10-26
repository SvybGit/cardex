using Cardex.DAL.Enums;

namespace Cardex.DAL.Entities;

public class CardResultEntity
{
    public int Id { get; set; }
    public int SessionId { get; set; }
    public int CardId { get; set; }
    public CardRating Rating { get; set; }
    public StudySessionEntity Session { get; set; } = null!;
    public CardEntity Card { get; set; } = null!;
}
