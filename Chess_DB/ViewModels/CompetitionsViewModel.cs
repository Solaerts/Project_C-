using System.Collections.ObjectModel;
using Chess_DB.Models;
using Chess_DB.Services;
using ReactiveUI;

namespace Chess_DB.ViewModels;

public class CompetitionsViewModel : ViewModelBase
{
    private readonly IDataStore _store;

    public ObservableCollection<Competition> Competitions { get; }
    public ObservableCollection<Player> Players { get; }

    public Competition? SelectedCompetition { get; set; }
    public Player? SelectedPlayerToAdd { get; set; }

    public string NewCompetitionName { get; set; } = "";

    public CompetitionsViewModel(IDataStore store)
    {
        _store = store;
        Competitions = new ObservableCollection<Competition>(store.Competitions);
        Players = new ObservableCollection<Player>(store.Players);
    }

    public void AddCompetition()
    {
        if (string.IsNullOrWhiteSpace(NewCompetitionName)) return;

        var comp = new Competition { Name = NewCompetitionName };
        _store.Competitions.Add(comp);
        Competitions.Add(comp);
        NewCompetitionName = "";

        _store.Save();
    }

    public void AddPlayerToCompetition()
    {
        if (SelectedCompetition == null || SelectedPlayerToAdd == null)
            return;

        if (!SelectedCompetition.PlayerIds.Contains(SelectedPlayerToAdd.Id))
        {
            SelectedCompetition.PlayerIds.Add(SelectedPlayerToAdd.Id);
            _store.Save();
        }
    }
}
