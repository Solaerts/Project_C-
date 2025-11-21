using System;

namespace ChessDB.Models
{
    public static class EloCalculator
    {
        public static (int newA, int newB) UpdateElo(int eloA, int eloB, double resultA)
        {
            // resultA: 1 = A win, 0.5 = draw, 0 = A loses
            const int K = 32;
            double expectedA = 1.0 / (1.0 + Math.Pow(10.0, (eloB - eloA) / 400.0));
            double expectedB = 1.0 - expectedA;

            int newA = (int)Math.Round(eloA + K * (resultA - expectedA));
            int newB = (int)Math.Round(eloB + K * ((1.0 - resultA) - expectedB));
            return (newA, newB);
        }
    }
}
