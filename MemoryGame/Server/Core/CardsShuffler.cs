using System;

namespace MemoryGame.Server.Core
{
    public class CardsShuffler
    {
        public static Card[,] Shuffle(Card[,] cards, int shuffleSeed)
        {
            var width = cards.GetLength(0);
            var height = cards.GetLength(1);

            var rnd = new Random((int)DateTime.Now.Ticks);

            for (var i = 0; i < (width * height * shuffleSeed); i++)
            {
                var x1 = rnd.Next(width - 1);
                var x2 = rnd.Next(width - 1);
                var y1 = rnd.Next(height - 1);
                var y2 = rnd.Next(height - 1);

                Swap(cards, x1, y1, x2, y2);
            }

            return cards;
        }

        private static void Swap(Card[,] cards, int x1, int y1, int x2, int y2)
        {
            var tmpCard = cards[x1, y1];
            cards[x1, y1] = cards[x2, y2];
            cards[x2, y2] = tmpCard;
        }
    }
}