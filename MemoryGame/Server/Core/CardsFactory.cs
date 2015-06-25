using System.Collections.Generic;

namespace MemoryGame.Server.Core
{
    public class CardsFactory
    {
        public static Card[,] Create(int width, int height)
        {
            var nrOfCards = width * height;

            var cards = CreateListOfCards(nrOfCards);

            return ConvertTo2Dimensions(width, height, cards);
        }

        private static Card[] CreateListOfCards(int nrOfCards)
        {
            var cards = new Card[nrOfCards];

            for (var i = 0; i < cards.Length; i += 2)
            {
                cards[i] = new Card(i + 1);
                cards[i + 1] = new Card(i + 1);
            }

            return cards;
        }

        private static Card[,] ConvertTo2Dimensions(int width, int height, IReadOnlyList<Card> cards)
        {
            var result = new Card[width, height];

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    result[i, j] = cards[j * width + i];
                }
            }

            return result;
        }
    }
}