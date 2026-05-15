using Cardex.DAL.Entities;
using Cardex.DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace Cardex.DAL;

public static class CardexSeedData
{
    public static void SeedTestData(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CategoryEntity>().HasData(
            new CategoryEntity
            {
                Id = 1,
                Name = "Programming"
            },
            new CategoryEntity
            {
                Id = 2,
                Name = "Databases"
            },
            new CategoryEntity
            {
                Id = 3,
                Name = "Languages"
            },
            new CategoryEntity
            {
                Id = 4,
                Name = "Computer Science"
            });

        modelBuilder.Entity<DeckEntity>().HasData(
            new DeckEntity
            {
                Id = 1,
                Name = "C# Basics",
                CategoryId = 1,
                CreatedAt = new DateTime(2026, 5, 6, 12, 0, 0, DateTimeKind.Utc)
            },
            new DeckEntity
            {
                Id = 2,
                Name = "Entity Framework Core",
                CategoryId = 1,
                CreatedAt = new DateTime(2026, 5, 6, 12, 30, 0, DateTimeKind.Utc)
            },
            new DeckEntity
            {
                Id = 3,
                Name = "SQL Fundamentals",
                CategoryId = 2,
                CreatedAt = new DateTime(2026, 5, 6, 13, 0, 0, DateTimeKind.Utc)
            },
            new DeckEntity
            {
                Id = 4,
                Name = "Spanish Vocabulary",
                CategoryId = 3,
                CreatedAt = new DateTime(2026, 5, 6, 13, 30, 0, DateTimeKind.Utc)
            },
            new DeckEntity
            {
                Id = 5,
                Name = "Data Structures",
                CategoryId = 4,
                CreatedAt = new DateTime(2026, 5, 6, 14, 0, 0, DateTimeKind.Utc)
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
            },
            new CardEntity
            {
                Id = 3,
                DeckId = 1,
                Front = "Which keyword creates an instance of a class?",
                Back = "new",
                CreatedAt = new DateTime(2026, 5, 6, 12, 15, 0, DateTimeKind.Utc)
            },
            new CardEntity
            {
                Id = 4,
                DeckId = 2,
                Front = "Which EF Core type represents a database session?",
                Back = "DbContext",
                CreatedAt = new DateTime(2026, 5, 6, 12, 35, 0, DateTimeKind.Utc)
            },
            new CardEntity
            {
                Id = 5,
                DeckId = 2,
                Front = "Which method saves pending EF Core changes?",
                Back = "SaveChangesAsync",
                CreatedAt = new DateTime(2026, 5, 6, 12, 40, 0, DateTimeKind.Utc)
            },
            new CardEntity
            {
                Id = 6,
                DeckId = 2,
                Front = "Which EF Core method creates a database when no migrations are used?",
                Back = "EnsureCreated",
                CreatedAt = new DateTime(2026, 5, 6, 12, 45, 0, DateTimeKind.Utc)
            },
            new CardEntity
            {
                Id = 7,
                DeckId = 3,
                Front = "Which SQL clause filters rows?",
                Back = "WHERE",
                CreatedAt = new DateTime(2026, 5, 6, 13, 5, 0, DateTimeKind.Utc)
            },
            new CardEntity
            {
                Id = 8,
                DeckId = 3,
                Front = "Which SQL statement reads data from a table?",
                Back = "SELECT",
                CreatedAt = new DateTime(2026, 5, 6, 13, 10, 0, DateTimeKind.Utc)
            },
            new CardEntity
            {
                Id = 9,
                DeckId = 3,
                Front = "Which key uniquely identifies a row?",
                Back = "Primary key",
                CreatedAt = new DateTime(2026, 5, 6, 13, 15, 0, DateTimeKind.Utc)
            },
            new CardEntity
            {
                Id = 10,
                DeckId = 4,
                Front = "How do you say hello in Spanish?",
                Back = "Hola",
                CreatedAt = new DateTime(2026, 5, 6, 13, 35, 0, DateTimeKind.Utc)
            },
            new CardEntity
            {
                Id = 11,
                DeckId = 4,
                Front = "How do you say thank you in Spanish?",
                Back = "Gracias",
                CreatedAt = new DateTime(2026, 5, 6, 13, 40, 0, DateTimeKind.Utc)
            },
            new CardEntity
            {
                Id = 12,
                DeckId = 4,
                Front = "How do you say goodbye in Spanish?",
                Back = "Adios",
                CreatedAt = new DateTime(2026, 5, 6, 13, 45, 0, DateTimeKind.Utc)
            },
            new CardEntity
            {
                Id = 13,
                DeckId = 5,
                Front = "Which data structure uses FIFO order?",
                Back = "Queue",
                CreatedAt = new DateTime(2026, 5, 6, 14, 5, 0, DateTimeKind.Utc)
            },
            new CardEntity
            {
                Id = 14,
                DeckId = 5,
                Front = "Which data structure uses LIFO order?",
                Back = "Stack",
                CreatedAt = new DateTime(2026, 5, 6, 14, 10, 0, DateTimeKind.Utc)
            },
            new CardEntity
            {
                Id = 15,
                DeckId = 5,
                Front = "Which structure stores nodes with left and right children?",
                Back = "Binary tree",
                CreatedAt = new DateTime(2026, 5, 6, 14, 15, 0, DateTimeKind.Utc)
            });

        modelBuilder.Entity<StudySessionEntity>().HasData(
            new StudySessionEntity
            {
                Id = 1,
                DeckId = 1,
                StartedAt = new DateTime(2026, 5, 6, 15, 0, 0, DateTimeKind.Utc),
                FinishedAt = new DateTime(2026, 5, 6, 15, 15, 0, DateTimeKind.Utc),
                TotalCards = 3,
                CurrentCardIndex = 3
            },
            new StudySessionEntity
            {
                Id = 2,
                DeckId = 2,
                StartedAt = new DateTime(2026, 5, 6, 16, 0, 0, DateTimeKind.Utc),
                FinishedAt = new DateTime(2026, 5, 6, 16, 20, 0, DateTimeKind.Utc),
                TotalCards = 3,
                CurrentCardIndex = 3
            },
            new StudySessionEntity
            {
                Id = 3,
                DeckId = 3,
                StartedAt = new DateTime(2026, 5, 7, 9, 0, 0, DateTimeKind.Utc),
                FinishedAt = new DateTime(2026, 5, 7, 9, 12, 0, DateTimeKind.Utc),
                TotalCards = 3,
                CurrentCardIndex = 3
            },
            new StudySessionEntity
            {
                Id = 4,
                DeckId = 5,
                StartedAt = new DateTime(2026, 5, 7, 10, 0, 0, DateTimeKind.Utc),
                FinishedAt = null,
                TotalCards = 3,
                CurrentCardIndex = 2
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
            },
            new CardResultEntity
            {
                Id = 3,
                SessionId = 1,
                CardId = 3,
                Rating = CardRating.Easy
            },
            new CardResultEntity
            {
                Id = 4,
                SessionId = 2,
                CardId = 4,
                Rating = CardRating.Hard
            },
            new CardResultEntity
            {
                Id = 5,
                SessionId = 2,
                CardId = 5,
                Rating = CardRating.Easy
            },
            new CardResultEntity
            {
                Id = 6,
                SessionId = 2,
                CardId = 6,
                Rating = CardRating.Again
            },
            new CardResultEntity
            {
                Id = 7,
                SessionId = 3,
                CardId = 7,
                Rating = CardRating.Easy
            },
            new CardResultEntity
            {
                Id = 8,
                SessionId = 3,
                CardId = 8,
                Rating = CardRating.Easy
            },
            new CardResultEntity
            {
                Id = 9,
                SessionId = 3,
                CardId = 9,
                Rating = CardRating.Hard
            },
            new CardResultEntity
            {
                Id = 10,
                SessionId = 4,
                CardId = 13,
                Rating = CardRating.Hard
            },
            new CardResultEntity
            {
                Id = 11,
                SessionId = 4,
                CardId = 14,
                Rating = CardRating.Again
            });
    }
}
