using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChessDB.Models
{
    public class Player
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; } = "";
        public int Elo { get; set; } = 1200;

        // Relationship: A player plays many games
        public ICollection<Game> GamesAsWhite { get; set; } = new List<Game>();
        public ICollection<Game> GamesAsBlack { get; set; } = new List<Game>();

        public override string ToString() => $"{FullName} ({Elo})";
    }
}