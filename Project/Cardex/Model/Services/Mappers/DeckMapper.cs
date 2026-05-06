using Cardex.DAL.Entities;
using Cardex.Model.Dtos;

namespace Cardex.Model.Services.Mappers;

public static class DeckMapper
{
    public static DeckDto ToDto(DeckEntity database)
    {
        return new DeckDto
        {
            Id = database.Id,
            Name = database.Name,
            CategoryId = database.CategoryId,
            CreatedAt = database.CreatedAt
        };
    }

    public static DeckEntity ToDatabase(DeckDto dto)
    {
        return new DeckEntity
        {
            Id = dto.Id,
            Name = dto.Name,
            CategoryId = dto.CategoryId,
            CreatedAt = dto.CreatedAt
        };
    }
}
