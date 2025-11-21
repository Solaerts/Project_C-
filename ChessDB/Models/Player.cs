using System;

namespace ChessDB.Models
{
    public class Player
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; } = "";
        public int Elo { get; set; } = 1200;

        // Convenience
        public override string ToString() => $"{FullName} ({Elo})";
    }
}
