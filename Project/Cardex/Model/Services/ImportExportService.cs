using System.Text.Json;
using Cardex.Model.Dtos;

namespace Cardex.Model.Services;

public class ImportExportService(
    CategoryService categoryService,
    DeckService deckService)
{
    public async Task<string> ExportAsync()
    {
        var data = new CardexExportDto
        {
            Categories = await categoryService.GetAllCategoriesAsync(),
            Decks = await deckService.GetAllDecksAsync(),
            Cards = await deckService.GetAllCardsAsync()
        };

        return JsonSerializer.Serialize(data);
    }

    public async Task ImportAsync(string path)
    {
        var data = ParseExport(await File.ReadAllTextAsync(path));

        var categories = await ImportCategoriesAsync(data.Categories);
        var decks = await ImportDecksAsync(data.Decks, categories);
        await ImportCardsAsync(data.Cards, decks);
    }

    private async Task<Dictionary<int, CategoryDto>> ImportCategoriesAsync(List<CategoryDto> importedCategories)
    {
        var categories = new Dictionary<int, CategoryDto>();
        foreach (var category in importedCategories)
        {
            categories[category.Id] = await categoryService.CreateCategoryAsync(new CategoryDto
            {
                Name = category.Name
            });
        }

        return categories;
    }

    private async Task<Dictionary<int, DeckDto>> ImportDecksAsync(
        List<DeckDto> importedDecks,
        Dictionary<int, CategoryDto> categories)
    {
        var decks = new Dictionary<int, DeckDto>();
        foreach (var deck in importedDecks)
        {
            var category = deck.Category is null
                ? null
                : FindImported(categories, deck.Category.Id, $"Deck '{deck.Name}' references missing category ID {deck.Category.Id}.");

            decks[deck.Id] = await deckService.CreateDeckAsync(new DeckDto
            {
                Name = deck.Name,
                Category = category,
                CreatedAt = deck.CreatedAt
            });
        }

        return decks;
    }

    private async Task ImportCardsAsync(List<CardDto> importedCards, Dictionary<int, DeckDto> decks)
    {
        foreach (var card in importedCards)
        {
            var deck = FindImported(decks, card.DeckId, $"Card ID {card.Id} references missing deck ID {card.DeckId}.");

            await deckService.CreateCardInDeckAsync(deck.Id, new CardDto
            {
                Front = card.Front,
                Back = card.Back,
                CreatedAt = card.CreatedAt
            });
        }
    }

    private static CardexExportDto ParseExport(string json)
    {
        try
        {
            return JsonSerializer.Deserialize<CardexExportDto>(json)
                ?? throw new InvalidOperationException("Cardex import JSON is invalid.");
        }
        catch (JsonException exception)
        {
            throw new InvalidOperationException("Cardex import JSON is invalid.", exception);
        }
    }

    private static TValue FindImported<TValue>(Dictionary<int, TValue> imported, int oldId, string message)
    {
        return imported.TryGetValue(oldId, out var value)
            ? value
            : throw new InvalidOperationException(message);
    }
}
