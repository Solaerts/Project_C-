using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading; // Nécessaire pour le Timer
using System;

namespace ChessDB.Views
{
    public partial class DiscoWindow : Window
    {
        private readonly DispatcherTimer _timer;
        private readonly Random _rand = new Random();

        public DiscoWindow()
        {
            InitializeComponent();

            // 1. On configure le Timer pour qu'il tique toutes les 200ms
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(200)
            };

            // 2. On lui dit quoi faire à chaque tic
            _timer.Tick += (sender, e) => ChangeColor();

            // 3. On démarre la fête !
            _timer.Start();

            // 4. Important : On arrête le timer quand on ferme la fenêtre
            this.Closed += (s, e) => _timer.Stop();
        }

        private void ChangeColor()
        {
            // On génère une couleur aléatoire (R, G, B)
            byte r = (byte)_rand.Next(256);
            byte g = (byte)_rand.Next(256);
            byte b = (byte)_rand.Next(256);

            // On l'applique au fond de la fenêtre
            this.Background = new SolidColorBrush(Color.FromRgb(r, g, b));
        }
    }
}