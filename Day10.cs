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
    
    [Fact] 
    public void Example()
    {
        var input = ExampleInput.Split(Environment.NewLine);

        var map = new Map(input);
        var result = TracePipe(map, null, 0);
        
        Assert.Equal(7145, result / 2);
    }
    
    [Fact] 
    public void Part1()
    {
        var input = File.ReadLines("Day10Input").ToArray();

        var map = new Map(input);
        var result = TracePipe(map, null, 0);
        
        Assert.Equal(8, result / 2);
    }

    private int TracePipe(Map map, int? wentFrom, int depth)
    {
        int? wf = wentFrom;
        while (true)
        {
            var neighbours = map.PossibleNeighbours();
            for (var index = 0; index < neighbours.Length; index++)
            {
                if(!neighbours[index] || index == wf) continue;

                var newCurrent = map.Go(index);
            
                _testOutputHelper.WriteLine($"Am on {depth} depth and have new current {newCurrent}. Current: {map._currentPosition}");
            
                var newDestinations = PipeElementDestinations(newCurrent);
                for (var i = 0; i < newDestinations.Length; i++)
                {
                    if(!newDestinations[i] || i == AgainstDirection(index)) continue;

                    if (map.Get(i) is StartingPosition) return depth + 2;
                }

                depth += 1;
                wf = AgainstDirection(index);
                break;
            }
        }


        return -1;
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
        
        public Point _currentPosition;
        private readonly Point _startingPosition;

        private char CurrentElement => _map[_currentPosition.Y][_currentPosition.X];
        private char? ElementUp => GetElement(_currentPosition.Y - 1, _currentPosition.X);
        private char? ElementDown => GetElement(_currentPosition.Y + 1, _currentPosition.X);
        private char? ElementLeft => GetElement(_currentPosition.Y, _currentPosition.X - 1);
        private char? ElementRight => GetElement(_currentPosition.Y, _currentPosition.X + 1);

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
            return direction switch
            {
                0 => GoUp(),
                1 => GoDown(),
                2 => GoLeft(),
                3 => GoRight(),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }
        
        public char GoUp()
        {
            _currentPosition = _currentPosition with { Y = _currentPosition.Y - 1 };
            return CurrentElement;
        }
        
        public char GoDown()
        {
            _currentPosition = _currentPosition with { Y = _currentPosition.Y + 1 };
            return CurrentElement;
        }
        
        public char GoLeft()
        {
            _currentPosition = _currentPosition with { X = _currentPosition.X - 1 };
            return CurrentElement;
        }
        
        public char GoRight()
        {
            _currentPosition = _currentPosition with { X = _currentPosition.X + 1 };
            return CurrentElement;
        }

        public bool[] PossibleNeighbours()
        {
            bool HasUp(char? x) => x is SouthWest or SouthEast or Vertical;
            bool HasDown(char? x) => x is NorthEast or NorthWest or Vertical;
            bool HasLeft(char? x) => x is NorthEast or SouthEast or Horizontal;
            bool HasRight(char? x) => x is NorthWest or SouthWest or Horizontal;

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
        }

        public Map(IEnumerable<string> input)
        {
            _map = input.Select(x => x.ToCharArray()).ToArray();
            _startingPosition = CalculateStartingPosition();
            _currentPosition = _startingPosition;
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
