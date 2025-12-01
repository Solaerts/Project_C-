using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using ChessDB.Models;

namespace ChessDB.Services
{
    public class JsonDataStore : IDataStore
    {
        private const string FilePath = "chessdb.json";

        public List<Player> Players { get; private set; } = new();
        public List<Competition> Competitions { get; private set; } = new();
        public List<Game> Games { get; private set; } = new();

        public JsonDataStore()
        {
            if (File.Exists(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                var dto = JsonSerializer.Deserialize<DatabaseDto>(json);

                if (dto != null)
                {
                    Players = dto.Players ?? new();
                    Competitions = dto.Competitions ?? new();
                    Games = dto.Games ?? new();
                }
            }
        }

        public void Save()
        {
            var dto = new DatabaseDto
            {
                Players = this.Players,
                Competitions = this.Competitions,
                Games = this.Games
            };

            var json = JsonSerializer.Serialize(dto, new JsonSerializerOptions
            {
                WriteIndented = true
            });

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
