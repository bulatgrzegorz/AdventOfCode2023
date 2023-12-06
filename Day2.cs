using System.Drawing;

namespace adventOfCode2023;

public class Day2
{
    private const string ExampleInput = """
Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green
""";
    
    [Fact]
    public void Example()
    {
        var lines = ExampleInput.Split(Environment.NewLine);
        var red = 12;
        var green = 13;
        var blue = 14;
        var games = lines.Select(Game.Parse).ToList();

        var sum = games.Where(x => x.Rounds.TrueForAll(t => t.Red <= red && t.Blue <= blue && t.Green <= green)).Sum(x => x.Index);
        Assert.Equal(8, sum);
    }

    [Fact]
    public void Part1()
    {
        var lines = File.ReadLines("Day2Input");
        var red = 12;
        var green = 13;
        var blue = 14;
        
        var games = lines.Select(Game.Parse).ToList();
        var sum = games.Where(x => x.Rounds.TrueForAll(t => t.Red <= red && t.Blue <= blue && t.Green <= green)).Sum(x => x.Index);
        Assert.Equal(2913, sum);
    }
    
    [Fact]
    public void Part2_Example()
    {
        var lines = ExampleInput.Split(Environment.NewLine);

        var games = lines.Select(Game.Parse).ToList();
        var sum = 0;
        foreach (var game in games)
        {
            sum += Math.Max(game.Rounds.Max(x => x.Red), 1) * 
                   Math.Max(game.Rounds.Max(x => x.Green), 1) * 
                   Math.Max(game.Rounds.Max(x => x.Blue), 1);
        }
        
        Assert.Equal(2286, sum);
    }
    
    [Fact]
    public void Part2()
    {
        var lines = File.ReadLines("Day2Input");

        var games = lines.Select(Game.Parse).ToList();
        var sum = 0;
        foreach (var game in games)
        {
            sum += Math.Max(game.Rounds.Max(x => x.Red), 1) * 
                   Math.Max(game.Rounds.Max(x => x.Green), 1) * 
                   Math.Max(game.Rounds.Max(x => x.Blue), 1);
        }
        
        Assert.Equal(2913, sum);
    }

    public record struct Game(int Index, List<(int Red, int Green, int Blue)> Rounds)
    {
        public static Game Parse(string value)
        {
            var resultRounds = new List<(int, int, int)>();
            var semicolonIndex = value.IndexOf(':');
            var index = int.Parse(value[5..semicolonIndex]);
            var rounds = value[(semicolonIndex+ 1)..].Split(";");
            foreach (var round in rounds)
            {
                var cubes = round.Split(",");
                foreach (var cube in cubes)
                {
                    var choosen = cube.Split(" ");
                    var count = int.Parse(choosen[1]);
                    var color = choosen[2];
                    var red = 0;
                    var green = 0;
                    var blue = 0;
                    if (color == "red")
                    {
                        red = count;
                    }
                    else if(color == "green")
                    {
                        green = count;
                    }
                    else if(color == "blue")
                    {
                        blue = count;
                    }
                    
                    resultRounds.Add((red, green, blue));
                }
            }

            return new Game(index, resultRounds);
        }
    }
}