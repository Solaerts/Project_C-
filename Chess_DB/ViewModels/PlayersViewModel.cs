using Chess_DB.Models;
using Chess_DB.Services;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace Chess_DB.ViewModels;

public class PlayersViewModel : ViewModelBase
{
    private readonly IDataStore _store;

    public ObservableCollection<Player> Players { get; }

    public string NewPlayerName { get; set; } = "";

    public PlayersViewModel(IDataStore store)
    {
        _store = store;
        Players = new ObservableCollection<Player>(store.Players);
    }

    public void AddPlayer()
    {
        if (string.IsNullOrWhiteSpace(NewPlayerName)) return;

        var p = new Player { FullName = NewPlayerName };
        _store.Players.Add(p);
        Players.Add(p);

        NewPlayerName = "";
        _store.Save();
    }
}
