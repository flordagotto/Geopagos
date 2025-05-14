using Common.Enums;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
