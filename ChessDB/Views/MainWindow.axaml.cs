using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using ChessDB.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace ChessDB.Views
{
    public partial class MainWindow : Window
    {
        // La séquence secrète : G, D, G, D, H, H, B, B, Enter
        private readonly List<Key> _secretCode = new List<Key>
        {
            Key.Left, Key.Right, 
            Key.Left, Key.Right,
            Key.Up, Key.Up,
            Key.Down, Key.Down,
            Key.Enter
        };

        // Tampon pour stocker les dernières touches appuyées
        private readonly List<Key> _inputHistory = new List<Key>();

        public MainWindow()
        {
            InitializeComponent();
            
            // On s'abonne à l'événement "Touche appuyée" de la fenêtre
            this.KeyDown += OnWindowKeyDown;
        }

        private void OnWindowKeyDown(object? sender, KeyEventArgs e)
        {
            // 1. Ajouter la touche à l'historique
            _inputHistory.Add(e.Key);

            // 2. Garder l'historique propre (pas plus long que le code secret)
            if (_inputHistory.Count > _secretCode.Count)
            {
                _inputHistory.RemoveAt(0); // On retire la plus vieille touche
            }

            // 3. Vérifier si l'historique correspond au code secret
            if (_inputHistory.SequenceEqual(_secretCode))
            {
                TriggerEasterEgg();
                _inputHistory.Clear(); // On vide pour éviter de le déclencher 2 fois de suite
            }
        }

        private void TriggerEasterEgg()
        {
            // Au lieu d'ajouter un joueur, on lance la fenêtre Disco
            var discoWindow = new DiscoWindow();
            
            // On l'affiche par dessus la fenêtre principale
            discoWindow.ShowDialog(this);
        }
    }
}