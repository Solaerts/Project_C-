using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChessDB.Models
{
    public class Player
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string FirstName { get; set; } = string.Empty; // Prénom
        public string LastName { get; set; } = string.Empty;  // Nom
        public int Age { get; set; }
        public int Elo { get; set; } = 1200; // Valeur par défaut demandée

        // Propriété pratique pour l'affichage (ne sera pas stockée en base, juste calculée)
        [NotMapped] 
        public string FullName => $"{FirstName} {LastName}";

        public ICollection<Game> GamesAsWhite { get; set; } = new List<Game>();
        public ICollection<Game> GamesAsBlack { get; set; } = new List<Game>();

        public override string ToString() => $"{FullName} ({Elo})";
    }
}