using Common.Enums;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace DAL.Context
{
    public class GameDbContext : DbContext
    {
        public GameDbContext(DbContextOptions<GameDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Match> Matches { get; set; }

        private void UseSeed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FemalePlayer>().HasData(
                new FemalePlayer
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "Florencia",
                    Skill = 85,
                    Gender = Gender.Female,
                    ReactionTime = 90
                });

            modelBuilder.Entity<MalePlayer>().HasData(
                new FemalePlayer
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "Milagros",
                    Skill = 90,
                    Gender = Gender.Female,
                    ReactionTime = 75
                });

            modelBuilder.Entity<FemalePlayer>().HasData(
                new MalePlayer
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Name = "Juan",
                    Skill = 85,
                    Gender = Gender.Male,
                    Strength = 80,
                    Speed = 95
                });

            modelBuilder.Entity<MalePlayer>().HasData(
                new MalePlayer
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Name = "Joaquin",
                    Skill = 90,
                    Gender = Gender.Male,
                    Strength = 80,
                    Speed = 95
                });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            UseSeed(modelBuilder);

            modelBuilder.Entity<Player>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Player>()
                .HasDiscriminator<Gender>("Gender")
                .HasValue<MalePlayer>(Gender.Male)
                .HasValue<FemalePlayer>(Gender.Female);

            modelBuilder.Entity<Tournament>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<Tournament>()
                .HasOne(t => t.Winner)
                .WithMany()
                .HasForeignKey(t => t.WinnerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PlayersByTournament>()
                .HasKey(pbt => new { pbt.PlayerId, pbt.TournamentId });

            modelBuilder.Entity<PlayersByTournament>()
                .HasOne(pbt => pbt.Tournament)
                .WithMany(t => t.Players)
                .HasForeignKey(pbt => pbt.TournamentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PlayersByTournament>()
                .HasOne(pbt => pbt.Player)
                .WithMany()
                .HasForeignKey(pbt => pbt.PlayerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Match>()
                    .HasKey(m => m.Id);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Player1)
                .WithMany()
                .HasForeignKey(m => m.Player1Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Player2)
                .WithMany()
                .HasForeignKey(m => m.Player2Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Winner)
                .WithMany()
                .HasForeignKey(m => m.WinnerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Tournament)
                .WithMany(t => t.Matches)
                .HasForeignKey(m => m.TournamentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
