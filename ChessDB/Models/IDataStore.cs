using System.Collections.Generic;

namespace ChessDB.Models
{
    public interface IDataStore
    {
        List<Player> Players { get; }
        List<Competition> Competitions { get; }
        List<Game> Games { get; }

        void Save();
    }
}
