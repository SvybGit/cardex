using Cardex.DAL.Entities;
using Cardex.DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace Cardex.DAL;

public static class CardexSeedData
{
    public static void SeedTestData(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CategoryEntity>().HasData(new CategoryEntity
        {
            Id = 1,
            Name = "Programming",
            Color = "#2563EB"
        });

        modelBuilder.Entity<DeckEntity>().HasData(new DeckEntity
        {
            Id = 1,
            Name = "C# Basics",
            CategoryId = 1,
            CreatedAt = new DateTime(2026, 5, 6, 12, 0, 0, DateTimeKind.Utc)
        });

        modelBuilder.Entity<CardEntity>().HasData(
            new CardEntity
            {
                Id = 1,
                DeckId = 1,
                Front = "What keyword declares a class in C#?",
                Back = "class",
                CreatedAt = new DateTime(2026, 5, 6, 12, 5, 0, DateTimeKind.Utc)
            },
            new CardEntity
            {
                Id = 2,
                DeckId = 1,
                Front = "What type stores true or false values in C#?",
                Back = "bool",
                CreatedAt = new DateTime(2026, 5, 6, 12, 10, 0, DateTimeKind.Utc)
            });

        modelBuilder.Entity<StudySessionEntity>().HasData(new StudySessionEntity
        {
            Id = 1,
            DeckId = 1,
            StartedAt = new DateTime(2026, 5, 6, 13, 0, 0, DateTimeKind.Utc),
            FinishedAt = new DateTime(2026, 5, 6, 13, 15, 0, DateTimeKind.Utc),
            TotalCards = 2
        });

        modelBuilder.Entity<CardResultEntity>().HasData(
            new CardResultEntity
            {
                Id = 1,
                SessionId = 1,
                CardId = 1,
                Rating = CardRating.Easy
            },
            new CardResultEntity
            {
                Id = 2,
                SessionId = 1,
                CardId = 2,
                Rating = CardRating.Hard
            });
    }
}
