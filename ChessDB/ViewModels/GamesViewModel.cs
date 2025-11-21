using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ChessDB.Models;
using ChessDB.Services;
using ChessDB.Utils;

namespace ChessDB.ViewModels
{
    public class GamesViewModel : ViewModelBase
    {
        private readonly JsonDataStore _store;
        private readonly PlayersViewModel _playersVm;
        private readonly CompetitionsViewModel _competitionsVm;

        public ObservableCollection<Game> Games { get; }

        public Guid SelectedCompetitionId { get; set; }
        public Guid SelectedWhitePlayer { get; set; }
        public Guid SelectedBlackPlayer { get; set; }
        public string MovesInput { get; set; } = "";
        public string Result { get; set; } = "1-0"; // "1-0", "0-1", "1/2-1/2"

        public ICommand AddGameCommand { get; }

        public GamesViewModel(JsonDataStore store, PlayersViewModel playersVm, CompetitionsViewModel competitionsVm)
        {
            _store = store;
            _playersVm = playersVm;
            _competitionsVm = competitionsVm;
            Games = new ObservableCollection<Game>(_store.Games);
            AddGameCommand = new RelayCommand(_ => AddGame(), _ => CanAddGame());
        }

        private bool CanAddGame()
        {
            return SelectedCompetitionId != Guid.Empty &&
                   SelectedWhitePlayer != Guid.Empty &&
                   SelectedBlackPlayer != Guid.Empty &&
                   SelectedWhitePlayer != SelectedBlackPlayer;
        }

        public void AddGame()
        {
            if (!CanAddGame()) return;

            var g = new Game
            {
                CompetitionId = SelectedCompetitionId,
                WhitePlayer = SelectedWhitePlayer,
                BlackPlayer = SelectedBlackPlayer,
                Moves = MovesInput,
                PlayedAt = DateTime.UtcNow,
            };

            // Determine winner id based on Result string
            if (Result == "1-0") g.WinnerId = SelectedWhitePlayer;
            else if (Result == "0-1") g.WinnerId = SelectedBlackPlayer;
            else g.WinnerId = null; // draw

            // Update ELO
            var white = _playersVm.GetById(SelectedWhitePlayer);
            var black = _playersVm.GetById(SelectedBlackPlayer);
            if (white != null && black != null)
            {
                double resForWhite = Result == "1-0" ? 1.0 : Result == "0-1" ? 0.0 : 0.5;
                var (newWhite, newBlack) = EloCalculator.UpdateElo(white.Elo, black.Elo, resForWhite);
                white.Elo = newWhite;
                black.Elo = newBlack;
            }

            _store.Games.Add(g);
            Games.Add(g);
            _store.Save();
        }
    }
}
