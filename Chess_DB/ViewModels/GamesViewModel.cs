using System;
using System.Collections.Generic;

using System.Collections.ObjectModel;
using Chess_DB.Models;
using Chess_DB.Services;
using ReactiveUI;

namespace Chess_DB.ViewModels;

public class GamesViewModel : ViewModelBase
{
    private readonly IDataStore _store;

    public ObservableCollection<Competition> Competitions { get; }
    public ObservableCollection<Player> Players { get; }

    public Competition? SelectedCompetition { get; set; }
    public Player? SelectedWhite { get; set; }
    public Player? SelectedBlack { get; set; }

    public string Moves { get; set; } = "";
    public Player? SelectedWinner { get; set; }

    public GamesViewModel(IDataStore store)
    {
        _store = store;

        Competitions = new ObservableCollection<Competition>(store.Competitions);
        Players = new ObservableCollection<Player>(store.Players);
    }

    public void SaveGame()
    {
        if (SelectedCompetition == null || SelectedWhite == null || SelectedBlack == null)
            return;

        var game = new Game
        {
            WhitePlayer = SelectedWhite.Id,
            BlackPlayer = SelectedBlack.Id,
            CompetitionId = SelectedCompetition.Id,
            Moves = Moves,
            WinnerId = SelectedWinner?.Id ?? Guid.Empty
        };

        _store.Games.Add(game);

        // Mise à jour ELO
        UpdateElo(SelectedWhite, SelectedBlack, SelectedWinner);

        _store.Save();

        // Reset UI
        Moves = "";
        SelectedWinner = null;
    }

    private void UpdateElo(Player white, Player black, Player? winner)
    {
        double result = winner == null ? 0.5 :
                        (winner.Id == white.Id ? 1.0 : 0.0);

        var (newWhite, newBlack) = EloCalculator.UpdateElo(white.Elo, black.Elo, result);

        white.Elo = newWhite;
        black.Elo = newBlack;
    }
}
