using Microsoft.EntityFrameworkCore;
using ChessDB.Models;
using System;
using System.IO;

namespace ChessDB.Data
{
    public class ChessDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Competition> Competitions { get; set; }
        public DbSet<Game> Games { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // This creates the .db file in your local app data folder to avoid path issues
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "chess_club.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // On explique la relation pour le joueur blanc
            modelBuilder.Entity<Game>()
                .HasOne(g => g.WhitePlayer)           // Une partie a un Joueur Blanc
                .WithMany(p => p.GamesAsWhite)        // Un joueur a plusieurs parties en tant que Blanc
                .HasForeignKey(g => g.WhitePlayerId); // La clé étrangère est WhitePlayerId

            // On explique la relation pour le joueur noir
            modelBuilder.Entity<Game>()
                .HasOne(g => g.BlackPlayer)           // Une partie a un Joueur Noir
                .WithMany(p => p.GamesAsBlack)        // Un joueur a plusieurs parties en tant que Noir
                .HasForeignKey(g => g.BlackPlayerId); // La clé étrangère est BlackPlayerId
        }
    }
}