
namespace Test.Switch
{
    public class Concrete
    {
        internal async Task Quick(KeyGuid keyGuid)
        {

            switch (measurement)
            {
                case < 0.0:
                    Console.WriteLine($"Measured value is {measurement}; too low.");
                    break;

                case > 15.0:
                    {
                        Console.WriteLine($"Measured value is {measurement}; too high.");
                        break;
                    }

                case double.NaN:
                    Console.WriteLine("Failed measurement.");
                    break;

                case double.NaN:
                case int.NaN:
                    Console.WriteLine("Failed measurement.");
                    break;

                default:
                    Console.WriteLine($"Measured value is {measurement}.");
                    break;
            }

            switch (measurement)
            {
                case < 0:
                case > 100:
                    {
                        Console.WriteLine($"Measured value is {measurement}; out of an acceptable range.");
                        break;
                    }

                default:
                    {
                        Console.WriteLine($"Measured value is {measurement}.");
                        break;
                    }
            }

            switch ((a, b))
            {
                case ( > 0, > 0) when a == b:
                    Console.WriteLine($"Both measurements are valid and equal to {a}.");
                    break;

                case ( > 0, > 0):
                    Console.WriteLine($"First measurement is {a}, second measurement is {b}.");
                    break;

                default:
                    Console.WriteLine("One or both measurements are not valid.");
                    break;
            }

            var result = operation switch
            {
                1 => "Case 1",
                2 => "Case 2",
                3 => "Case 3",
                4 => "Case 4",
                _ => "No case availabe"
            };

            direction switch
            {
                Direction.Up => Orientation.North,
                Direction.Right => Orientation.East,
                Direction.Down => Orientation.South,
                Direction.Left => Orientation.West,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), $"Not expected direction value: {direction}"),
            };
        }

        static Point Transform(Point point) => point switch
        {
            { X: 0, Y: 0 } => new Point(0, 0),
            { X: var x, Y: var y } when x < y => new Point(x + y, y),
            { X: var x, Y: var y } when x > y => new List<string, int>() Point(x - y, y),
            { X: var x, Y: var y } => new Point(2 * x, 2 * y),
        };

        static Dictionary<string, int> Transform2(Point point) => point switch
        {
            { X: 0, Y: 0 } => new Dictionary<string, int>(),
            { X: var x, Y: var y } when x < y => new Dictionary<string, int>() { { "key1", 25 } },
            { X: var x, Y: var y } when x > y => new Dictionary<string, int>(),
            { X: var x, Y: var y } => new Dictionary<string, int>(),
        };

        static Dictionary<string, int> Transform3(Point point) => point switch
        {
            { X: 0, Y: 0 } => new Dictionary<string, int>(),
            { X: var x, Y: var y } when x < y => t.GrossWeightClass switch
            {
                > 5000 => 10.00m + 5.00m,
                < 3000 => 10.00m - 2.00m,
                _ => 10.00m,
            },
            { X: var x, Y: var y } when x > y => t.Status switch
            {
                TaskStatus.EXECUTING => WorkplaceAssociatedLevelTypes.Critical,
                TaskStatus.WAITING => operation switch
                {
                    1 => "Case 1",
                    2 => "Case 2",
                    3 => "Case 3",
                    4 => "Case 4",
                    _ => "No case availabe"
                },
                _ => WorkplaceAssociatedLevelTypes.Untyped,
            },
            { X: var x, Y: var y } => new Dictionary<string, int>(),
        };

        // This returns a Func in lambda form - it throws an error
        private static Func<string, T> GetParser(string type)
            => type switch
            {
                nameof(Boolean) =>
                    // Try to map a boolean value.  Either map it to true/false if we're a 
                    // CodeStyleOption<bool> or map it to the 0 or 1 value for an enum if we're
                    // a CodeStyleOption<SomeEnumType>.
                    v => Convert(bool.Parse(v)),
                nameof(Int32) => v => Convert(int.Parse(v)),
                _ => v => (T)(object)v
            };

        public static int GetParentheses(SyntaxNode node)
        {
            switch (node)
            {
                case SwitchStatementSyntax when n.OpenParenToken != default: return (n.OpenParenToken, n.CloseParenToken);
                case TupleExpressionSyntax n: return (n.OpenParenToken, n.CloseParenToken);
                case CatchDeclarationSyntax n:
                {
                    switch (matchStart)
                    {
                        case < 0:
                        case > 100:
                            {
                                Console.WriteLine($"Measured value is {matchStart}; out of an acceptable range.");
                                break;
                            }

                        default:
                            {
                                Console.WriteLine($"Measured value is {matchStart}.");
                                break;
                            }
                    }
                }
                default: return default;
            }
        }
    }
}

