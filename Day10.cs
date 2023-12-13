using System.Text;
using Xunit.Abstractions;
using static adventOfCode2023.Day10.PipeElements;

namespace adventOfCode2023;

public class Day10
{
    private readonly ITestOutputHelper _testOutputHelper;
    public Day10(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    private const string ExampleInput = """
                                        7-F7-
                                        .FJ|7
                                        SJLL7
                                        |F--J
                                        LJ.LJ
                                        """;

    private const string ExampleInput2 = """
                                         FF7FSF7F7F7F7F7F---7
                                         L|LJ||||||||||||F--J
                                         FL-7LJLJ||||||LJL-77
                                         F--JF--7||LJLJIF7FJ-
                                         L---JF-JLJIIIIFJLJJ7
                                         |F|F-JF---7IIIL7L|7|
                                         |FFJF7L7F-JF7IIL---7
                                         7-L-JL7||F7|L7F-7F7|
                                         L.L7LFJ|||||FJL7||LJ
                                         L7JLJL-JLJLJL--JLJ.L
                                         """;
    
    private const string ExampleInput3 = """
                                         ..........
                                         .S------7.
                                         .|F----7|.
                                         .||....||.
                                         .||....||.
                                         .|L-7F-J|.
                                         .|..||..|.
                                         .L--JL--J.
                                         ..........
                                         """;
    
    [Fact] 
    public void Example()
    {
        var input = ExampleInput.Split(Environment.NewLine);

        var map = new Map(input);
        var result = TracePipe(ref map, null, 0);
        
        map.CountPointsInside();
        
        Assert.Equal(8, result / 2);
    }
    
    [Fact] 
    public void Example2()
    {
        var input = ExampleInput2.Split(Environment.NewLine);

        var map = new Map(input);
        TracePipe(ref map, null, 0);
        
        var pointsInside = map.CountPointsInside();
        
        Assert.Equal(10, pointsInside);
    }
    
    [Fact] 
    public void Example3()
    {
        var input = ExampleInput3.Split(Environment.NewLine);

        var map = new Map(input);
        TracePipe(ref map, null, 0);
        
        var pointsInside = map.CountPointsInside();
        
        Assert.Equal(4, pointsInside);
    }
    
    [Fact] 
    public void Part1()
    {
        var input = File.ReadLines("Day10Input").ToArray();

        var map = new Map(input);
        var result = TracePipe(ref map, null, 0);

        var pointsInside = map.CountPointsInside();
        
        Assert.Equal(7145, result / 2);
        Assert.Equal(445, pointsInside);
    }
    
    private int TracePipe(ref Map map, int? wentFrom, int depth)
    {
        var wf = wentFrom;
        while (true)
        {
            var neighbours = map.PossibleNeighbours();
            for (var index = 0; index < neighbours.Length; index++)
            {
                if(!neighbours[index] || index == wf) continue;

                var newCurrent = map.Go(index);
            
                _testOutputHelper.WriteLine($"Am on {depth} depth and have new current {newCurrent}. Current: {map.CurrentPosition}");
            
                var newDestinations = PipeElementDestinations(newCurrent);
                for (var i = 0; i < newDestinations.Length; i++)
                {
                    if(!newDestinations[i] || i == AgainstDirection(index)) continue;

                    if (map.Get(i) is not StartingPosition) continue;
                    
                    map.Go(i);
                    return depth + 2;
                }

                depth += 1;
                wf = AgainstDirection(index);
                break;
            }
        }
    }

    private int AgainstDirection(int direction)
    {
        return direction switch
        {
            0 => 1,
            1 => 0,
            2 => 3,
            3 => 2,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
    
    private bool[] PipeElementDestinations(char element)
    {
        return element switch
        {
            Vertical => [true, true, false, false],
            Horizontal => [false, false, true, true],
            NorthEast => [true, false, false, true],
            NorthWest => [true, false, true, false],
            SouthEast => [false, true, false, true],
            SouthWest => [false, true, true, false],
            _ => throw new ArgumentOutOfRangeException(nameof(element), element, null)
        };
    }
    
    public static class PipeElements
    {
        public const char Vertical = '|';
        public const char Horizontal = '-';
        public const char NorthEast = 'L';
        public const char NorthWest = 'J';
        public const char SouthWest = '7';
        public const char SouthEast = 'F';
        public const char Ground = '.';
        public const char StartingPosition = 'S';
    }

    private readonly record struct Point(int X, int Y);

    private struct Map
    {
        private readonly char[][] _map;
        private readonly char[,] _pipe;
        
        public Point CurrentPosition;
        private int _wentFromStarting;
        private int _wentToStarting;

        private char CurrentElement => _map[CurrentPosition.Y][CurrentPosition.X];
        private char? ElementUp => GetElement(CurrentPosition.Y - 1, CurrentPosition.X);
        private char? ElementDown => GetElement(CurrentPosition.Y + 1, CurrentPosition.X);
        private char? ElementLeft => GetElement(CurrentPosition.Y, CurrentPosition.X - 1);
        private char? ElementRight => GetElement(CurrentPosition.Y, CurrentPosition.X + 1);

        private char GetStartingElement()
        {
            return (_wentFromStarting, _wentToStarting) switch
            {
                (0, 1) => Vertical,
                (0, 0) => Vertical,
                (0, 2) => NorthEast,
                (0, 3) => NorthWest,
                (1, 0) => Vertical,
                (1, 1) => Vertical,
                (1, 2) => SouthEast,
                (1, 3) => SouthWest,
                (2, 0) => SouthWest,
                (2, 1) => NorthWest,
                (2, 3) => Horizontal,
                (2, 2) => Horizontal,
                (3, 0) => SouthEast,
                (3, 1) => NorthEast,
                (3, 2) => Horizontal,
                (3, 3) => Horizontal,
                _ => throw new ArgumentOutOfRangeException($"Could not find starting element for from: {_wentFromStarting}, to: {_wentToStarting}")
            };
        }

        private char? GetElement(int y, int x)
        {
            try
            {
                return _map[y][x];
            }
            catch
            {
                return null;
            }
        }

        public char? Get(int direction)
        {
            return direction switch
            {
                0 => ElementUp,
                1 => ElementDown,
                2 => ElementLeft,
                3 => ElementRight,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }
        public char Go(int direction)
        {
            if (CurrentElement == StartingPosition)
            {
                _wentFromStarting = direction;
            }
            
            var currentElement = direction switch
            {
                0 => GoUp(),
                1 => GoDown(),
                2 => GoLeft(),
                3 => GoRight(),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };

            if (currentElement == StartingPosition)
            {
                _wentToStarting = direction;
            }
            
            _pipe[CurrentPosition.Y, CurrentPosition.X] = currentElement;
            return currentElement;
        }

        private char GoUp()
        {
            CurrentPosition = CurrentPosition with { Y = CurrentPosition.Y - 1 };
            return CurrentElement;
        }

        private char GoDown()
        {
            CurrentPosition = CurrentPosition with { Y = CurrentPosition.Y + 1 };
            return CurrentElement;
        }

        private char GoLeft()
        {
            CurrentPosition = CurrentPosition with { X = CurrentPosition.X - 1 };
            return CurrentElement;
        }

        private char GoRight()
        {
            CurrentPosition = CurrentPosition with { X = CurrentPosition.X + 1 };
            return CurrentElement;
        }

        public bool[] PossibleNeighbours()
        {
            return CurrentElement switch
            {
                StartingPosition => [HasUp(ElementUp), HasDown(ElementDown), HasLeft(ElementLeft), HasRight(ElementRight)],
                Vertical => [HasUp(ElementUp), HasDown(ElementDown), false, false],
                Horizontal => [false, false, HasLeft(ElementLeft), HasRight(ElementRight)],
                NorthEast => [HasUp(ElementUp), false, false, HasRight(ElementRight)],
                NorthWest => [HasUp(ElementUp), false, HasLeft(ElementLeft), false],
                SouthEast => [false, HasDown(ElementDown), false, HasRight(ElementRight)],
                SouthWest => [false, HasDown(ElementDown), HasLeft(ElementLeft), false],
                _ => throw new ArgumentOutOfRangeException($"There is no mapping for current element: {CurrentElement}")
            };

            bool HasRight(char? x) => x is NorthWest or SouthWest or Horizontal;
            bool HasLeft(char? x) => x is NorthEast or SouthEast or Horizontal;
            bool HasDown(char? x) => x is NorthEast or NorthWest or Vertical;
            bool HasUp(char? x) => x is SouthWest or SouthEast or Vertical;
        }

        public Map(IEnumerable<string> input)
        {
            _map = input.Select(x => x.ToCharArray()).ToArray();
            _pipe = new char[_map.Length, _map[0].Length];
            var startingPosition = CalculateStartingPosition();
            CurrentPosition = startingPosition;

            _pipe[startingPosition.Y, startingPosition.X] = StartingPosition;
        }

        public int CountPointsInside()
        {
            var points = 0;
            for (var i = 0; i < _pipe.GetLength(0); i++)
            {
                for (var j = 0; j < _pipe.GetLength(1); j++)
                {
                    if (_pipe[i, j] != '\0') continue;
                    
                    if (RayTrace(i, j))
                    {
                        points++;
                    }
                }
            }

            return points;
        }

        private bool RayTrace(int y, int x)
        {
            var sum = 0;
            for (int yi = y + 1, xi = x + 1; yi < _pipe.GetLength(0) && xi < _pipe.GetLength(1); yi++, xi++)
            {
                if (_pipe[yi, xi] is Horizontal or Vertical or NorthWest or SouthEast)
                {
                    sum++;
                }
                else if (_pipe[yi, xi] is StartingPosition)
                {
                    if (GetStartingElement() is Horizontal or Vertical or NorthWest or SouthEast)
                    {
                        sum++;
                    }
                }
            }

            return sum % 2 != 0;
        }

        private Point CalculateStartingPosition()
        {
            for (var y = 0; y < _map.Length; y++)
            {
                var mapLine = _map[y];
                for (var x = 0; x < mapLine.Length; x++)
                {
                    if (mapLine[x] == StartingPosition) return new Point(x, y);
                }
            }

            throw new Exception("Could not find starting position in given map");
        }
    }
}
