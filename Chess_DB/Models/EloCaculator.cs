using System;
using System.Collections.Generic;

namespace Chess_DB.Models;

public static class EloCalculator
{
    public static (int, int) UpdateElo(int eloA, int eloB, double result)
    {
        const int K = 32;

        double expectedA = 1.0 / (1 + Math.Pow(10, (eloB - eloA) / 400.0));
        double expectedB = 1.0 - expectedA;

        int newA = (int)(eloA + K * (result - expectedA));
        int newB = (int)(eloB + K * ((1 - result) - expectedB));

        return (newA, newB);
    }
}