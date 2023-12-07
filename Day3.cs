namespace adventOfCode2023;

public class Day3
{
    private const string ExampleInput = """
467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..
""";

    [Fact]
    public void Example()
    {
        var inputLines = ExampleInput.Split(Environment.NewLine);
        var sum = CalculateAdjacentNumbersSum(inputLines);

        Assert.Equal(4361, sum);
    }
    
    [Fact]
    public void Example2()
    {
        var inputLines = ExampleInput.Split(Environment.NewLine);
        var sum = CalculateGearRatio(inputLines);

        Assert.Equal(467835, sum);
    }

    private static int CalculateGearRatio(IEnumerable<string> inputLines)
    {
        var sum = 0;

        var lines = inputLines.Select(ParseLine).ToArray();

        for (int i = 0; i < lines.Length; i++)
        {
            foreach (var lineSymbol in lines[i].Symbols)
            {
                if(lineSymbol.Symbol != '*') continue;

                var adjacentNumbersToSymbol = new List<int>();
                
                if (i > 0)
                {
                    adjacentNumbersToSymbol.AddRange(
                        lines[i - 1].LineNumbers
                            .Where(x => IsSymbolAdjacent(lineSymbol, i, x, i - 1))
                            .Select(x => x.Number));
                }

                if (adjacentNumbersToSymbol.Count > 2) continue;

                if (i + 1 < lines.Length)
                {
                    adjacentNumbersToSymbol.AddRange(
                        lines[i + 1].LineNumbers
                            .Where(x => IsSymbolAdjacent(lineSymbol, i, x, i + 1))
                            .Select(x => x.Number));
                }
                
                if (adjacentNumbersToSymbol.Count > 2) continue;

                adjacentNumbersToSymbol.AddRange(
                    lines[i].LineNumbers
                        .Where(x => IsSymbolAdjacent(lineSymbol, i, x, i))
                        .Select(x => x.Number));

                if (adjacentNumbersToSymbol.Count == 2)
                {
                    sum += adjacentNumbersToSymbol[0] * adjacentNumbersToSymbol[1];
                }
            }
        }

        return sum;
    }
    
    private static int CalculateAdjacentNumbersSum(IEnumerable<string> inputLines)
    {
        var sum = 0;

        var lines = inputLines.Select(ParseLine).ToArray();

        for (int i = 0; i < lines.Length; i++)
        {
            foreach (var lineNumber in lines[i].LineNumbers)
            {
                if (i > 0)
                {
                    if (lines[i - 1].Symbols.Any(x => IsSymbolAdjacent(x, i - 1, lineNumber, i)))
                    {
                        sum += lineNumber.Number;
                        continue;
                    }
                }

                if (i + 1 < lines.Length)
                {
                    if (lines[i + 1].Symbols.Any(x => IsSymbolAdjacent(x, i + 1, lineNumber, i)))
                    {
                        sum += lineNumber.Number;
                        continue;
                    }
                }

                if (lines[i].Symbols.Any(x => IsSymbolAdjacent(x, i, lineNumber, i)))
                {
                    sum += lineNumber.Number;
                }
            }
        }

        return sum;
    }

    [Fact]
    public void Part1()
    {
        var lines = File.ReadLines("Day3Input");
        var sum = CalculateAdjacentNumbersSum(lines);
        Assert.Equal(529618, sum);
    }
    
    [Fact]
    public void Part2()
    {
        var lines = File.ReadLines("Day3Input");
        var sum = CalculateGearRatio(lines);
        Assert.Equal(77509019, sum);
    }
    
    [InlineData(
        ".1.....", 
        "*......",
        1)]
    [InlineData(
        ".1.....", 
        ".*.....",
        1)]
    [InlineData(
        ".1.....", 
        "..*....",
        1)]
    [InlineData(
        "*......",
        ".1.....",
        1)]
    [InlineData(
        ".*.....",
        ".1.....",
        1)]
    [InlineData(
        "..*....",
        ".1.....",
        1)]
    [InlineData(
        "*1.....", 
        ".......",
        1)]
    [InlineData(
        ".1*....", 
        ".......",
        1)]
    [InlineData(
        ".1.1...", 
        "..*....",
        2)]
    [InlineData(
        ".1*1...", 
        ".......",
        2)]
    [InlineData(
        "..*1*..", 
        "..***..",
        1)]
    [InlineData(
        "..*1*..", 
        ".1***1.",
        3)]
    [InlineData(
        "1.*1*.1", 
        ".*.*.*.",
        3)]
    [InlineData(
        ".*.*.*.",
        "1.*1*.1",
        3)]
    [Theory]
    public void Adjacent_Tests(string line1, string line2, int expected)
    {
        var sum = CalculateAdjacentNumbersSum(new []{line1, line2});
        Assert.Equal(expected, sum);
        //11825
    }

    private static bool IsSymbolAdjacent(LineSymbol symbol, int symbolIndexLine, LineNumber number, int numberIndexLine)
    {
        if (symbolIndexLine == numberIndexLine)
        {
            return symbol.Index + 1 == number.StartIndex || symbol.Index - 1 == number.EndIndex;
        }

        return symbol.Index + 1 >= number.StartIndex && symbol.Index - 1 <= number.EndIndex;
    }
    
    private static Line ParseLine(string line)
    {
        var lineNumbers = new List<LineNumber>();
        var lineSymbols = new List<LineSymbol>();

        int? startIndex = default;
        int? endIndex = default;
        var currentIndex = 0;
        var hasNumber = false;
        foreach (var @char in line)
        {
            if (char.IsDigit(@char))
            {
                startIndex ??= currentIndex;
                endIndex = currentIndex;
                hasNumber = true;
            }
            else if(hasNumber)
            {
                var number = int.Parse(line[startIndex!.Value..(endIndex!.Value + 1)]);
                lineNumbers.Add(new LineNumber(startIndex.Value, endIndex.Value, number));

                startIndex = default;
                endIndex = default;
                hasNumber = false;
            }

            if (!hasNumber && @char != '.')
            {
                lineSymbols.Add(new LineSymbol(currentIndex, @char));
            }

            currentIndex++;
        }

        if (hasNumber)
        {
            var number = int.Parse(line[startIndex!.Value..(endIndex!.Value + 1)]);
            lineNumbers.Add(new LineNumber(startIndex.Value, endIndex.Value, number));
        }

        return new Line(lineSymbols.ToArray(), lineNumbers.ToArray());
    }

    private readonly record struct Line(LineSymbol[] Symbols, LineNumber[] LineNumbers);
    private readonly record struct LineSymbol(int Index, char Symbol);    
    private readonly record struct LineNumber(int StartIndex, int EndIndex, int Number);
}