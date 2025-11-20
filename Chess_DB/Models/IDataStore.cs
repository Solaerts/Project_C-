using System;
using System.Collections.Generic;

using Chess_DB.Models;

namespace Chess_DB.Models;

public interface IDataStore
{
    List<Player> Players { get; }
    List<Competition> Competitions { get; }
    List<Game> Games { get; }

    void Save();
}
