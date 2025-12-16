using System;

namespace ChessDB.Models // ou ChessDB.Utils
{
    public static class EloCalculator
    {
        public static (int newWhiteElo, int newBlackElo) UpdateElo(int whiteElo, int blackElo, double whiteScore)
        {
            const int K = 32;

            // ATTENTION : Bien utiliser 400.0 (avec le point) pour faire une division décimale
            double expectedWhite = 1.0 / (1.0 + Math.Pow(10, (blackElo - whiteElo) / 400.0));
            double expectedBlack = 1.0 / (1.0 + Math.Pow(10, (whiteElo - blackElo) / 400.0));

            double blackScore = 1.0 - whiteScore;

            int newWhite = (int)(whiteElo + K * (whiteScore - expectedWhite));
            int newBlack = (int)(blackElo + K * (blackScore - expectedBlack));

            return (newWhite, newBlack);
        }
    }
}