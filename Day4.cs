using System.Collections.Frozen;

namespace adventOfCode2023;

public class Day4
{
    private const string ExampleInput = """
Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11
""";
    
    [Fact]
    public void Example()
    {
        var inputLines = ExampleInput.Split(Environment.NewLine);

        var sum = 0;
        var cards = inputLines.Select(Card.Parse).ToArray();
        foreach (var card in cards)
        {
            var winningCount = card.ChosenNumbers.Intersect(card.WinningNumbers).Count();
            sum += 1 * (int)Math.Pow(2, winningCount - 1);
        }
        
        Assert.Equal(13, sum);
    }
    
    [Fact]
    public void Example2()
    {
        var inputLines = ExampleInput.Split(Environment.NewLine);

        var cardScores = new int[inputLines.Length];
        var cards = inputLines.Select(Card.Parse).ToArray();
        for (var i = 0; i < cards.Length; i++)
        {
            var winningCount = cards[i].ChosenNumbers.Intersect(cards[i].WinningNumbers).Count();

            cardScores[i] = winningCount;
        }

        var cardCount = new int[inputLines.Length];
        for (var j = 0; j < cards.Length; j++)
        {
            for (var k = 0; k < cardCount[j] + 1; k++)
            {
                for (var i = 1; i <= cardScores[j]; i++)
                {
                    cardCount[j + i] += 1;
                }
            }

        }

        Assert.Equal(30, cardCount.Sum() + cards.Length);
    }
    
    [Fact]
    public void Part1()
    {
        var lines = File.ReadLines("Day4Input");

        var sum = 0;
        var cards = lines.Select(Card.Parse).ToArray();
        foreach (var card in cards)
        {
            var winningCount = card.ChosenNumbers.Intersect(card.WinningNumbers).Count();
            sum += 1 * (int)Math.Pow(2, winningCount - 1);
        }
        
        Assert.Equal(20829, sum);
    }
    
    [Fact]
    public void Part2()
    {
        var inputLines = File.ReadLines("Day4Input").ToArray();

        var cardScores = new int[inputLines.Length];
        var cards = inputLines.Select(Card.Parse).ToArray();
        for (var i = 0; i < cards.Length; i++)
        {
            var winningCount = cards[i].ChosenNumbers.Intersect(cards[i].WinningNumbers).Count();

            cardScores[i] = winningCount;
        }

        var cardCount = new int[inputLines.Length];
        for (var j = 0; j < cards.Length; j++)
        {
            for (var k = 0; k < cardCount[j] + 1; k++)
            {
                for (var i = 1; i <= cardScores[j]; i++)
                {
                    cardCount[j + i] += 1;
                }
            }

        }

        Assert.Equal(12648035, cardCount.Sum() + cards.Length);
    }
    

    private readonly record struct Card(FrozenSet<int> WinningNumbers, FrozenSet<int> ChosenNumbers)
    {
        public static Card Parse(string value)
        {
            var semicolonIndex = value.IndexOf(':');
            var pipeIndex = value.IndexOf('|');

            var winning = value[(semicolonIndex+1)..pipeIndex].Trim().Split(" ").Where(x => int.TryParse(x, out _)).Select(int.Parse).ToFrozenSet();
            var chosen = value[(pipeIndex+1)..].Trim().Split(" ").Where(x => int.TryParse(x, out _)).Select(int.Parse).ToFrozenSet();
            return new Card(winning, chosen);
        }
    }
}