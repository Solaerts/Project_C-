using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ReactiveUI; // 1. Indispensable

namespace ChessDB.Models
{
    // 2. Indispensable
    public class Player : ReactiveObject 
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }

        // 3. Indispensable pour que l'écran bouge
        private int _elo = 1200;
        public int Elo 
        { 
            get => _elo; 
            set => this.RaiseAndSetIfChanged(ref _elo, value); 
        }

        [NotMapped] 
        public string FullName => $"{FirstName} {LastName}";

        public ICollection<Game> GamesAsWhite { get; set; } = new List<Game>();
        public ICollection<Game> GamesAsBlack { get; set; } = new List<Game>();

        public override string ToString() => $"{FullName} ({Elo})";
    }
}