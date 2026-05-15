using Cardex.DAL;
using Cardex.Model.Dtos;
using Cardex.Model.Services.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Cardex.Model.Services;

public class CategoryService(CardexDbContext dbContext)
{
    public async Task<List<CategoryDto>> GetAllCategoriesAsync()
    {
        return await dbContext.Categories
            .AsNoTracking()
            .Select(category => CategoryMapper.ToDto(category))
            .ToListAsync();
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
    {
        var category = await dbContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(category => category.Id == id);

        return category is null ? null : CategoryMapper.ToDto(category);
    }

    public async Task<CategoryDto> CreateCategoryAsync(CategoryDto category)
    {
        var entity = CategoryMapper.ToDatabase(category);
        entity.Id = 0;

        dbContext.Categories.Add(entity);
        await dbContext.SaveChangesAsync();

        return CategoryMapper.ToDto(entity);
    }

    public async Task<CategoryDto> UpdateCategoryAsync(CategoryDto category)
    {
        var entity = await dbContext.Categories.FindAsync(category.Id);
        if (entity is null)
        {
            throw new InvalidOperationException("Category does not exist.");
        }

        entity.Name = category.Name;

        await dbContext.SaveChangesAsync();

        return CategoryMapper.ToDto(entity);
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var category = await dbContext.Categories.FindAsync(id);
        if (category is not null && await dbContext.Decks.AnyAsync(deck => deck.CategoryId == id))
        {
            throw new InvalidOperationException("Cannot delete a category that is used by decks.");
        }

        if (category is not null)
        {
            dbContext.Categories.Remove(category);
            await dbContext.SaveChangesAsync();
        }
    }
}
