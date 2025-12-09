using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChessDB.Models
{
    public class Game
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // Foreign Key for White Player
        public Guid WhitePlayerId { get; set; }
        [ForeignKey("WhitePlayerId")]
        public Player WhitePlayer { get; set; } = null!;

        // Foreign Key for Black Player
        public Guid BlackPlayerId { get; set; }
        [ForeignKey("BlackPlayerId")]
        public Player BlackPlayer { get; set; } = null!;

        // Link to Competition
        public Guid CompetitionId { get; set; }
        public Competition? Competition { get; set; }

        public string Moves { get; set; } = ""; 
        public Guid? WinnerId { get; set; }
        public DateTime PlayedAt { get; set; } = DateTime.UtcNow;
    }
}