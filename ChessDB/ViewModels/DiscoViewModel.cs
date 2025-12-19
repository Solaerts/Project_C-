using System;
using System.Collections.Generic;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using ReactiveUI;
using System.Reactive.Linq;

namespace ChessDB.ViewModels
{
    public class DiscoViewModel : ViewModelBase
    {
        // CORRECTION 1 : On utilise List<IBrush> pour accepter images ET couleurs
        private readonly List<IBrush> _images = new List<IBrush>();
        
        private int _currentIndex = 0;

        // CORRECTION 2 : On initialise la variable tout de suite pour éviter l'avertissement CS8618
        private IBrush _backgroundBrush = Brushes.Black; 
        
        public IBrush BackgroundBrush
        {
            get => _backgroundBrush;
            set => this.RaiseAndSetIfChanged(ref _backgroundBrush, value);
        }

        public DiscoViewModel()
        {
            LoadImages();

            // Si on a trouvé des images ou créé des couleurs de secours, on affiche la première
            if (_images.Count > 0)
            {
                 BackgroundBrush = _images[0];
            }

            // On lance le défilement
            Observable.Interval(TimeSpan.FromMilliseconds(300))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(_ => ChangeImage());
        }

        private void LoadImages()
        {
            // Tes fichiers images (assure-toi qu'ils sont bien dans Assets avec "Build Action: AvaloniaResource")
            string[] filenames = { "Disco1.jpg", "Disco2.jpg", "Disco3.jpg", "Disco4.jpg" };
            
            foreach (var name in filenames)
            {
                try
                {
                    var uri = new Uri($"avares://ChessDB/Assets/{name}");
                    using (var stream = AssetLoader.Open(uri))
                    {
                        var bitmap = new Bitmap(stream);
                        var brush = new ImageBrush(bitmap)
                        {
                            Stretch = Stretch.UniformToFill 
                        };
                        
                        _images.Add(brush); // Ajout d'une ImageBrush
                    }
                }
                catch (Exception)
                {
                    // Image non trouvée, on continue
                }
            }

            // Si aucune image n'a fonctionné, on met des couleurs qui flashent
            if (_images.Count == 0)
            {
                // C'est ici que ça plantait avant : maintenant on a le droit d'ajouter des SolidColorBrush
                _images.Add(new SolidColorBrush(Colors.Magenta));
                _images.Add(new SolidColorBrush(Colors.Cyan));
                _images.Add(new SolidColorBrush(Colors.Yellow));
            }
        }

        private void ChangeImage()
        {
            if (_images.Count == 0) return;

            _currentIndex = (_currentIndex + 1) % _images.Count;
            BackgroundBrush = _images[_currentIndex];
        }
    }
}