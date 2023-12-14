namespace adventOfCode2023;

public class Day11
{
    private const string ExampleInput = """
                                        ...#......
                                        .......#..
                                        #.........
                                        ..........
                                        ......#...
                                        .#........
                                        .........#
                                        ..........
                                        .......#..
                                        #...#.....
                                        """;
    
    [Fact] 
    public void Example()
    {
        var input = ExampleInput.Split(Environment.NewLine);
        
        var distance = GetResult(input);

        Assert.Equal(374, distance);
    }
    
    [Fact] 
    public void Example2()
    {
        var input = ExampleInput.Split(Environment.NewLine);
        
        var distance = GetResult(input, 100);

        Assert.Equal(8410, distance);
    }
    
    [Fact] 
    public void Part1()
    {
        var input = File.ReadLines("Day11Input").ToArray();

        var distance = GetResult(input);

        Assert.Equal(9742154, distance);
    }
    
    [Fact] 
    public void Part2()
    {
        var input = File.ReadLines("Day11Input").ToArray();

        var distance = GetResult(input, 1000000);

        Assert.Equal(411142919886, distance);
    }

    private long GetResult(string[] input, int expansionRate = 2)
    {
        var map = input.Select(x => x.ToCharArray()).ToArray();
        var emptyRows = GetEmptyRows(map).ToArray();
        var emptyColumns = GetEmptyColumns(map).ToArray();
        var galaxies = GetGalaxies(map).ToArray();
        var pairs = GetGalaxiesPairs(galaxies);

        long distance = 0;
        foreach (var ((fy, fx), (sy, sx)) in pairs)
        {
            var ly = fy > sy ? sy : fy;
            var hy = fy > sy ? fy : sy;

            for (int y = ly; y < hy; y++)
            {
                if (emptyRows.Contains(y))
                {
                    distance += expansionRate;
                }
                else
                {
                    distance++;
                }
            }
            
            var lx = fx > sx ? sx : fx;
            var hx = fx > sx ? fx : sx;

            for (int x = lx; x < hx; x++)
            {
                if (emptyColumns.Contains(x))
                {
                    distance += expansionRate;
                }
                else
                {
                    distance++;
                }
            }
        }

        return distance;
    }

    private (Point first, Point second)[] GetGalaxiesPairs(Point[] galaxies)
    {
        return galaxies.SelectMany((_, i) => galaxies.Skip(i + 1), (x, y) => (x, y)).ToArray();
    }

    private IEnumerable<Point> GetGalaxies(char[][] map)
    {
        for (var iy = 0; iy < map.Length; iy++)
        {
            for (var ix = 0; ix < map[iy].Length; ix++)
            {
                if (map[iy][ix] == '#') yield return new Point(iy, ix);
            }
        }
    }
    
    private IEnumerable<int> GetEmptyRows(char[][] map)
    {
        for (var i = 0; i < map.Length; i++)
        {
            if (map[i].All(x => x == '.'))
            {
                yield return i;
            }
        }
    }
    
    private IEnumerable<int> GetEmptyColumns(char[][] map)
    {
        for (var ix = 0; ix < map[0].Length; ix++)
        {
            var allColumnsEmpty = map.All(t => t[ix] == '.');

            if (allColumnsEmpty)
            {
                yield return ix;
            }
        }
    }

    private readonly record struct Point(int Y, int X);
}