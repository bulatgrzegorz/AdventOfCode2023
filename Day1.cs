namespace adventOfCode2023;

public class Day1
{
    private const string ExampleInput = """
1abc2
pqr3stu8vwx
a1b2c3d4e5f
treb7uchet
""";
    
    private const string ExampleInput2 = """
two1nine
eightwothree
abcone2threexyz
xtwone3four
4nineeightseven2
zoneight234
7pqrstsixteen
""";
    
    [Fact]
    public void Example()
    {
        var lines = ExampleInput.Split(Environment.NewLine);
        var sum = CalculateSum(lines);

        Assert.Equal(142, sum);
    }
    
    [Fact]
    public void Example2()
    {
        var lines = ExampleInput2.Split(Environment.NewLine);
        var sum = lines.Select(LineToNumber).Sum(x => int.Parse($"{x.first}{x.last}"));

        Assert.Equal(281, sum);
    }

    private static int CalculateSum(IEnumerable<string> lines)
    {
        var sum = 0;
        foreach (var line in lines)
        {
            var firstIndex = default(int?);
            var lastIndex = default(int?);

            for (var j = 0; j < line.Length; j++)
            {
                if (!char.IsDigit(line[j]))
                {
                    continue;
                }

                firstIndex ??= j;
                lastIndex = j;
            }

            sum += int.Parse($"{line[firstIndex!.Value]}{line[lastIndex!.Value]}");
        }

        return sum;
    }

    [Fact]
    public void Part1()
    {
        var sum = CalculateSum(File.ReadLines("Day1Input"));
        
        Assert.Equal(55816, sum);
    }
    
    [Theory]
    [InlineData("two1nine", 2, 9)]
    [InlineData("eightwothree", 8, 3)]
    [InlineData("abcone2threexyz", 1, 3)]
    [InlineData("xtwone3four", 2, 4)]
    [InlineData("4nineeightseven2", 4, 2)]
    [InlineData("zoneight234", 1, 4)]
    [InlineData("7pqrstsixteen", 7, 6)]
    [InlineData("eighthree", 8, 3)]
    [InlineData("sevenine", 7, 9)]
    public void LineToNumbers(string value, int first, int last)
    {
        var number = LineToNumber(value);

        Assert.Equal(first, number.first);
        Assert.Equal(last, number.last);
    }
    
    [Fact]
    public void Part2()
    {
        var sum = File.ReadLines("Day1Input").Select(LineToNumber).Sum(x => int.Parse($"{x.first}{x.last}"));

        Assert.Equal(54980, sum);
    }

    private (int first, int last) LineToNumber(string value)
    {
        var currentIndex = 0;
        var firstNumber = default(int?);
        var lastNumber = default(int?);
        while (currentIndex < value.Length)
        {
            if (char.IsDigit(value[currentIndex]))
            {
                var numericValue = (int)char.GetNumericValue(value[currentIndex]);
                
                firstNumber ??= numericValue;
                lastNumber = numericValue;
            }
            else if (StartsWithNumber(value[currentIndex..], out var number))
            {
                firstNumber ??= number;
                lastNumber = number;
            }

            currentIndex += 1;

        }

        return (firstNumber!.Value, lastNumber!.Value);
    }

    private bool StartsWithNumber(string value, out int? number)
    {
        number = default;
        for (var i = 0; i < _numbers.Length; i++)
        {
            if (value.StartsWith(_numbers[i]))
            {
                number = i + 1;
                return true;
            }
        }

        return false;
    }
    

    private readonly string[] _numbers = {
        "one",
        "two",
        "three",
        "four",
        "five",
        "six",
        "seven",
        "eight",
        "nine",
    };
}