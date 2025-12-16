using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ReactiveUI; // <--- 1. INDISPENSABLE

namespace ChessDB.Models
{
    // 2. C'EST ICI L'ERREUR : Il faut ajouter " : ReactiveObject"
    public class Player : ReactiveObject 
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }

        // La propriété Elo modifiée
        private int _elo = 1200;
        public int Elo 
        { 
            get => _elo; 
            // Cette ligne ne marche QUE si la classe hérite de ReactiveObject
            set => this.RaiseAndSetIfChanged(ref _elo, value); 
        }

        [NotMapped] 
        public string FullName => $"{FirstName} {LastName}";

        public ICollection<Game> GamesAsWhite { get; set; } = new List<Game>();
        public ICollection<Game> GamesAsBlack { get; set; } = new List<Game>();

        public override string ToString() => $"{FullName} ({Elo})";
    }
}