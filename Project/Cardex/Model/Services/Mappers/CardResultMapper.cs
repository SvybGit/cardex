using Cardex.DAL.Entities;
using Cardex.Model.Dtos;

namespace Cardex.Model.Services.Mappers;

public static class CardResultMapper
{
    public static CardResultDto ToDto(CardResultEntity database)
    {
        return new CardResultDto
        {
            Id = database.Id,
            SessionId = database.SessionId,
            CardId = database.CardId,
            Rating = database.Rating
        };
    }

    public static CardResultEntity ToDatabase(CardResultDto dto)
    {
        return new CardResultEntity
        {
            Id = dto.Id,
            SessionId = dto.SessionId,
            CardId = dto.CardId,
            Rating = dto.Rating
        };
    }
}
