using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ChessDB.Models;
using ChessDB.Utils;

namespace ChessDB.ViewModels
{
    public class GamesViewModel : ViewModelBase
    {
        private readonly IDataStore _store;
        private readonly PlayersViewModel _playersVm;
        private readonly CompetitionsViewModel _competitionsVm;

        public ObservableCollection<Game> Games { get; }

        // Properties bound to the UI ComboBoxes
        public Guid SelectedCompetitionId { get; set; }
        public Guid SelectedWhitePlayer { get; set; }
        public Guid SelectedBlackPlayer { get; set; }
        public string MovesInput { get; set; } = "";
        public string Result { get; set; } = "1-0"; 

        public ICommand AddGameCommand { get; }

        public GamesViewModel(IDataStore store, PlayersViewModel playersVm, CompetitionsViewModel competitionsVm)
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

            // 1. Create the Game Object
            var g = new Game
            {
                CompetitionId = SelectedCompetitionId,
                WhitePlayerId = SelectedWhitePlayer, // Set the Foreign Key
                BlackPlayerId = SelectedBlackPlayer, // Set the Foreign Key
                Moves = MovesInput,
                PlayedAt = DateTime.UtcNow,
                Result = Result
            };

            // 2. Logic for Winner
            if (Result == "1-0") g.WinnerId = SelectedWhitePlayer;
            else if (Result == "0-1") g.WinnerId = SelectedBlackPlayer;
            else g.WinnerId = null;

            // 3. ELO Calculation
            // We fetch the player objects from the other VM
            var white = _playersVm.GetById(SelectedWhitePlayer);
            var black = _playersVm.GetById(SelectedBlackPlayer);

            if (white != null && black != null)
            {
                double resForWhite = Result == "1-0" ? 1.0 : Result == "0-1" ? 0.0 : 0.5;
                
                // Assuming you have an EloCalculator class
                var (newWhite, newBlack) = EloCalculator.UpdateElo(white.Elo, black.Elo, resForWhite);
                
                white.Elo = newWhite;
                black.Elo = newBlack;
                
                // Save the updated ELOs to DB
                _store.UpdatePlayer(white);
                _store.UpdatePlayer(black);
            }

            // 4. Save Game to DB
            _store.AddGame(g);
            Games.Add(g); // Update UI
        }
    }
}