using System;

namespace ChessDB.Utils // Ou namespace ChessDB.Models selon où tu le mets
{
    public static class EloCalculator
    {
        public static (int newWhiteElo, int newBlackElo) UpdateElo(int whiteElo, int blackElo, double whiteScore)
        {
            // K-Factor détermine la volatilité du classement. 32 est standard pour les clubs amateurs.
            const int K = 32;

            // Estimation de la probabilité de victoire
            double expectedWhite = 1 / (1 + Math.Pow(10, (blackElo - whiteElo) / 400.0));
            double expectedBlack = 1 / (1 + Math.Pow(10, (whiteElo - blackElo) / 400.0));

            // Score réel du noir (si blanc fait 1, noir fait 0)
            double blackScore = 1.0 - whiteScore;

            // Calcul des nouveaux classements
            int newWhite = (int)(whiteElo + K * (whiteScore - expectedWhite));
            int newBlack = (int)(blackElo + K * (blackScore - expectedBlack));

            return (newWhite, newBlack);
        }
    }
}