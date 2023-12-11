using System.Collections.Immutable;

namespace adventOfCode2023;

public class Day6
{
    private const string ExampleInput = """
Time:      7  15   30
Distance:  9  40  200
""";

    private const string ExamplePart2Input = """
Time:      71530
Distance:  940200
""";

    private const string Part2Input = """
Time:       60808676
Distance:   601116315591300
""";
    
    [Fact] 
    public void Example()
    {
        var inputLines = ExampleInput.Split(Environment.NewLine);
        var times= inputLines[0][(inputLines[0].IndexOf(':') + 1)..].Trim().Split(' ').Where(x => int.TryParse(x, out _)).Select(int.Parse).ToImmutableArray();
        var distances= inputLines[1][(inputLines[1].IndexOf(':') + 1)..].Trim().Split(' ').Where(x => int.TryParse(x, out _)).Select(int.Parse).ToImmutableArray();

        var result = 1;
        for (int i = 0; i < times.Length; i++)
        {
            var ind = CalculateNumberOfWaysToWin(times[i], distances[i]);
            result *= ind;
        }
        
        Assert.Equal(352, result);
    }
    
    [Fact]
    public void Example2()
    {
        var inputLines = ExamplePart2Input.Split(Environment.NewLine);
        var times= inputLines[0][(inputLines[0].IndexOf(':') + 1)..].Trim().Split(' ').Where(x => int.TryParse(x, out _)).Select(int.Parse).ToImmutableArray();
        var distances= inputLines[1][(inputLines[1].IndexOf(':') + 1)..].Trim().Split(' ').Where(x => int.TryParse(x, out _)).Select(int.Parse).ToImmutableArray();

        var result = 1;
        for (int i = 0; i < times.Length; i++)
        {
            var ind = CalculateNumberOfWaysToWin(times[i], distances[i]);
            result *= ind;
        }
        
        Assert.Equal(71503, result);
    }
    
    [Fact]
    public void Part1()
    {
        var inputLines = File.ReadLines("Day6Input").ToArray();

        var times= inputLines[0][(inputLines[0].IndexOf(':') + 1)..].Trim().Split(' ').Where(x => int.TryParse(x, out _)).Select(int.Parse).ToImmutableArray();
        var distances= inputLines[1][(inputLines[1].IndexOf(':') + 1)..].Trim().Split(' ').Where(x => int.TryParse(x, out _)).Select(int.Parse).ToImmutableArray();

        var result = 1;
        for (int i = 0; i < times.Length; i++)
        {
            var ind = CalculateNumberOfWaysToWin(times[i], distances[i]);
            result *= ind;
        }
        
        Assert.Equal(1255625, result);
    }
    
    [Fact]
    public void Part2()
    {
        var inputLines = Part2Input.Split(Environment.NewLine);
        var times= inputLines[0][(inputLines[0].IndexOf(':') + 1)..].Trim().Split(' ').Where(x => long.TryParse(x, out _)).Select(long.Parse).ToImmutableArray();
        var distances= inputLines[1][(inputLines[1].IndexOf(':') + 1)..].Trim().Split(' ').Where(x => long.TryParse(x, out _)).Select(long.Parse).ToImmutableArray();

        var result = 1;
        for (int i = 0; i < times.Length; i++)
        {
            var ind = CalculateNumberOfWaysToWin(times[i], distances[i]);
            result *= ind;
        }
        
        Assert.Equal(35961505, result);
    }

    private int CalculateNumberOfWaysToWin(long time, long recordDist)
    {
        var sqrtInternal = time * time - 4 * recordDist;
        var sqrt = Math.Sqrt(sqrtInternal);
        var cross1 = (time + sqrt) / 2;
        var cross2 = (time - sqrt) / 2;
        
        return Math.Abs((cross2 % 1 == 0 ? (int)(cross2 - 1) : (int)Math.Floor(cross2)) - (cross1 % 1 == 0 ? (int)(cross1 + 1) : (int)Math.Ceiling(cross1)) + 1);
    }
}