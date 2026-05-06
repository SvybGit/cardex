using Cardex.DAL;
using Cardex.DAL.Entities;
using Cardex.DAL.Enums;
using Cardex.Model.Dtos;
using Cardex.Model.Services.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Cardex.Model.Services;

public class StudySessionService(CardexDbContext dbContext)
{
    public async Task<StudySessionDto> StartStudySessionAsync(int deckId, DateTime? startedAt = null)
    {
        if (!await dbContext.Decks.AnyAsync(deck => deck.Id == deckId))
        {
            throw new ArgumentException("Deck does not exist.", nameof(deckId));
        }

        var totalCards = await dbContext.Cards.CountAsync(card => card.DeckId == deckId);
        var entity = new StudySessionEntity
        {
            DeckId = deckId,
            StartedAt = startedAt ?? DateTime.UtcNow,
            TotalCards = totalCards
        };

        dbContext.StudySessions.Add(entity);
        await dbContext.SaveChangesAsync();

        return StudySessionMapper.ToDto(entity);
    }

    public async Task<StudySessionDto?> GetStudySessionByIdAsync(int id)
    {
        var session = await dbContext.StudySessions
            .AsNoTracking()
            .FirstOrDefaultAsync(session => session.Id == id);

        return session is null ? null : StudySessionMapper.ToDto(session);
    }

    public async Task<List<StudySessionDto>> GetSessionsForDeckAsync(int deckId)
    {
        return await dbContext.StudySessions
            .AsNoTracking()
            .Where(session => session.DeckId == deckId)
            .Select(session => StudySessionMapper.ToDto(session))
            .ToListAsync();
    }

    public async Task<CardResultDto> RecordCardResultAsync(int sessionId, int cardId, CardRating rating)
    {
        var session = await dbContext.StudySessions.FindAsync(sessionId);
        if (session is null)
        {
            throw new ArgumentException("Study session does not exist.", nameof(sessionId));
        }

        var card = await dbContext.Cards.FindAsync(cardId);
        if (card is null)
        {
            throw new ArgumentException("Card does not exist.", nameof(cardId));
        }

        if (card.DeckId != session.DeckId)
        {
            throw new InvalidOperationException("Card does not belong to the study session deck.");
        }

        var entity = new CardResultEntity
        {
            SessionId = sessionId,
            CardId = cardId,
            Rating = rating
        };

        dbContext.CardResults.Add(entity);
        await dbContext.SaveChangesAsync();

        return CardResultMapper.ToDto(entity);
    }

    public async Task<StudySessionDto> FinishStudySessionAsync(int sessionId, DateTime? finishedAt = null)
    {
        var session = await dbContext.StudySessions.FindAsync(sessionId);
        if (session is null)
        {
            throw new InvalidOperationException("Study session does not exist.");
        }

        session.FinishedAt = finishedAt ?? DateTime.UtcNow;

        await dbContext.SaveChangesAsync();

        return StudySessionMapper.ToDto(session);
    }
}
