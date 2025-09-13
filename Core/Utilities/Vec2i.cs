
using System.Runtime.ConstrainedExecution;

public struct Vec2i
{
    public int x;
    public int y;

    public Vec2i(int x = 0, int y = 0)
    {
        this.x = x;
        this.y = y;
    }

    public static Vec2i Zero() => new Vec2i();

}