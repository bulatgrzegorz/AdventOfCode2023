namespace adventOfCode2023;

public class Day7
{
    private const string ExampleInput = """
32T3K 765
T55J5 684
KK677 28
KTJJT 220
QQQJA 483
""";
    
    [Fact] 
    public void Example()
    {
        var input = ExampleInput.Split(Environment.NewLine).Select(x => x.Trim().Split(' ')).Select(x => new {deck = new Deck(x[0].ToCharArray()), bid = int.Parse(x[1])});
        var result = input.OrderBy(x => x.deck).Select((x, index) => x.bid * (index + 1)).Sum();
        
        Assert.Equal(6440, result);
    }
    
    [Fact] 
    public void Example2()
    {
        var input = ExampleInput.Split(Environment.NewLine).Select(x => x.Trim().Split(' ')).Select(x => new {deck = new Deck2(x[0].ToCharArray()), bid = int.Parse(x[1])});
        var result = input.OrderBy(x => x.deck).Select((x, index) => x.bid * (index + 1)).Sum();
        
        Assert.Equal(5905, result);
    }
    
    [Fact] 
    public void Part1()
    {
        var inputLines = File.ReadLines("Day7Input").ToArray();
        
        var input = inputLines.Select(x => x.Trim().Split(' ')).Select(x => new {deck = new Deck(x[0].ToCharArray()), bid = int.Parse(x[1])});
        var result = input.OrderBy(x => x.deck).Select((x, index) => x.bid * (index + 1)).Sum();
        
        Assert.Equal(250347426, result);
    }
        
    [Fact] 
    public void Part2()
    {
        var inputLines = File.ReadLines("Day7Input").ToArray();
        
        var input = inputLines.Select(x => x.Trim().Split(' ')).Select(x => new {deck = new Deck2(x[0].ToCharArray()), bid = int.Parse(x[1])});
        var result = input.OrderBy(x => x.deck).Select((x, index) => x.bid * (index + 1)).Sum();
        
        Assert.Equal(251224870, result);
    }


    private class Deck : IComparable<Deck>
    {
        private enum HandType
        {
            HighCard = 0,
            OnePair,
            TwoPair,
            Three,
            FullHouse,
            Four,
            Five
        }

        private readonly char[] _hand;
        private readonly HandType _handType;
        
        public Deck(char[] hand)
        {
            _hand = hand;
            _handType = CalculateHandType();
        }

        private HandType CalculateHandType()
        {
            var havePair = false;
            var haveThree = false;
            
            var groupedHand = _hand.GroupBy(x => x).Select(x => new {Card = x.Key, Count = x.Count()});
            foreach (var group in groupedHand.OrderByDescending(x => x.Count))
            {
                switch (group.Count)
                {
                    case 5:
                        return HandType.Five;
                    case 4:
                        return HandType.Four;
                    case 3:
                        haveThree = true;
                        continue;
                    case 2:
                        if (havePair)
                        {
                            return HandType.TwoPair;
                        }
                        havePair = true;
                        continue;
                }
            }

            if (havePair && haveThree)
            {
                return HandType.FullHouse;
            }

            if (haveThree)
            {
                return HandType.Three;
            }

            return havePair ? HandType.OnePair : HandType.HighCard;
        }
        
        private readonly Dictionary<char, int> _cardStrength = new()
        {
            { 'A', 13 },
            { 'K', 12 },
            { 'Q', 11 },
            { 'J', 10 },
            { 'T', 9 },
            { '9', 8 },
            { '8', 7 },
            { '7', 6 },
            { '6', 5 },
            { '5', 4 },
            { '4', 3 },
            { '3', 2 },
            { '2', 1 },
        };

        public int CompareTo(Deck? other)
        {
            ArgumentNullException.ThrowIfNull(other);
            
            if (_handType > other._handType)
            {
                return 1;
            }

            if (_handType < other._handType)
            {
                return -1;
            }

            for (var i = 0; i < _hand.Length; i++)
            {
                if (_cardStrength[_hand[i]] > _cardStrength[other._hand[i]])
                {
                    return 1;
                }
                
                if (_cardStrength[_hand[i]] < _cardStrength[other._hand[i]])
                {
                    return -1;
                }
            }

            return 0;
        }
    }
    
    private class Deck2 : IComparable<Deck2>
    {
        private enum HandType
        {
            HighCard = 0,
            OnePair,
            TwoPair,
            Three,
            FullHouse,
            Four,
            Five
        }

        private readonly char[] _hand;
        private readonly HandType _handType;
        
        public Deck2(char[] hand)
        {
            _hand = hand;
            _handType = CalculateHandType();
        }

        private HandType CalculateHandType()
        {
            var groupedHand = _hand.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
            
            HandType CalculateHandTypeInternal()
            {
                var havePair = false;
                var haveThree = false;
            
                
                foreach (var group in groupedHand.OrderByDescending(x => x.Value))
                {
                    if (group.Key == 'J') continue;
                
                    switch (group.Value)
                    {
                        case 5:
                            return HandType.Five;
                        case 4:
                            return HandType.Four;
                        case 3:
                            haveThree = true;
                            continue;
                        case 2:
                            if (havePair)
                            {
                                return HandType.TwoPair;
                            }
                            havePair = true;
                            continue;
                    }
                }

                if (havePair && haveThree)
                {
                    return HandType.FullHouse;
                }

                if (haveThree)
                {
                    return HandType.Three;
                }

                return havePair ? HandType.OnePair : HandType.HighCard;
            }

            var calculatedType = CalculateHandTypeInternal();

            if (!groupedHand.TryGetValue('J', out var jokers))
            {
                return calculatedType;
            }

            return (calculatedType, jokers) switch
            {
                (HandType.Five, _) => HandType.Five,
                (HandType.Four, _) => HandType.Five,
                (HandType.Three, 1) => HandType.Four,
                (HandType.Three, 2) => HandType.Five,
                (HandType.OnePair, 1) => HandType.Three,
                (HandType.OnePair, 2) => HandType.Four,
                (HandType.OnePair, 3) => HandType.Five,
                (HandType.TwoPair, _) => HandType.FullHouse,
                (HandType.HighCard, 1) => HandType.OnePair,
                (HandType.HighCard, 2) => HandType.Three,
                (HandType.HighCard, 3) => HandType.Four,
                (HandType.HighCard, _) => HandType.Five,
                _ => throw new ArgumentOutOfRangeException($"Type: {calculatedType} jokers: {jokers}")
            };
        }
        
        private readonly Dictionary<char, int> _cardStrength = new()
        {
            { 'A', 13 },
            { 'K', 12 },
            { 'Q', 11 },
            { 'T', 9 },
            { '9', 8 },
            { '8', 7 },
            { '7', 6 },
            { '6', 5 },
            { '5', 4 },
            { '4', 3 },
            { '3', 2 },
            { '2', 1 },
            { 'J', 0 },
        };

        public int CompareTo(Deck2? other)
        {
            ArgumentNullException.ThrowIfNull(other);
            
            if (_handType > other._handType)
            {
                return 1;
            }

            if (_handType < other._handType)
            {
                return -1;
            }

            for (var i = 0; i < _hand.Length; i++)
            {
                if (_cardStrength[_hand[i]] > _cardStrength[other._hand[i]])
                {
                    return 1;
                }
                
                if (_cardStrength[_hand[i]] < _cardStrength[other._hand[i]])
                {
                    return -1;
                }
            }

            return 0;
        }
    }
}