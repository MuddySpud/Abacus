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

static Dictionary<string, int> Transform2(Point point) => point switch
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
        TaskStatus.WAITING => WorkplaceAssociatedLevelTypes.Warning,
        _ => WorkplaceAssociatedLevelTypes.Untyped,
    },
    { X: var x, Y: var y } => new Dictionary<string, int>(),
};
