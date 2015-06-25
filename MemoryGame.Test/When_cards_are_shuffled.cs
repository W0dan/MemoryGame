using System.Collections.Generic;
using MemoryGame.Server.Core;
using Xunit;

namespace MemoryGame.Test
{
    public class When_cards_are_shuffled
    {
        [Theory,
        InlineData(1),
        InlineData(2),
        InlineData(3),
        InlineData(10),
        ]
        public void Given_a_Seed_Then_each_card_still_occurs_exactly_2_times(int seed)
        {
            const int width = 5;
            const int height = 6;

            var cards = CardsFactory.Create(width, height);

            cards = CardsShuffler.Shuffle(cards, seed);

            var testResults = new Dictionary<int, int>();

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    var cardNumber = cards[i, j].Number;

                    if (!testResults.ContainsKey(cardNumber))
                    {
                        testResults.Add(cardNumber, 1);
                    }
                    else
                    {
                        testResults[cardNumber]++;
                    }
                }
            }

            Assert.Equal(width * height / 2, testResults.Count);

            foreach (var testResult in testResults)
            {
                Assert.Equal(2, testResult.Value);
            }
        }
    }
}