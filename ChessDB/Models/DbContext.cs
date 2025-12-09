using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

public class ClubContext : DbContext
{
    // Liste des tables que tu veux créer
    public DbSet<Member> Members { get; set; }
    public DbSet<Game> Games { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // 1. On définit le nom du fichier
        string Filename = "club_echecs.db";

        // 2. On récupère le chemin vers ton dossier "Data_Base"
        // Note : En mode "Debug", l'application tourne dans bin/Debug/net8.0/
        // L'astuce ci-dessous permet de pointer vers le dossier Data_Base 
        // situé à la racine du projet quand tu développes.
        string ProjectPath = AppDomain.CurrentDomain.BaseDirectory;
        
        // On construit le chemin complet : "C:\...\MonProjet\Data_Base\club_echecs.db"
        // Si tu veux que le fichier soit créé dans le dossier d'exécution, simplifie le chemin.
        string DbPath = Path.Combine(ProjectPath, "Data_Base", Filename);

        // ASTUCE : S'assurer que le dossier existe, sinon ça plante
        string Directory = Path.GetDirectoryName(DbPath);
        if (!Directory.Exists(Directory))
        {
            Directory.CreateDirectory(Directory);
        }

        // 3. On dit à l'outil d'utiliser ce fichier
        optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }
}