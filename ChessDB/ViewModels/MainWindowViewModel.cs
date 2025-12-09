using ChessDB.Models;
using ChessDB.Services;

namespace ChessDB.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public PlayersViewModel PlayersVM { get; }
        public CompetitionsViewModel CompetitionsVM { get; }
        public GamesViewModel GamesVM { get; }

        // We inject the Interface here (Dependency Injection)
        public MainWindowViewModel(IDataStore dataStore)
        {
            PlayersVM = new PlayersViewModel(dataStore);
            CompetitionsVM = new CompetitionsViewModel(dataStore);
            GamesVM = new GamesViewModel(dataStore, PlayersVM, CompetitionsVM);
        }
    }
}