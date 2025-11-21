using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using ChessDB.Models;

namespace ChessDB.Services
{
    // Simple JSON persistence for Players, Competitions and Games.
    public class JsonDataStore
    {
        private const string FilePath = "chessdb.json";

        public List<Player> Players { get; set; } = new();
        public List<Competition> Competitions { get; set; } = new();
        public List<Game> Games { get; set; } = new();

        public JsonDataStore()
        {
            Load();
        }

        public void Load()
        {
            try
            {
                if (!File.Exists(FilePath)) return;

                var json = File.ReadAllText(FilePath);
                var dto = JsonSerializer.Deserialize<DatabaseDto>(json);
                if (dto != null)
                {
                    Players = dto.Players ?? new List<Player>();
                    Competitions = dto.Competitions ?? new List<Competition>();
                    Games = dto.Games ?? new List<Game>();
                }
            }
            catch
            {
                // If load fails, start with empty dataset.
                Players = new List<Player>();
                Competitions = new List<Competition>();
                Games = new List<Game>();
            }
        }

        public void Save()
        {
            var dto = new DatabaseDto
            {
                Players = Players,
                Competitions = Competitions,
                Games = Games
            };

            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(dto, options);
            File.WriteAllText(FilePath, json);
        }

        private class DatabaseDto
        {
            public List<Player>? Players { get; set; }
            public List<Competition>? Competitions { get; set; }
            public List<Game>? Games { get; set; }
        }
    }
}
