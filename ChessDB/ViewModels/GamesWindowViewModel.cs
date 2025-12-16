using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Reactive; // Nécessaire pour Unit
using ReactiveUI;      // Nécessaire pour ReactiveCommand et WhenAnyValue
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

        // Listes accessibles pour la Vue
        public ObservableCollection<Player> AvailablePlayers => _playersVm.Players;
        public ObservableCollection<Competition> AvailableCompetitions => _competitionsVm.Competitions;

        // --- PROPRIÉTÉS RÉACTIVES (Changement important !) ---
        
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

        // --- COMMANDE RÉACTIVE ---
        public ReactiveCommand<Unit, Unit> AddGameCommand { get; }

        public GamesViewModel(IDataStore store, PlayersViewModel playersVm, CompetitionsViewModel competitionsVm)
        {
            _store = store;
            _playersVm = playersVm;
            _competitionsVm = competitionsVm;
            
            Games = new ObservableCollection<Game>(_store.Games);

            // --- LA CONDITION MAGIQUE ---
            // On surveille en temps réel les 3 sélections (Compétition, Blanc, Noir)
            var canAddGame = this.WhenAnyValue(
                x => x.SelectedCompetitionId,
                x => x.SelectedWhitePlayer,
                x => x.SelectedBlackPlayer,
                (compId, whiteId, blackId) => 
                    compId != Guid.Empty &&        // Une compétition est choisie
                    whiteId != Guid.Empty &&       // Joueur blanc choisi
                    blackId != Guid.Empty &&       // Joueur noir choisi
                    whiteId != blackId             // Ce n'est pas le même joueur !
            )
            .ObserveOn(RxApp.MainThreadScheduler); // <--- AJOUTE ÇA
            AddGameCommand = ReactiveCommand.Create(AddGame, canAddGame);
        }

        public void AddGame()
        {
            // On crée l'objet Game
            var g = new Game
            {
                CompetitionId = SelectedCompetitionId,
                WhitePlayerId = SelectedWhitePlayer,
                BlackPlayerId = SelectedBlackPlayer,
                Moves = MovesInput,
                PlayedAt = DateTime.UtcNow,
                Result = Result
            };

            // Logique pour déterminer le vainqueur (pour l'objet en mémoire)
            if (Result == "1-0") g.WinnerId = SelectedWhitePlayer;
            else if (Result == "0-1") g.WinnerId = SelectedBlackPlayer;
            else g.WinnerId = null;

            // Calcul ELO (optionnel, si tu as gardé le EloCalculator)
            var white = _playersVm.GetById(SelectedWhitePlayer);
            var black = _playersVm.GetById(SelectedBlackPlayer);

            if (white != null && black != null)
            {
                // Score pour les blancs : 1 si gagne, 0 si perd, 0.5 si nul
                double resForWhite = Result == "1-0" ? 1.0 : Result == "0-1" ? 0.0 : 0.5;
                
                // Assure-toi d'avoir le "using ChessDB.Utils;" pour que ça marche
                // Si tu n'utilises plus EloCalculator, tu peux commenter ces 3 lignes
                var (newWhite, newBlack) = EloCalculator.UpdateElo(white.Elo, black.Elo, resForWhite);
                white.Elo = newWhite;
                black.Elo = newBlack;
                
                _store.UpdatePlayer(white);
                _store.UpdatePlayer(black);
            }

            // Sauvegarde
            _store.AddGame(g);
            Games.Add(g);

            // Réinitialisation (facultatif, on garde souvent la compétition sélectionnée pour enchainer)
            MovesInput = "";
            // On ne remet pas forcément les ID à Empty pour permettre de saisir une autre partie du même tournoi rapidement
        }
    }
}