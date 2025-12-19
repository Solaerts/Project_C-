using Avalonia.Controls;
using Avalonia.Input;
using System.Collections.Generic;
using System.Linq;
using ChessDB.ViewModels;

namespace ChessDB.Views
{
    public partial class MainWindow : Window
    {
        // La séquence : Gauche, Droite, Gauche, Droite, Haut, Haut, Bas, Bas, Enter
        private readonly List<Key> _secretCode = new List<Key>
        {
            Key.Left, Key.Right, 

        };

        private readonly List<Key> _inputHistory = new List<Key>();

        public MainWindow()
        {
            InitializeComponent();
            
            // IMPORTANT : On dit à la fenêtre d'écouter le clavier
            this.KeyDown += OnWindowKeyDown;
        }

        private void OnWindowKeyDown(object? sender, KeyEventArgs e)
        {
            // 1. On mémorise la touche
            _inputHistory.Add(e.Key);

            // 2. On garde l'historique court (pas plus long que le code secret)
            if (_inputHistory.Count > _secretCode.Count)
            {
                _inputHistory.RemoveAt(0);
            }

            // 3. On compare l'historique avec le code secret
            if (_inputHistory.SequenceEqual(_secretCode))
            {
                TriggerEasterEgg();
                _inputHistory.Clear();
            }
        }

        private void TriggerEasterEgg()
        {
            var discoWindow = new DiscoWindow();
            discoWindow.DataContext = new DiscoViewModel();
            discoWindow.ShowDialog(this);
        }
    }
}