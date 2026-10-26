namespace Cardex.Model.Dtos;

public class CardexExportDto
{
    public List<CategoryDto> Categories { get; set; } = [];
    public List<DeckDto> Decks { get; set; } = [];
    public List<CardDto> Cards { get; set; } = [];
}
