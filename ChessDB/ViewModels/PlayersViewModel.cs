using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ReactiveUI;
using ChessDB.Models;
using ChessDB.Services;
using ChessDB.Utils;

namespace ChessDB.ViewModels
{
    public class PlayersViewModel : ViewModelBase
    {
        private readonly JsonDataStore _store;

        public ObservableCollection<Player> Players { get; }
        private string _newPlayerName = "";
        public string NewPlayerName
        {
            get => _newPlayerName;
            set => this.RaiseAndSetIfChanged(ref _newPlayerName, value);
        }

        public ICommand AddPlayerCommand { get; }

        public PlayersViewModel(JsonDataStore store)
        {
            _store = store;
            Players = new ObservableCollection<Player>(_store.Players);
            AddPlayerCommand = new RelayCommand(_ => AddPlayer(), _ => !string.IsNullOrWhiteSpace(NewPlayerName));
        }

        public void AddPlayer()
        {
            if (string.IsNullOrWhiteSpace(NewPlayerName)) return;
            var p = new Player { FullName = NewPlayerName.Trim() };
            _store.Players.Add(p);
            Players.Add(p);
            NewPlayerName = "";
            _store.Save();
        }

        public Player? GetById(System.Guid id) => Players.FirstOrDefault(p => p.Id == id);
    }
}
