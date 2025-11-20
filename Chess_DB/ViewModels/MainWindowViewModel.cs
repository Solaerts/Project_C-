using Chess_DB.Services;

namespace Chess_DB.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public PlayersViewModel Players { get; }
    public CompetitionsViewModel Competitions { get; }
    public GamesViewModel Games { get; }

    public MainWindowViewModel()
    {
        var store = new JsonDataStore();

        Players = new PlayersViewModel(store);
        Competitions = new CompetitionsViewModel(store);
        Games = new GamesViewModel(store);
    }
}
