using Cardex.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cardex.DAL;

public class CardexDbContext(DbContextOptions<CardexDbContext> options) : DbContext(options)
{
    public DbSet<CategoryEntity> Categories => Set<CategoryEntity>();
    public DbSet<DeckEntity> Decks => Set<DeckEntity>();
    public DbSet<CardEntity> Cards => Set<CardEntity>();
    public DbSet<StudySessionEntity> StudySessions => Set<StudySessionEntity>();
    public DbSet<CardResultEntity> CardResults => Set<CardResultEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CategoryEntity>(entity =>
        {
            entity.HasKey(category => category.Id);

            entity.Property(category => category.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(category => category.Color)
                .IsRequired()
                .HasMaxLength(32);

            entity.HasMany(category => category.Decks)
                .WithOne(deck => deck.Category)
                .HasForeignKey(deck => deck.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<DeckEntity>(entity =>
        {
            entity.HasKey(deck => deck.Id);

            entity.Property(deck => deck.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.HasMany(deck => deck.Cards)
                .WithOne(card => card.Deck)
                .HasForeignKey(card => card.DeckId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(deck => deck.StudySessions)
                .WithOne(session => session.Deck)
                .HasForeignKey(session => session.DeckId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CardEntity>(entity =>
        {
            entity.HasKey(card => card.Id);

            entity.Property(card => card.Front)
                .IsRequired()
                .HasMaxLength(4000);

            entity.Property(card => card.Back)
                .IsRequired()
                .HasMaxLength(4000);

            entity.HasMany(card => card.CardResults)
                .WithOne(result => result.Card)
                .HasForeignKey(result => result.CardId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<StudySessionEntity>(entity =>
        {
            entity.HasKey(session => session.Id);

            entity.Property(session => session.FinishedAt)
                .IsRequired(false);

            entity.HasMany(session => session.CardResults)
                .WithOne(result => result.Session)
                .HasForeignKey(result => result.SessionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CardResultEntity>(entity =>
        {
            entity.HasKey(result => result.Id);

            entity.Property(result => result.Rating)
                .IsRequired()
                .HasMaxLength(32)
                .HasConversion<string>();
        });

        modelBuilder.SeedTestData();
    }
}
