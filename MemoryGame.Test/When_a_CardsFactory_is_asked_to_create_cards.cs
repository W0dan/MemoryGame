using System.Collections.Generic;
using MemoryGame.Server.Core;
using Xunit;

namespace MemoryGame.Test
{
    public class When_a_CardsFactory_is_asked_to_create_cards
    {
        [Fact]
        public void Given_a_width_and_a_height_Then_cards_are_created_with_width_x_height_cards()
        {
            const int expectedWidth = 5;
            const int expectedHeight = 6;

            var cards = DeckOfCards.Deal(expectedWidth, expectedHeight);

            Assert.Equal(expectedWidth, cards.GetLength(0));
            Assert.Equal(expectedHeight, cards.GetLength(1));
        }

        [Fact]
        public void Given_a_width_and_a_height_Then_each_card_occurs_exactly_2_times()
        {
            const int width = 5;
            const int height = 6;

            var cards = DeckOfCards.Deal(width, height);

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