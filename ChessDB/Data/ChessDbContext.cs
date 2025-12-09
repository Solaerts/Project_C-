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
    }
}