using ContestSystem.Entit;
using ContestSystem.Entity;
using Microsoft.EntityFrameworkCore;
using System;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Contest> Contests { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Option> Options { get; set; }
    public DbSet<ContestParticipant> ContestParticipants { get; set; }
    public DbSet<UserAnswer> UserAnswers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Question>()
        .Property(q => q.Type)
        .HasConversion<int>();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Question>()
            .HasOne(q => q.Contest)
            .WithMany(c => c.Questions)
            .HasForeignKey(q => q.ContestId);

        modelBuilder.Entity<Option>()
            .HasOne(o => o.Question)
            .WithMany(q => q.Options)
            .HasForeignKey(o => o.QuestionId);

        modelBuilder.Entity<ContestParticipant>()
            .HasOne(cp => cp.User)
            .WithMany(u => u.ContestParticipants)
            .HasForeignKey(cp => cp.UserId);

        modelBuilder.Entity<ContestParticipant>()
            .HasOne(cp => cp.Contest)
            .WithMany()
            .HasForeignKey(cp => cp.ContestId);
    }
}