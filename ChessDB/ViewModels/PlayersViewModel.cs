using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ReactiveUI;
using ChessDB.Models;
using ChessDB.Utils;

namespace ChessDB.ViewModels
{
    public class PlayersViewModel : ViewModelBase
    {
        private readonly IDataStore _store;

        public ObservableCollection<Player> Players { get; }
        
        // Nouveaux champs de saisie
        private string _newFirstName = "";
        public string NewFirstName
        {
            get => _newFirstName;
            set => this.RaiseAndSetIfChanged(ref _newFirstName, value);
        }

        private string _newLastName = "";
        public string NewLastName
        {
            get => _newLastName;
            set => this.RaiseAndSetIfChanged(ref _newLastName, value);
        }

        private int _newAge = 18;
        public int NewAge
        {
            get => _newAge;
            set => this.RaiseAndSetIfChanged(ref _newAge, value);
        }

        public ICommand AddPlayerCommand { get; }

        public PlayersViewModel(IDataStore store)
        {
            _store = store;
            Players = new ObservableCollection<Player>(_store.Players);
            
            // La commande n'est active que si Nom et Prénom sont remplis
            AddPlayerCommand = new RelayCommand(_ => AddPlayer(), 
                _ => !string.IsNullOrWhiteSpace(NewFirstName) && !string.IsNullOrWhiteSpace(NewLastName));
        }

        public void AddPlayer()
        {
            if (string.IsNullOrWhiteSpace(NewFirstName) || string.IsNullOrWhiteSpace(NewLastName)) return;

            var p = new Player 
            { 
                FirstName = NewFirstName.Trim(),
                LastName = NewLastName.Trim(),
                Age = NewAge,
                Elo = 1200 // ELO par défaut
            };
            
            _store.AddPlayer(p);
            Players.Add(p);
            
            // Remise à zéro des champs
            NewFirstName = "";
            NewLastName = "";
            NewAge = 18;
        }

        public Player? GetById(System.Guid id) => Players.FirstOrDefault(p => p.Id == id);
    }
}