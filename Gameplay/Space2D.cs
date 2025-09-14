
// Repersents a 2D space that contains physics objects
public class Space2D
{
    public Vec2i gravity;
    public Vec2i size;
    public int airDensity;

    public Space2D(Vec2i size, Vec2i? gravity = null, int airDensity = 1)
    {
        if (gravity == null) this.gravity = Vec2i.South();
        this.size = size;
        this.airDensity = airDensity;
    }

}