using Avalonia.Media;

namespace ChessDB.Models
{
    public class MoveDisplay
    {
        public string Text { get; set; } = string.Empty;
        
        // La couleur de la pastille (Noir ou Blanc/Gris clair)
        public IBrush Color { get; set; } = Brushes.Black;
    }
}