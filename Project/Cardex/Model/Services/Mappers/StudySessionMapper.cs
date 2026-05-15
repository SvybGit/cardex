using Cardex.DAL.Entities;
using Cardex.Model.Dtos;

namespace Cardex.Model.Services.Mappers;

public static class StudySessionMapper
{
    public static StudySessionDto ToDto(StudySessionEntity database)
    {
        return new StudySessionDto
        {
            Id = database.Id,
            DeckId = database.DeckId,
            StartedAt = database.StartedAt,
            FinishedAt = database.FinishedAt,
            TotalCards = database.TotalCards,
            CurrentCardIndex = database.CurrentCardIndex
        };
    }

    public static StudySessionEntity ToDatabase(StudySessionDto dto)
    {
        return new StudySessionEntity
        {
            Id = dto.Id,
            DeckId = dto.DeckId,
            StartedAt = dto.StartedAt,
            FinishedAt = dto.FinishedAt,
            TotalCards = dto.TotalCards,
            CurrentCardIndex = dto.CurrentCardIndex
        };
    }
}
