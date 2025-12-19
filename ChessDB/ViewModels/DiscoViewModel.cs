using System;
using Avalonia.Media;
using ReactiveUI;
using System.Reactive.Linq; 

namespace ChessDB.ViewModels
{
    public class DiscoViewModel : ViewModelBase
    {
        private readonly Random _rand = new Random();

        // La couleur de fond qui change
        private IBrush _backgroundBrush = Brushes.Black;
        public IBrush BackgroundBrush
        {
            get => _backgroundBrush;
            set => this.RaiseAndSetIfChanged(ref _backgroundBrush, value);
        }

        public DiscoViewModel()
        {
            // Change la couleur toutes les 200ms (0.2 secondes)
            Observable.Interval(TimeSpan.FromMilliseconds(200))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(_ => ChangeColor());
        }

        private void ChangeColor()
        {
            byte r = (byte)_rand.Next(256);
            byte g = (byte)_rand.Next(256);
            byte b = (byte)_rand.Next(256);
            BackgroundBrush = new SolidColorBrush(Color.FromRgb(r, g, b));
        }
    }
}