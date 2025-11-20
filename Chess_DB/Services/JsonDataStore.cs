using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;

using Chess_DB.Models;

namespace Chess_DB.Services;

public class JsonDataStore : IDataStore
{
    private const string FilePath = "database.json";

    public List<Player> Players { get; private set; } = new();
    public List<Competition> Competitions { get; private set; } = new();
    public List<Game> Games { get; private set; } = new();

    public JsonDataStore()
    {
        if (File.Exists(FilePath))
        {
            var json = File.ReadAllText(FilePath);
            var data = JsonSerializer.Deserialize<JsonDataStore>(json);

            Players = data.Players;
            Competitions = data.Competitions;
            Games = data.Games;
        }
    }

    public void Save()
    {
        var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }
}
