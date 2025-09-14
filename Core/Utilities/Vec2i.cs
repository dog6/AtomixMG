
using System;
using System.Xml.XPath;

public struct Vec2i : IEquatable<Vec2i>
{
    public int x;
    public int y;

    public Vec2i(int x = 0, int y = 0)
    {
        this.x = x;
        this.y = y;
    }


    public int Distance(Vec2i otherPoint) => (int)Math.Sqrt((otherPoint.x - this.x) + (otherPoint.y - this.y)); // estimation

    public bool Equals(Vec2i other) => x == other.x && y == other.y;

    public Vec2i Add(Vec2i other) => new Vec2i(this.x + other.x, this.y + other.y);
    public Vec2i Add(int xAdder, int yAdder) => new Vec2i(this.x + xAdder, this.y + yAdder);

    public float Magnitude() => (float)Math.Sqrt(x * x + y * y);

    public Vec2i Normalized()
    {
        float mag = Magnitude();
        if (mag == 0) return new Vec2i(0, 0);
        return new Vec2i((int)Math.Round(x / mag), (int)Math.Round(y / mag));
    }

    public Vec2i Multiply(Vec2i other) => new Vec2i(this.x * other.x, this.y * other.y);
    public Vec2i Multiply(int i) => new Vec2i(this.x * i, this.y * i);
    // public Vec2i CrossMultiply(Vec2i other) => new Vec2i(this.x * other.y, this.y * other.x);
    public Vec2i Divide(int i) => new Vec2i(this.x / i, this.y / i);
    public Vec2i Negate() => new Vec2i(-this.x, -this.y);

    // Static methods
    public static Vec2i Zero() => new Vec2i();
    public static Vec2i North() => new Vec2i(0, -1);
    public static Vec2i NorthEast() => new Vec2i(1, -1);
    public static Vec2i NorthWest() => new Vec2i(-1, -1);

    public static Vec2i South() => new Vec2i(0, 1);
    public static Vec2i SouthEast() => new Vec2i(1, 1);
    public static Vec2i SouthWest() => new Vec2i(-1, 1);

    public static Vec2i West() => new Vec2i(-1, 0);
    public static Vec2i East() => new Vec2i(1, 0);


    public static Vec2i GetCardinalDirection(CardinalDirection dir) => dir switch
    {
        CardinalDirection.NORTH => North(),
        CardinalDirection.EAST => East(),
        CardinalDirection.SOUTH => South(),
        CardinalDirection.WEST => West(),
        CardinalDirection.NORTHEAST => NorthEast(),
        CardinalDirection.NORTHWEST => NorthWest(),
        CardinalDirection.SOUTHEAST => SouthEast(),
        CardinalDirection.SOUTHWEST => SouthWest(),
        _ => Zero()
    };

}