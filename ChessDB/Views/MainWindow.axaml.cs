using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System.Collections.Generic;
using System.Linq;
using ChessDB.ViewModels;


namespace ChessDB.Views
{
    public partial class MainWindow : Window
    {
        // La séquence secrète : Haut, Haut, Bas, Bas, Gauche, Droite, Gauche, Droite, B, A (ou ta version)
        // Ici je remets ta version : G, D, G, D, H, H, B, B, Enter
        private readonly List<Key> _secretCode = new List<Key>
        {
            Key.Up, Key.Up,
            Key.Down, Key.Down,
            Key.Left, Key.Right, 
            Key.Left, Key.Right,
            Key.Enter
        };

        private readonly List<Key> _inputHistory = new List<Key>();

        public MainWindow()
        {
            InitializeComponent();
            
            // INDISPENSABLE : On écoute le clavier
            this.AddHandler(KeyDownEvent, OnWindowKeyDown, RoutingStrategies.Tunnel);
        }

        private void OnWindowKeyDown(object? sender, KeyEventArgs e)
        {
            _inputHistory.Add(e.Key);

            // On garde seulement les X dernières touches pour ne pas saturer la mémoire
            if (_inputHistory.Count > _secretCode.Count)
            {
                _inputHistory.RemoveAt(0);
            }

            // On compare
            if (_inputHistory.SequenceEqual(_secretCode))
            {
                TriggerEasterEgg();
                _inputHistory.Clear();
            }
        }

        private void TriggerEasterEgg()
        {
            // 1. Créer la fenêtre
            var discoWindow = new DiscoWindow();
            
            // 2. Lui donner son ViewModel (Le Cerveau)
            discoWindow.DataContext = new DiscoViewModel();
            
            // 3. Afficher
            discoWindow.ShowDialog(this);
        }
    }
}