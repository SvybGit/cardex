using Cardex.DAL;
using Cardex.Model.Dtos;
using Cardex.Model.Services.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Cardex.Model.Services;

public class DeckService(CardexDbContext dbContext)
{
    public async Task<List<DeckDto>> GetAllDecksAsync()
    {
        var decks = await dbContext.Decks
            .AsNoTracking()
            .Include(deck => deck.Category)
            .ToListAsync();

        return [.. decks.Select(DeckMapper.ToDto)];
    }

    public async Task<DeckDto?> GetDeckByIdAsync(int id)
    {
        var deck = await dbContext.Decks
            .AsNoTracking()
            .Include(deck => deck.Category)
            .FirstOrDefaultAsync(deck => deck.Id == id);

        return deck is null ? null : DeckMapper.ToDto(deck);
    }

    public async Task<DeckDto> CreateDeckAsync(DeckDto deck)
    {
        var categoryId = deck.Category?.Id;
        if (categoryId.HasValue && !await dbContext.Categories.AnyAsync(category => category.Id == categoryId.Value))
        {
            throw new ArgumentException("Category does not exist.", nameof(deck));
        }

        var entity = DeckMapper.ToDatabase(deck);
        entity.Id = 0;
        entity.CreatedAt = deck.CreatedAt == default ? DateTime.UtcNow : deck.CreatedAt;

        dbContext.Decks.Add(entity);
        await dbContext.SaveChangesAsync();

        return DeckMapper.ToDto(entity);
    }

    public async Task<DeckDto> UpdateDeckAsync(DeckDto deck)
    {
        var categoryId = deck.Category?.Id;
        var entity = await dbContext.Decks.FindAsync(deck.Id) ?? throw new InvalidOperationException("Deck does not exist.");

        if (categoryId.HasValue && !await dbContext.Categories.AnyAsync(category => category.Id == categoryId.Value))
        {
            throw new ArgumentException("Category does not exist.", nameof(deck));
        }

        entity.Name = deck.Name;
        entity.CategoryId = categoryId;
        entity.CreatedAt = deck.CreatedAt == default ? entity.CreatedAt : deck.CreatedAt;

        await dbContext.SaveChangesAsync();

        return DeckMapper.ToDto(entity);
    }

    public async Task DeleteDeckAsync(int id)
    {
        var deck = await dbContext.Decks.FindAsync(id);
        if (deck is not null)
        {
            dbContext.Decks.Remove(deck);
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task<List<CardDto>> GetCardsForDeckAsync(int deckId)
    {
        return await dbContext.Cards
            .AsNoTracking()
            .Where(card => card.DeckId == deckId)
            .Select(card => CardMapper.ToDto(card))
            .ToListAsync();
    }

    public async Task<List<CardDto>> GetAllCardsAsync()
    {
        return await dbContext.Cards
            .AsNoTracking()
            .Select(card => CardMapper.ToDto(card))
            .ToListAsync();
    }

    public async Task<CardDto?> GetCardByIdAsync(int id)
    {
        var card = await dbContext.Cards
            .AsNoTracking()
            .FirstOrDefaultAsync(card => card.Id == id);

        return card is null ? null : CardMapper.ToDto(card);
    }

    public async Task<CardDto> CreateCardInDeckAsync(int deckId, CardDto card)
    {
        if (!await dbContext.Decks.AnyAsync(deck => deck.Id == deckId))
        {
            throw new ArgumentException("Deck does not exist.", nameof(deckId));
        }

        var entity = CardMapper.ToDatabase(card);
        entity.Id = 0;
        entity.DeckId = deckId;
        entity.CreatedAt = card.CreatedAt == default ? DateTime.UtcNow : card.CreatedAt;

        dbContext.Cards.Add(entity);
        await dbContext.SaveChangesAsync();

        return CardMapper.ToDto(entity);
    }

    public async Task<CardDto> UpdateCardAsync(CardDto card)
    {
        var entity = await dbContext.Cards.FindAsync(card.Id) ?? throw new InvalidOperationException("Card does not exist.");

        if (!await dbContext.Decks.AnyAsync(deck => deck.Id == card.DeckId))
        {
            throw new ArgumentException("Deck does not exist.", nameof(card));
        }

        entity.DeckId = card.DeckId;
        entity.Front = card.Front;
        entity.Back = card.Back;
        entity.CreatedAt = card.CreatedAt == default ? entity.CreatedAt : card.CreatedAt;

        await dbContext.SaveChangesAsync();

        return CardMapper.ToDto(entity);
    }

    public async Task DeleteCardAsync(int id)
    {
        var card = await dbContext.Cards.FindAsync(id);
        if (card is not null)
        {
            dbContext.Cards.Remove(card);
            await dbContext.SaveChangesAsync();
        }
    }
}
