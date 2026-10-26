namespace Cardex.Model.Dtos;

public class CardDto
{
    public int Id { get; set; }
    public int DeckId { get; set; }
    public string Front { get; set; } = string.Empty;
    public string Back { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
