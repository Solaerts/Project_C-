using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using ChessDB.ViewModels;
using ChessDB.Views;
using ChessDB.Services; // Add this namespace
using System;

namespace ChessDB;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
{
    if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
    {
        // On supprime les plugins de validation par défaut qui peuvent gêner
        // (Si cette ligne pose problème, tu peux la commenter)
        BindingPlugins.DataValidators.RemoveAt(0);

        try 
        {
            // 1. On essaie de créer le Service de Base de Données
            Console.WriteLine("Tentative de connexion à la base de données...");
            var dataStore = new SqliteDataStore();
            Console.WriteLine("Base de données connectée/créée avec succès !");

            // 2. On crée le ViewModel Principal avec le service
            var vm = new MainWindowViewModel(dataStore);

            // 3. On lance la fenêtre
            desktop.MainWindow = new MainWindow
            {
                DataContext = vm
            };
        }
        catch (Exception ex)
        {
            // C'EST ICI QUE L'ERREUR SERA AFFICHÉE
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("ERREUR FATALE AU DÉMARRAGE :");
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
            Console.WriteLine("--------------------------------------------------");
        }
    }

    base.OnFrameworkInitializationCompleted();
}
}