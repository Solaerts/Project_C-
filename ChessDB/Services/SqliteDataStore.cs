using ChessDB.Data;
using ChessDB.Models;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ChessDB.Services
{
    public class SqliteDataStore : IDataStore
    {
        private readonly ChessDbContext _context;

        public SqliteDataStore()
        {
            _context = new ChessDbContext();
            // Ensures the DB is created at startup
            _context.Database.EnsureCreated(); 
        }

        public List<Player> Players => _context.Players.ToList();
        
        // We include Games to load the related data
        public List<Competition> Competitions => _context.Competitions.Include(c => c.Games).ToList();
        
        public List<Game> Games => _context.Games
            .Include(g => g.WhitePlayer)
            .Include(g => g.BlackPlayer)
            .ToList();

        public void AddPlayer(Player player)
        {
            _context.Players.Add(player);
            Save();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void AddCompetition(Competition competition)
        {
            _context.Competitions.Add(competition);
            _context.SaveChanges();
        }

        public void AddGame(Game game)
        {
            _context.Games.Add(game);
            _context.SaveChanges();
        }

        // Cette méthode est cruciale pour l'ELO
        public void UpdatePlayer(Player player)
        {
            try 
            {
                // 1. On force EF Core à reconnaître cet objet
                _context.Players.Attach(player);
                
                // 2. On lui dit explicitement "Cet objet a été modifié"
                _context.Entry(player).State = EntityState.Modified;
                
                // 3. On sauvegarde et on affiche une preuve dans la console
                _context.SaveChanges();
                Console.WriteLine($"[SUCCÈS] ELO sauvegardé pour {player.LastName} : {player.Elo}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERREUR] Impossible de sauvegarder l'ELO : {ex.Message}");
            }
        }
    }
}