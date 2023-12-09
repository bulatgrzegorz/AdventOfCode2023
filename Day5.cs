namespace adventOfCode2023;

public class Day5
{
    private const string ExampleInput = """
seeds: 79 14 55 13

seed-to-soil map:
50 98 2
52 50 48

soil-to-fertilizer map:
0 15 37
37 52 2
39 0 15

fertilizer-to-water map:
49 53 8
0 11 42
42 0 7
57 7 4

water-to-light map:
88 18 7
18 25 70

light-to-temperature map:
45 77 23
81 45 19
68 64 13

temperature-to-humidity map:
0 69 1
1 0 69

humidity-to-location map:
60 56 37
56 93 4
""";

    [Fact]
    public void Example()
    {
        var inputLines = ExampleInput.Split(Environment.NewLine);
        var input = new Input(inputLines.ToList());
        var maps = new MapRanges[7];
        for (var i = 0; i < 7; i++)
        {
            maps[i] = new MapRanges(input.ParseMap(i));
        }
        var seeds = input.Seeds;

        var locations = new List<long>();
        foreach (var seed in seeds)
        {
            var lookFor = seed;
            foreach (var map in maps)
            {
                lookFor = map.Map(lookFor);
            }
            
            locations.Add(lookFor);
        }

        Assert.Equal(35, locations.Min());
    }
    
    [Fact]
    public void Example2()
    {
        var inputLines = ExampleInput.Split(Environment.NewLine);
        var input = new Input(inputLines.ToList());
        var maps = new MapRanges[7];
        for (var i = 0; i < 7; i++)
        {
            maps[i] = new MapRanges(input.ParseMap(i));
        }
        var seeds = input.Seeds2;

        var locations = new List<long>();
        foreach (var seed in seeds)
        {
            var lookFor = seed;
            foreach (var map in maps)
            {
                lookFor = map.Map(lookFor);
            }
            
            locations.Add(lookFor);
        }

        Assert.Equal(46, locations.Min());
    }

    [Fact]
    public void Part1()
    {
        var lines = File.ReadLines("Day5Input");

        var input = new Input(lines.ToList());
        var maps = new MapRanges[7];
        for (var i = 0; i < 7; i++)
        {
            maps[i] = new MapRanges(input.ParseMap(i));
        }
        var seeds = input.Seeds;

        var locations = new List<long>();
        foreach (var seed in seeds)
        {
            var lookFor = seed;
            foreach (var map in maps)
            {
                lookFor = map.Map(lookFor);
            }
            
            locations.Add(lookFor);
        }

        Assert.Equal(4361, locations.Min());
    }
    
    [Fact]
    public void Part2()
    {
        var lines = File.ReadLines("Day5Input");

        var input = new Input(lines.ToList());
        var maps = new MapRanges[7];
        for (var i = 0; i < 7; i++)
        {
            maps[i] = new MapRanges(input.ParseMap(i));
        }
        var seeds = input.Seeds2;

        var locationsMin = long.MaxValue;

        foreach (var seed in seeds)
        {
            var lookFor = seed;
            foreach (var map in maps)
            {
                lookFor = map.Map(lookFor);
            }

            locationsMin = locationsMin < lookFor ? locationsMin : lookFor;
        }

        Assert.Equal(31161857, locationsMin);
    }
    
    private class MapRanges
    {
        private readonly (long destinationStart, long sourceStart, long rangeLength)[] _complexMaps;

        public MapRanges((long destinationStart, long sourceStart, long rangeLength)[] complexMaps)
        {
            _complexMaps = complexMaps;
        }

        public long Map(long id)
        {
            foreach (var (destinationStart, sourceStart, rangeLength) in _complexMaps)
            {
                if (id >= sourceStart && id <= sourceStart + rangeLength) return destinationStart + id - sourceStart;
            }

            return id;
        }
    }

    private class Input
    {
        private readonly List<string> _inputLines;

        public Input(List<string> inputLines)
        {
            _inputLines = inputLines;
        }

        public long[] Seeds => ParseSeedsLine(_inputLines[0]);
        public IEnumerable<long> Seeds2 => ParseSeedsLine2(_inputLines[0]);

        public (long destinationStart, long sourceStart, long rangeLength)[] ParseMap(int mapIndex)
        {
            var indexOfMarker = _inputLines.IndexOf(MapNameIndex[mapIndex]);
            var indexOfMapEnd = _inputLines.FindIndex(indexOfMarker, string.IsNullOrWhiteSpace);
            
            return _inputLines[(indexOfMarker + 1)..(indexOfMapEnd == -1 ? _inputLines.Count : indexOfMapEnd)].Select(ParseNumberLine).Select(x => (x[0], x[1], x[2])).ToArray();
        }

        private static readonly string[] MapNameIndex = {
            "seed-to-soil map:",
            "soil-to-fertilizer map:",
            "fertilizer-to-water map:",
            "water-to-light map:",
            "light-to-temperature map:",
            "temperature-to-humidity map:",
            "humidity-to-location map:",
        };

        private static long[] ParseNumberLine(string value)
        {
            return value.Trim().Split(' ').Select(long.Parse).ToArray();
        }
        
        private static long[] ParseSeedsLine(string value)
        {
            var indexOfSemicolon = value.IndexOf(':');
            return value[(indexOfSemicolon + 1)..].Trim().Split(' ').Select(long.Parse).ToArray();
        }
        
        private static IEnumerable<long> ParseSeedsLine2(string value)
        {
            var indexOfSemicolon = value.IndexOf(':');
            var seeds = value[(indexOfSemicolon + 1)..].Trim().Split(' ').Select(long.Parse).ToArray();
            for (int i = 0; i < seeds.Length; i += 2)
            {
                for (int j = 0; j < seeds[i + 1]; j++)
                {
                    yield return seeds[i] + j;
                }
            }
        }
    }
}