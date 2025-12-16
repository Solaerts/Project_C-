using Avalonia.Controls;
using Avalonia.Interactivity;
using ChessDB.Models;
using ChessDB.ViewModels;

namespace ChessDB.Views
{
    public partial class GamesView : UserControl
    {
        public GamesView()
        {
            InitializeComponent();
        }

        // Cette méthode est appelée quand on clique sur "Voir les coups"
        private void OnOpenDetailsClick(object sender, RoutedEventArgs e)
        {
            // 1. On récupère le bouton cliqué
            var button = sender as Button;
            
            // 2. On récupère la partie liée à ce bouton (grâce à la propriété Tag="{Binding}")
            var game = button?.Tag as Game;

            if (game != null)
        {
                var detailsWindow = new GameDetailsWindow();
                detailsWindow.DataContext = new GameDetailsViewModel(game);

                // CORRECTION : On vérifie que la fenêtre parente existe bien avant d'ouvrir
                var topLevel = TopLevel.GetTopLevel(this) as Window;
                if (topLevel != null)
            {
                detailsWindow.ShowDialog(topLevel);
            }
        }
    }
}
}