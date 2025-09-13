
// Defines a single pixel, holds no graphical data
// All particles will be drawn to screen using a Color[] arr
using Microsoft.Xna.Framework.Graphics;

public class Particle
{
    public Vec2i position { get; private set; }
    public Vec2i acceleration { get; private set; }
    public Vec2i velocity { get; private set; }
    public int mass;
    public int gravity;

    public Particle(Vec2i position, int mass = 1, Vec2i initialAcceleration = new Vec2i())
    {
        this.position = position;
        this.mass = mass;

        this.velocity = Vec2i.Zero();
        this.acceleration = initialAcceleration;
    }

    public void Simulate()
    {
        
        // Handle collisions


        // Handle physics

    }

}