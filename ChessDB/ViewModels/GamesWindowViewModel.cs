using System;
using System.Collections.Generic; // <--- C'EST CETTE LIGNE QUI MANQUAIT
using System.Collections.ObjectModel;
using System.Reactive; 
using System.Reactive.Linq;
using ReactiveUI;
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

        public ObservableCollection<Player> AvailablePlayers => _playersVm.Players;
        public ObservableCollection<Competition> AvailableCompetitions => _competitionsVm.Competitions;

        // Liste des résultats possibles pour la ComboBox
        public List<string> PossibleResults { get; } = new List<string> 
        { 
            "1-0", 
            "0-1", 
            "1/2-1/2" 
        };

        // --- Propriétés Réactives ---
        private Guid _selectedCompetitionId;
        public Guid SelectedCompetitionId
        {
            get => _selectedCompetitionId;
            set => this.RaiseAndSetIfChanged(ref _selectedCompetitionId, value);
        }

        private Guid _selectedWhitePlayer;
        public Guid SelectedWhitePlayer
        {
            get => _selectedWhitePlayer;
            set => this.RaiseAndSetIfChanged(ref _selectedWhitePlayer, value);
        }

        private Guid _selectedBlackPlayer;
        public Guid SelectedBlackPlayer
        {
            get => _selectedBlackPlayer;
            set => this.RaiseAndSetIfChanged(ref _selectedBlackPlayer, value);
        }

        private string _movesInput = "";
        public string MovesInput
        {
            get => _movesInput;
            set => this.RaiseAndSetIfChanged(ref _movesInput, value);
        }

        private string _result = "1-0";
        public string Result
        {
            get => _result;
            set => this.RaiseAndSetIfChanged(ref _result, value);
        }

        public ReactiveCommand<Unit, Unit> AddGameCommand { get; }

        public GamesViewModel(IDataStore store, PlayersViewModel playersVm, CompetitionsViewModel competitionsVm)
        {
            _store = store;
            _playersVm = playersVm;
            _competitionsVm = competitionsVm;
            
            Games = new ObservableCollection<Game>(_store.Games);

            // Condition d'activation du bouton
            var canAddGame = this.WhenAnyValue(
                x => x.SelectedCompetitionId,
                x => x.SelectedWhitePlayer,
                x => x.SelectedBlackPlayer,
                (compId, whiteId, blackId) => 
                    compId != Guid.Empty &&
                    whiteId != Guid.Empty &&
                    blackId != Guid.Empty &&
                    whiteId != blackId
            )
            .ObserveOn(RxApp.MainThreadScheduler);

            AddGameCommand = ReactiveCommand.Create(AddGame, canAddGame);
        }

        public void AddGame()
        {
            var g = new Game
            {
                CompetitionId = SelectedCompetitionId,
                WhitePlayerId = SelectedWhitePlayer,
                BlackPlayerId = SelectedBlackPlayer,
                Moves = MovesInput,
                PlayedAt = DateTime.UtcNow,
                Result = Result
            };

            // Gestion du vainqueur
            if (Result == "1-0") g.WinnerId = SelectedWhitePlayer;
            else if (Result == "0-1") g.WinnerId = SelectedBlackPlayer;
            else g.WinnerId = null;

            // Calcul ELO
            var white = _playersVm.GetById(SelectedWhitePlayer);
            var black = _playersVm.GetById(SelectedBlackPlayer);

            if (white != null && black != null)
            {
                // Note : Assure-toi que Result contient bien "1-0", "0-1" ou "1/2-1/2"
                double resForWhite = Result == "1-0" ? 1.0 : Result == "0-1" ? 0.0 : 0.5;
                
                var (newWhite, newBlack) = EloCalculator.UpdateElo(white.Elo, black.Elo, resForWhite);
                
                // Mise à jour des objets (l'écran se mettra à jour grâce à ReactiveObject)
                white.Elo = newWhite;
                black.Elo = newBlack;
                
                // Sauvegarde forcée en base de données
                _store.UpdatePlayer(white);
                _store.UpdatePlayer(black);
            }

            _store.AddGame(g);
            Games.Add(g);

            MovesInput = "";
        }
    }
}