using System.Collections.Generic;

namespace ChessDB.Models
{
    public interface IDataStore
    {
        // We use IEnumerable or List to read data
        List<Player> Players { get; }
        List<Competition> Competitions { get; }
        List<Game> Games { get; }

        // Methods to modify data
        void AddPlayer(Player player);
        void AddCompetition(Competition competition);
        void AddGame(Game game);
        
        void UpdatePlayer(Player player); // Useful for ELO updates
    }
}