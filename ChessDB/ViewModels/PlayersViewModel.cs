using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive; 
using ReactiveUI;      
using ChessDB.Models;
using ChessDB.Utils;

namespace ChessDB.ViewModels
{
    public class PlayersViewModel : ViewModelBase
    {
        private readonly IDataStore _store;
        public ObservableCollection<Player> Players { get; }
        
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

        public ReactiveCommand<Unit, Unit> AddPlayerCommand { get; }

        public PlayersViewModel(IDataStore store)
        {
            _store = store;
            Players = new ObservableCollection<Player>(_store.Players);

            // On définit quand la commande peut s'exécuter.
            // WhenAnyValue observe les changements de NewFirstName et NewLastName en temps réel.
            var canAddPlayer = this.WhenAnyValue(
            x => x.NewFirstName,
            x => x.NewLastName,
            (firstName, lastName) => 
                !string.IsNullOrWhiteSpace(firstName) && 
                !string.IsNullOrWhiteSpace(lastName)
            )
            .ObserveOn(RxApp.MainThreadScheduler);

            AddPlayerCommand = ReactiveCommand.Create(AddPlayer, canAddPlayer);
        }


        public void AddPlayer()
        {
            var p = new Player 
            { 
                FirstName = NewFirstName.Trim(),
                LastName = NewLastName.Trim(),
                Age = NewAge,
                Elo = 1200 
            };
            
            _store.AddPlayer(p);
            Players.Add(p);
            
            NewFirstName = "";
            NewLastName = "";
            NewAge = 18;
        }

        public Player? GetById(System.Guid id) => Players.FirstOrDefault(p => p.Id == id);
    }
}