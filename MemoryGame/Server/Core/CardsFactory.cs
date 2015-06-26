using System;
using System.Collections.Generic;

namespace MemoryGame.Server.Core
{
    public class CardsFactory
    {
        public static Card[,] Create(int width, int height)
        {
            var nrOfCards = width * height;

            var cards = CreateListOfCards(nrOfCards);

            cards = Shuffle(cards);

            return ConvertTo2Dimensions(width, height, cards);
        }

        private static Card[] CreateListOfCards(int nrOfCards)
        {
            var cards = new Card[nrOfCards];

            for (var i = 0; i < cards.Length; i += 2)
            {
                cards[i] = new Card(i / 2 + 1);
                cards[i + 1] = new Card(i / 2 + 1);
            }

            return cards;
        }

        private static Card[] Shuffle(Card[] cards)
        {
            var rnd = new Random((int)DateTime.Now.Ticks);

            for (var i = 0; i < cards.Length; i++)
            {
                var randomIndex = rnd.Next(cards.Length - 1);

                Swap(cards, i, randomIndex);
            }

            return cards;
        }

        private static void Swap(IList<Card> cards, int index1, int index2)
        {
            var tmp = cards[index1];
            cards[index1] = cards[index2];
            cards[index2] = tmp;
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