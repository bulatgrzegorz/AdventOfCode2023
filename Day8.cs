using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Threading.Channels;
using Xunit.Abstractions;

namespace adventOfCode2023;

public class Day8
{
    private readonly ITestOutputHelper _testOutputHelper;
    public Day8(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    private const string ExampleInput1 = """
RL

AAA = (BBB, CCC)
BBB = (DDD, EEE)
CCC = (ZZZ, GGG)
DDD = (DDD, DDD)
EEE = (EEE, EEE)
GGG = (GGG, GGG)
ZZZ = (ZZZ, ZZZ)
""";
    
    private const string ExampleInput2 = """
LLR

AAA = (BBB, BBB)
BBB = (AAA, ZZZ)
ZZZ = (ZZZ, ZZZ)
""";
    
    [Fact] 
    public void Example()
    {
        var input = ExampleInput1.Split(Environment.NewLine);
        var steps = CountSteps1(input);

        Assert.Equal(2, steps);
    }
    
    [Fact] 
    public void Example2()
    {
        var input = ExampleInput2.Split(Environment.NewLine);
        var steps = CountSteps1(input);

        Assert.Equal(6, steps);
    }
    
    [Fact] 
    public void Part1()
    {
        var inputLines = File.ReadLines("Day8Input").ToArray();
        var steps = CountSteps1(inputLines);

        Assert.Equal(19951, steps);
    }
    
    [Fact] 
    public void Part2()
    {
        var input = File.ReadLines("Day8Input").ToArray();
        var instructions = input[0].Trim().ToCharArray();
        var nodeNetwork = input[2..].ToDictionary(line => line[..3], line => (line[7..10], line[12..15]));
        var r = new List<long>();
        foreach (var startingNode in nodeNetwork.Select(x => x.Key).Where(x => x.EndsWith('A')))
        {
            r.Add(CountSteps2(instructions, nodeNetwork, startingNode));
        }

        var a = LCM(r.ToArray());

        Assert.Equal(16342438708751, a);
    }
    
    static long LCM(long[] numbers)
    {
        return numbers.Aggregate(LCM);
    }
    static long LCM(long a, long b)
    {
        return Math.Abs(a * b) / GCD(a, b);
    }
    static long GCD(long a, long b)
    {
        return b == 0 ? a : GCD(b, a % b);
    }
    
    private int CountSteps2(char[] instructions, Dictionary<string, (string, string)> nodeNetwork, string startingNode)
    {
        var steps = 0;

        foreach (var instruction in RepeatForever(instructions))
        {
            steps++;

            startingNode = instruction == 'R' ? nodeNetwork[startingNode].Item2 : nodeNetwork[startingNode].Item1;
            if (!startingNode.EndsWith('Z'))
            {
                continue;
            }

            break;
        }
        
        return steps;
    }
    
    private int CountSteps1(string[] input)
    {
        var instructions = input[0].Trim().ToCharArray();
        var nodeNetwork = input[2..].ToDictionary(line => line[..3], line => (line[7..10], line[12..15]));

        var steps = 0;
        var startingNode = "AAA";
        var currentNode = startingNode;

        foreach (var instruction in RepeatForever(instructions))
        {
            steps++;

            currentNode = instruction == 'R' ? nodeNetwork[currentNode].Item2 : nodeNetwork[currentNode].Item1;

            if (currentNode != "ZZZ") continue;
            
            break;
        }

        return steps;
    }
    
    private static IEnumerable<T> RepeatForever<T>(T[] sequence, int? max = null)
    {
        var i = 0;
        while (!max.HasValue || i < max)
        {
            // ReSharper disable once PossibleMultipleEnumeration
            foreach (var item in sequence)
            {
                yield return item;
            }

            i++;
        }
        // ReSharper disable once IteratorNeverReturns
    }
}