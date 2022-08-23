

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

    default:
        Console.WriteLine($"Measured value is {measurement}.");
        break;
}

switch (measurement)
{
    case < 0:
    case > 100:
        Console.WriteLine($"Measured value is {measurement}; out of an acceptable range.");
        break;

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