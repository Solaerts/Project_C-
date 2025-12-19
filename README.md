Rapport de Projet : Chess Federation Manager (ChessDB)
1. Introduction et Mise en Contexte

Ce projet a pour objectif la création d'une application de bureau permettant de gérer les activités d'une fédération d'échecs. L'application, nommée ChessDB, a été développée en C# avec le framework Avalonia UI (version .NET 9), en adoptant l'architecture MVVM (Model-View-ViewModel) via la librairie ReactiveUI.

Le but principal est de fournir une interface intuitive pour :

Enregistrer et gérer les joueurs (création, affichage, modification).

Gérer les compétitions (tournois, championnats).

Enregistrer les matchs joués entre deux joueurs.

Calculer et mettre à jour automatiquement le classement ELO des joueurs après chaque partie.

La persistance des données est assurée par une base de données SQLite gérée via l'ORM Entity Framework Core, garantissant la portabilité et la robustesse de l'application.

2. Fonctionnalité Supplémentaire (Bonus)

Pour la fonctionnalité bonus, j'ai implémenté un "Easter Egg" ludique accessible via une séquence de touches secrète (le célèbre Konami Code).

Description : Lorsque l'utilisateur tape la séquence Haut, Haut, Bas, Bas, Gauche, Droite, Gauche, Droite, Entrée sur le clavier (depuis la fenêtre principale), une fenêtre cachée s'ouvre.

Comportement : Cette fenêtre, baptisée "Disco Mode", affiche un message de félicitations sur un fond qui change de couleur ou d'image de manière cyclique et rapide, créant une ambiance festive.

Implémentation Technique :

Utilisation de l'événement KeyDown avec une stratégie de "Tunneling" pour intercepter les touches avant les contrôles de l'interface.

Un DiscoViewModel dédié utilise System.Reactive (Observable.Interval) pour gérer le chronomètre d'animation sans bloquer l'interface utilisateur.

3. Diagramme de Classes

Ce diagramme illustre la structure principale de l'application, séparant les Modèles (Données), les ViewModels (Logique) et les Services.

    classDiagram
        class Player {
            +Guid Id
            +string FirstName
            +string LastName
            +int Elo
            +ICollection~Game~ GamesAsWhite
            +ICollection~Game~ GamesAsBlack
        }

        class Game {
            +Guid Id
            +Guid WhitePlayerId
            +Guid BlackPlayerId
            +string Result
            +DateTime PlayedAt
        }

        class IDataStore {
            <<interface>>
            +List~Player~ Players
            +List~Game~ Games
            +void AddGame(Game g)
            +void UpdatePlayer(Player p)
        }

        class SqliteDataStore {
            -ChessDbContext _context
            +void UpdatePlayer(Player p)
        }

        class ViewModelBase {
            <<abstract>>
        }

        class GamesViewModel {
            -IDataStore _store
            +ObservableCollection~Game~ Games
            +void AddGame()
        }

        class PlayersViewModel {
            -IDataStore _store
            +ObservableCollection~Player~ Players
        }

        class EloCalculator {
            <<static>>
            +UpdateElo(int white, int black, double score)
        }

        SqliteDataStore ..|> IDataStore
        Player "1" -- "*" Game : Plays
        GamesViewModel --> IDataStore
        PlayersViewModel --> IDataStore
        GamesViewModel ..> EloCalculator : Uses
        ViewModelBase <|-- GamesViewModel
        ViewModelBase <|-- PlayersViewModel

4. Diagramme de Séquences

Ce diagramme détaille le processus d'ajout d'un match, qui est l'action la plus complexe car elle implique la mise à jour de l'ELO des joueurs.

sequenceDiagram
    actor User
    participant View as GamesView
    participant VM as GamesViewModel
    participant Elo as EloCalculator
    participant DB as SqliteDataStore

    User->>View: Sélectionne Joueurs & Résultat (1-0)
    User->>View: Clique "Ajouter Partie"
    View->>VM: Execute AddGameCommand
    
    VM->>VM: Vérifie validité (White != Black)
    
    rect rgb(200, 220, 240)
    Note over VM, Elo: Calcul du nouvel ELO
    VM->>Elo: UpdateElo(WhiteElo, BlackElo, 1.0)
    Elo-->>VM: Retourne (NouveauEloBlanc, NouveauEloNoir)
    end

    VM->>DB: UpdatePlayer(WhitePlayer)
    VM->>DB: UpdatePlayer(BlackPlayer)
    
    VM->>DB: AddGame(NewGame)
    DB-->>VM: Confirmation
    
    VM-->>View: Mise à jour de la liste (Games.Add)
    View-->>User: Affiche la nouvelle partie

5. Diagramme d'Activité

Ce diagramme montre le flux de travail utilisateur lors de l'enregistrement d'une partie.

flowchart TD
    A([Début]) --> B{Sélection Joueurs}
    B -->|Joueurs Identiques| C[Désactiver Bouton Ajout]
    B -->|Joueurs Différents| D[Saisie du Résultat]
    D --> E[Clic sur Ajouter]
    
    E --> F{Calcul ELO}
    F --> G[Mise à jour Joueur Blanc]
    F --> H[Mise à jour Joueur Noir]
    
    G --> I[Sauvegarde en Base de Données]
    H --> I
    
    I --> J[Ajout à la liste affichée]
    J --> K([Fin])
    
    C --> B

6. Adaptabilité du Projet

Bien que conçu pour une fédération d'échecs, ce projet présente une forte adaptabilité pour d'autres fédérations sportives (ex: Tennis, Badminton, Go, E-sport) pour les raisons suivantes :

Abstraction des concepts : Les classes Player et Game sont génériques. Elles ne contiennent pas de règles spécifiques aux échecs (comme le déplacement des pièces). Seul le résultat (Gagnant/Perdant/Nul) compte.

Système ELO universel : Le moteur de calcul EloCalculator implémente l'algorithme standard utilisé dans de nombreux sports compétitifs en 1 contre 1. Il peut être réutilisé tel quel pour n'importe quel classement basé sur la performance relative.

Architecture Modulaire : Grâce à l'injection de dépendances (voir point suivant), on pourrait facilement remplacer le SqliteDataStore par une API Web ou un fichier JSON si la nouvelle fédération a des besoins de stockage différents, sans toucher au reste de l'application.

7. Principes SOLID Utilisés

Le projet respecte plusieurs principes de conception SOLID pour assurer sa maintenabilité.

A. Dependency Inversion Principle (DIP) - Inversion des Dépendances
Description : Les modules de haut niveau ne doivent pas dépendre des modules de bas niveau. Les deux doivent dépendre d'abstractions. Justification dans le projet : Les ViewModels (GamesViewModel, PlayersViewModel) ne connaissent pas la classe concrète SqliteDataStore. Ils ne connaissent que l'interface IDataStore.

Code : public GamesViewModel(IDataStore store) { ... }

Avantage : Cela permet de changer la base de données (passer de SQLite à MySQL ou à une fausse base de données pour les tests) sans changer une seule ligne de code dans les ViewModels.

B. Single Responsibility Principle (SRP) - Responsabilité Unique
Description : Une classe ne doit avoir qu'une seule raison de changer. Justification dans le projet : Nous avons séparé la logique métier de la logique de présentation et de l'accès aux données.

EloCalculator : Ne s'occupe que des mathématiques. Il ne sait pas ce qu'est une base de données ou une fenêtre.

SqliteDataStore : Ne s'occupe que de parler à la base de données (Sauvegarder, Lire). Il ne fait aucun calcul.

GamesViewModel : Ne s'occupe que de faire le lien entre l'utilisateur et les données.

Avantage : Si on veut changer la formule ELO, on ne risque pas de casser la base de données. Si on change le design de la fenêtre, on ne risque pas de fausser les calculs.

8. Conclusion

Ce projet m'a permis de mettre en pratique les concepts fondamentaux du développement d'applications modernes en C#. J'ai pu appréhender la puissance du pattern MVVM avec Avalonia et ReactiveUI, qui offre une séparation claire entre l'interface et le code. L'intégration d'Entity Framework Core a simplifié la gestion des données relationnelles.

La réalisation de la fonctionnalité bonus et la gestion des cas limites (comme la mise à jour réactive de l'interface lors des changements d'ELO) ont renforcé ma compréhension de la programmation événementielle et réactive. Le résultat est une application fonctionnelle, robuste et facilement extensible à d'autres contextes sportifs.