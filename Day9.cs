namespace adventOfCode2023;

public class Day9
{
    private const string ExampleInput1 = """
0 3 6 9 12 15
1 3 6 10 15 21
10 13 16 21 30 45
""";

    [Fact] 
    public void Example()
    {
        var input = ExampleInput1.Split(Environment.NewLine);
            
        var sum = CalculateSum(input);

        Assert.Equal(114, sum);
    }
    
    [Fact] 
    public void Example2()
    {
        var input = ExampleInput1.Split(Environment.NewLine);
            
        var sum = CalculateSum2(input);

        Assert.Equal(2, sum);
    }
    
    [Fact] 
    public void Part1()
    {
        var inputLines = File.ReadLines("Day9Input").ToArray();

        var sum = CalculateSum(inputLines);

        Assert.Equal(1993300041, sum);
    }
    
    [Fact] 
    public void Part2()
    {
        var inputLines = File.ReadLines("Day9Input").ToArray();

        var sum = CalculateSum2(inputLines);

        Assert.Equal(1038, sum);
    }

    private static int CalculateSum(string[] input)
    {
        var lines = input.Select(x => x.Trim().Split(' ').Select(int.Parse).ToArray()).ToList();

        var sum = 0;
        foreach (var line in lines)
        {
            var levels = new List<int[]>();
            var level = line.ToArray();
            while (level.Any(x => x != 0))
            {
                levels.Add(level);
                level = level.Zip(level[1..], (x, y) => y - x).ToArray();
            }

            var expected = 0;
            for (var i = levels.Count - 1; i >= 0; i--)
            {
                expected += levels[i][^1];
            }

            sum += expected;
        }

        return sum;
    }

    private static int CalculateSum2(string[] input)
    {
        var lines = input.Select(x => x.Trim().Split(' ').Select(int.Parse).ToArray()).ToList();

        var sum = 0;
        foreach (var line in lines)
        {
            var levels = new List<int[]>();
            var level = line.ToArray();
            while (level.Any(x => x != 0))
            {
                levels.Add(level);
                level = level.Zip(level[1..], (x, y) => y - x).ToArray();
            }

            var expected = 0;
            for (var i = levels.Count - 1; i >= 0; i--)
            {
                expected = levels[i][0] - expected;
            }

            sum += expected;
        }

        return sum;
    }
}