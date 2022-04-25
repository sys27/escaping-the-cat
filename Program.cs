using System;
using static System.Console;

public readonly struct Position : IEquatable<Position>
{
    public static readonly Position Zero = new Position(0, 0);

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static bool operator ==(Position left, Position right)
        => left.Equals(right);

    public static bool operator !=(Position left, Position right)
        => !left.Equals(right);

    public bool Equals(Position other)
        => X == other.X && Y == other.Y;

    public override bool Equals(object obj)
        => obj is Position other && Equals(other);

    public override int GetHashCode()
        => HashCode.Combine(X, Y);

    public double GetRadius()
        => Math.Sqrt(X * X + Y * Y);

    public double GetDistance(Position from)
        => Math.Sqrt(Math.Pow(from.X - X, 2) + Math.Pow(from.Y - Y, 2));

    public double GetDistance()
        => GetDistance(new Position(0, 0));

    public double DotProduct(Position other)
        => X * other.X + Y * other.Y;

    public Position GetOpposite()
        => new Position(-X, -Y);

    public int X { get; }

    public int Y { get; }
}

class Player
{
    static void Main(string[] args)
    {
        const double TOLERANCE = 7.0;

        var catSpeed = int.Parse(ReadLine());
        const int mouseSpeed = 10;
        const int poolRadius = 500;
        const int attackRange = 80;
        const double poolLength = 2 * Math.PI * poolRadius;
        const double poolRadiusWithAttackRange = (poolLength + attackRange) / (2 * Math.PI);
        var speedRatio = mouseSpeed / (double)catSpeed;
        var safeRadius = poolRadiusWithAttackRange * speedRatio;
        Error.WriteLine($"Safe Radius: {safeRadius}");

        var escape = false;
        var escapePosition = new Position(0, 0);

        // game loop
        while (true)
        {
            var inputs = ReadLine().Split(' ');
            var mouseX = int.Parse(inputs[0]);
            var mouseY = int.Parse(inputs[1]);
            var catX = int.Parse(inputs[2]);
            var catY = int.Parse(inputs[3]);
            var mouse = new Position(mouseX, mouseY);
            var cat = new Position(catX, catY);

            Error.WriteLine($"Mouse: {mouseX} {mouseY}");
            Error.WriteLine($"Cat: {catX} {catY} {catSpeed}");

            if (!escape)
            {
                var catDistance = cat.GetDistance();
                var catUX = catX / catDistance;
                var catUY = catY / catDistance;

                var catSafeX = -(int)Math.Ceiling(catUX * safeRadius);
                var catSafeY = -(int)Math.Ceiling(catUY * safeRadius);

                var mouseDistance = mouse.GetDistance();

                var angle = 0.0;
                if (mouse == Position.Zero)
                {
                    angle = Math.PI;
                }
                else
                {
                    angle = Math.Acos(cat.DotProduct(mouse) / (cat.GetDistance() * mouse.GetDistance()));
                }

                var angleDegrees = angle * 180 / Math.PI;

                Error.WriteLine($"MD: {mouseDistance}");
                Error.WriteLine($"SR: {safeRadius}");
                Error.WriteLine($"A: {angleDegrees}");

                if (Math.Abs(safeRadius - mouseDistance) > TOLERANCE || angleDegrees < Math.PI)
                {
                    WriteLine($"{catSafeX} {catSafeY}");
                    continue;
                }

                escape = true;
                var escapeX = -(int)Math.Ceiling(catUX * 2 * poolRadius);
                var escapeY = -(int)Math.Ceiling(catUY * 2 * poolRadius);
                escapePosition = new Position(escapeX, escapeY);
            }

            WriteLine($"{escapePosition.X} {escapePosition.Y}");
        }
    }
}