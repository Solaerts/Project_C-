using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ReactiveUI; // <--- 1. CETTE LIGNE EST OBLIGATOIRE

namespace ChessDB.Models
{
    // 2. L'HÉRITAGE " : ReactiveObject" EST OBLIGATOIRE
    public class Player : ReactiveObject
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }

        // 3. LA PROPRIÉTÉ ELO DOIT ÊTRE ÉCRITE COMME CECI :
        private int _elo = 1200;
        public int Elo 
        { 
            get => _elo; 
            set => this.RaiseAndSetIfChanged(ref _elo, value); // <--- C'EST CA QUI MET À JOUR L'ÉCRAN
        }

        [NotMapped] 
        public string FullName => $"{FirstName} {LastName}";

        public ICollection<Game> GamesAsWhite { get; set; } = new List<Game>();
        public ICollection<Game> GamesAsBlack { get; set; } = new List<Game>();

        public override string ToString() => $"{FullName} ({Elo})";
    }
}