using Cardex.Model.Dtos;

namespace ViewModel;

public class DeckItemViewModel(DeckDto deck, int cardCount)
{
    public DeckDto Deck { get; } = deck;
    public int Id => Deck.Id;
    public string Name => Deck.Name;
    public int CardCount { get; } = cardCount;
    public string CategoryName => Deck.Category?.Name ?? string.Empty;
    public bool HasCategory => !string.IsNullOrWhiteSpace(CategoryName);
}
