using System.Collections.ObjectModel;
using Avalonia.Media;
using ChessDB.Models;

namespace ChessDB.ViewModels
{
    public class GameDetailsViewModel : ViewModelBase
    {
        public string Title { get; }
        public ObservableCollection<MoveDisplay> MovesList { get; } = new();

        public GameDetailsViewModel(Game game)
        {
            Title = $"{game.WhitePlayer?.FullName ?? "Blanc"} vs {game.BlackPlayer?.FullName ?? "Noir"}";
            
            ParseMoves(game.Moves);
        }

        private void ParseMoves(string movesText)
        {
            if (string.IsNullOrWhiteSpace(movesText)) return;

            // On suppose que l'utilisateur sépare les coups par des virgules ou des sauts de ligne
            // Exemple : "Pion en E4, Pion en E5, Cavalier en F3"
            var splitMoves = movesText.Split(new[] { ',', '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

            bool isWhiteTurn = true;

            foreach (var moveText in splitMoves)
            {
                var cleanText = moveText.Trim();
                if (string.IsNullOrWhiteSpace(cleanText)) continue;

                MovesList.Add(new MoveDisplay
                {
                    Text = cleanText,
                    // Si c'est au tour des blancs : Pastille Blanche (GhostWhite), sinon Noire
                    Color = isWhiteTurn ? Brushes.GhostWhite : Brushes.Black
                });

                // On change de tour
                isWhiteTurn = !isWhiteTurn;
            }
        }
    }
}