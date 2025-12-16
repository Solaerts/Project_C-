using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;      // Nécessaire pour 'Unit'
using System.Reactive.Linq; // <--- INDISPENSABLE pour '.ObserveOn'
using ReactiveUI;           // Nécessaire pour ReactiveCommand
using ChessDB.Models;
using ChessDB.Utils;

namespace ChessDB.ViewModels
{
    public class CompetitionsViewModel : ViewModelBase
    {
        private readonly IDataStore _store;
        public ObservableCollection<Competition> Competitions { get; }

        private string _newCompetitionName = "";
        public string NewCompetitionName
        {
            get => _newCompetitionName;
            set => this.RaiseAndSetIfChanged(ref _newCompetitionName, value);
        }

        private int _gamesCount = 10;
        public int GamesCount
        {
            get => _gamesCount;
            set => this.RaiseAndSetIfChanged(ref _gamesCount, value);
        }

        // On utilise ReactiveCommand ici
        public ReactiveCommand<Unit, Unit> AddCompetitionCommand { get; }

        public CompetitionsViewModel(IDataStore store)
        {
            _store = store;
            Competitions = new ObservableCollection<Competition>(_store.Competitions);

            // Surveillance des changements
            var canAddCompetition = this.WhenAnyValue(
                x => x.NewCompetitionName,
                (name) => !string.IsNullOrWhiteSpace(name)
            )
            .ObserveOn(RxApp.MainThreadScheduler); // <--- EMPECHE LE CRASH "Invalid Thread"

            // Création de la commande avec la condition de sécurité
            AddCompetitionCommand = ReactiveCommand.Create(AddCompetition, canAddCompetition);
        }

        public void AddCompetition()
        {
            // Pas besoin de revérifier IsNullOrWhiteSpace ici, le bouton est désactivé sinon
            var c = new Competition 
            { 
                Name = NewCompetitionName.Trim(),
                TotalGamesPlanned = GamesCount
            };
            
            _store.AddCompetition(c);
            Competitions.Add(c);
            
            NewCompetitionName = "";
            GamesCount = 10;
        }

        public Competition? GetById(Guid id) => Competitions.FirstOrDefault(c => c.Id == id);
    }
}