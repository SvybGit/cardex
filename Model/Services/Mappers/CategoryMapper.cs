using Cardex.DAL.Entities;
using Cardex.Model.Dtos;

namespace Cardex.Model.Services.Mappers;

public static class CategoryMapper
{
    public static CategoryDto ToDto(CategoryEntity database)
    {
        return new CategoryDto
        {
            Id = database.Id,
            Name = database.Name
        };
    }

    public static CategoryEntity ToDatabase(CategoryDto dto)
    {
        return new CategoryEntity
        {
            Id = dto.Id,
            Name = dto.Name
        };
    }
}
