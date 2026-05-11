namespace Cardex.Model.Dtos;

public class DeckDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public CategoryDto? Category { get; set; }
    public DateTime CreatedAt { get; set; }
}
