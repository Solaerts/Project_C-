using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ReactiveUI;
using ChessDB.Models;
using ChessDB.Services;
using ChessDB.Utils;

namespace ChessDB.ViewModels
{
    public class CompetitionsViewModel : ViewModelBase
    {
        private readonly JsonDataStore _store;

        public ObservableCollection<Competition> Competitions { get; }

        private string _newCompetitionName = "";
        public string NewCompetitionName
        {
            get => _newCompetitionName;
            set => this.RaiseAndSetIfChanged(ref _newCompetitionName, value);
        }

        public ICommand AddCompetitionCommand { get; }

        public CompetitionsViewModel(JsonDataStore store)
        {
            _store = store;
            Competitions = new ObservableCollection<Competition>(_store.Competitions);
            AddCompetitionCommand = new RelayCommand(_ => AddCompetition(), _ => !string.IsNullOrWhiteSpace(NewCompetitionName));
        }

        public void AddCompetition()
        {
            if (string.IsNullOrWhiteSpace(NewCompetitionName)) return;
            var c = new Competition { Name = NewCompetitionName.Trim() };
            _store.Competitions.Add(c);
            Competitions.Add(c);
            NewCompetitionName = "";
            _store.Save();
        }

        public void AddPlayerToCompetition(Guid competitionId, Guid playerId)
        {
            var comp = Competitions.FirstOrDefault(c => c.Id == competitionId);
            if (comp == null) return;
            if (!comp.PlayerIds.Contains(playerId))
            {
                comp.PlayerIds.Add(playerId);
                _store.Save();
            }
        }

        public Competition? GetById(Guid id) => Competitions.FirstOrDefault(c => c.Id == id);
    }
}
