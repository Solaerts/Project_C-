using ChessDB.Services;

namespace ChessDB.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public PlayersViewModel PlayersVM { get; }
        public CompetitionsViewModel CompetitionsVM { get; }
        public GamesViewModel GamesVM { get; }

        public MainWindowViewModel()
        {
            var store = new JsonDataStore();
            PlayersVM = new PlayersViewModel(store);
            CompetitionsVM = new CompetitionsViewModel(store);
            GamesVM = new GamesViewModel(store, PlayersVM, CompetitionsVM);
        }
    }
}
