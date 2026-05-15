using Cardex.DAL;
using Cardex.DAL.Entities;
using Cardex.DAL.Enums;
using Cardex.Model.Dtos;
using Cardex.Model.Services.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Cardex.Model.Services;

public class StudySessionService(CardexDbContext dbContext)
{
    public async Task<StudySessionDto?> GetActiveSessionForDeckAsync(int deckId)
    {
        var session = await dbContext.StudySessions
            .AsNoTracking()
            .Where(session => session.DeckId == deckId && session.FinishedAt == null)
            .OrderByDescending(session => session.StartedAt)
            .FirstOrDefaultAsync();

        return session is null ? null : StudySessionMapper.ToDto(session);
    }

    public async Task<List<int>> GetDeckIdsWithActiveSessionsAsync()
    {
        return await dbContext.StudySessions
            .AsNoTracking()
            .Where(session => session.FinishedAt == null)
            .Select(session => session.DeckId)
            .Distinct()
            .ToListAsync();
    }

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
            TotalCards = totalCards,
            CurrentCardIndex = 0
        };

        dbContext.StudySessions.Add(entity);
        await dbContext.SaveChangesAsync();

        return StudySessionMapper.ToDto(entity);
    }

    public async Task<StudySessionDto> RestartStudySessionAsync(int deckId, DateTime? startedAt = null)
    {
        await DeleteActiveSessionsForDeckAsync(deckId);
        return await StartStudySessionAsync(deckId, startedAt);
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

    public async Task<List<int>> GetRepeatedCardIdsForSessionAsync(int sessionId)
    {
        return await dbContext.CardResults
            .AsNoTracking()
            .Where(result => result.SessionId == sessionId && result.Rating == CardRating.Again)
            .OrderBy(result => result.Id)
            .Select(result => result.CardId)
            .ToListAsync();
    }

    public async Task<CardResultDto> RecordCardResultAsync(int sessionId, int cardId, CardRating rating, int currentCardIndex, int totalCards)
    {
        var session = await dbContext.StudySessions.FindAsync(sessionId) ?? throw new ArgumentException("Study session does not exist.", nameof(sessionId));

        var card = await dbContext.Cards.FindAsync(cardId) ?? throw new ArgumentException("Card does not exist.", nameof(cardId));

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
        session.CurrentCardIndex = currentCardIndex;
        session.TotalCards = totalCards;
        await dbContext.SaveChangesAsync();

        return CardResultMapper.ToDto(entity);
    }

    public async Task<StudySessionDto> FinishStudySessionAsync(int sessionId, DateTime? finishedAt = null)
    {
        var session = await dbContext.StudySessions.FindAsync(sessionId) ?? throw new InvalidOperationException("Study session does not exist.");

        session.FinishedAt = finishedAt ?? DateTime.UtcNow;

        await dbContext.SaveChangesAsync();

        return StudySessionMapper.ToDto(session);
    }

    private async Task DeleteActiveSessionsForDeckAsync(int deckId)
    {
        var activeSessions = await dbContext.StudySessions
            .Where(session => session.DeckId == deckId && session.FinishedAt == null)
            .ToListAsync();

        if (activeSessions.Count == 0)
        {
            return;
        }

        dbContext.StudySessions.RemoveRange(activeSessions);
        await dbContext.SaveChangesAsync();
    }
}
