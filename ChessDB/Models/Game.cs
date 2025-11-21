using System;

namespace ChessDB.Models
{
    public class Game
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid WhitePlayer { get; set; }
        public Guid BlackPlayer { get; set; }
        public Guid CompetitionId { get; set; }
        public string Moves { get; set; } = ""; // simple PGN-like string or SAN moves
        public Guid? WinnerId { get; set; } = null; // null = draw
        public DateTime PlayedAt { get; set; } = DateTime.UtcNow;
    }
}
