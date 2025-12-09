using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChessDB.Models
{
    public class Competition
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "";

        // Relationship: A competition has many games
        public ICollection<Game> Games { get; set; } = new List<Game>();
    }
}