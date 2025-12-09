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
        
        private string _newPlayerName = "";
        public string NewPlayerName
        {
            get => _newPlayerName;
            set => this.RaiseAndSetIfChanged(ref _newPlayerName, value);
        }

        public ICommand AddPlayerCommand { get; }

        public PlayersViewModel(IDataStore store)
        {
            _store = store;
            // Load data from DB into the ObservableCollection for the UI
            Players = new ObservableCollection<Player>(_store.Players);
            
            AddPlayerCommand = new RelayCommand(_ => AddPlayer(), _ => !string.IsNullOrWhiteSpace(NewPlayerName));
        }

        public void AddPlayer()
        {
            if (string.IsNullOrWhiteSpace(NewPlayerName)) return;

            var p = new Player { FullName = NewPlayerName.Trim(), Elo = 1200 };
            
            // 1. Add to Database
            _store.AddPlayer(p);

            // 2. Add to UI List (so we see it immediately without restarting)
            Players.Add(p);
            
            NewPlayerName = "";
        }

        public Player? GetById(System.Guid id) => Players.FirstOrDefault(p => p.Id == id);
    }
}