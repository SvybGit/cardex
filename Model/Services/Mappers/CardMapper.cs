using Cardex.DAL.Entities;
using Cardex.Model.Dtos;

namespace Cardex.Model.Services.Mappers;

public static class CardMapper
{
    public static CardDto ToDto(CardEntity database)
    {
        return new CardDto
        {
            Id = database.Id,
            DeckId = database.DeckId,
            Front = database.Front,
            Back = database.Back,
            CreatedAt = database.CreatedAt
        };
    }

    public static CardEntity ToDatabase(CardDto dto)
    {
        return new CardEntity
        {
            Id = dto.Id,
            DeckId = dto.DeckId,
            Front = dto.Front,
            Back = dto.Back,
            CreatedAt = dto.CreatedAt
        };
    }
}
