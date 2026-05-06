namespace Cardex.DAL.Entities;

public class CategoryEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public ICollection<DeckEntity> Decks { get; set; } = new List<DeckEntity>();
}
