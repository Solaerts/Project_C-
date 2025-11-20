using System;
using System.Collections.Generic;

namespace Chess_DB.Models;

public class Game
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid WhitePlayer { get; set; }
    public Guid BlackPlayer { get; set; }
    public Guid CompetitionId { get; set; }
    public string Moves { get; set; } = ""; // Format PGN simplifié
    public Guid WinnerId { get; set; } // 0 = draw
}