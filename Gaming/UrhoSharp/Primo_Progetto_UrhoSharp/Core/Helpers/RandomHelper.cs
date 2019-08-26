using System;

namespace SamplyGame
{
    public class RandomHelper
    {
        private static readonly Random Random = new Random();

        // restituisce un float casuale tra min e max, inclusi da entrambe le estremità
        public static float NextRandom(float min, float max) => (float)((Random.NextDouble() * (max - min)) + min);

        // restituisce un numero intero casuale compreso tra min e max - 1
        public static int NextRandom(int min, int max) => Random.Next(min, max);

        // restituisce un valore booleano casuale
        public static bool NextBoolRandom() => Random.Next(0, 2) == 1;
    }
}