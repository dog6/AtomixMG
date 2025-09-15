
// Defines a single pixel, holds no graphical data
// All particles will be drawn to screen using a Color[] arr
using System.Collections.Generic;
using System;
public enum ParticleType {
    Sand,
    Water,
    Stone
}

public class Particle : IPhysicsBody
{
    public Vec2i Position;
    public Vec2i Acceleration;
    public Vec2i Velocity;
    public int Mass;
    public Vec2i Gravity;
    public ParticleType ParticleType;
    public bool IsStable;

    

    private Random rand;
    private Space2D space; // space this body is in

    public Particle(Vec2i position, Space2D space, ParticleType particleType, int mass = 1, Vec2i initialAcceleration = new Vec2i(), Vec2i gravity = new Vec2i())
    {
        this.Position = position;
        this.Mass = mass;

        this.Velocity = Vec2i.Zero();
        this.Acceleration = initialAcceleration;
        this.Gravity = gravity;
        this.space = space;
        this.ParticleType = particleType;

        this.rand = new Random();
    }

    public void Simulate(List<Particle> particles)
    {
        CellNeighbors neighbors = new CellNeighbors(this.Position, particles);
        
        switch (ParticleType)
        {
            case ParticleType.Sand:
                SimulateSand(particles, neighbors);
                break;
            case ParticleType.Water:
                SimulateWater(particles, neighbors);
                break;
            case ParticleType.Stone:
                IsStable = true;
                break;
            default:
                break;
        }
        HandleBorderCollisions();
    }

    // Moves pixel 1 unit in a given direction
    private void Move(CardinalDirection dir) => this.Position = this.Position.Add(Vec2i.GetCardinalDirection(dir));

    private bool CanMove(CardinalDirection dir, CellNeighbors neighbors)
    {
        switch (dir)
        {
            case CardinalDirection.NORTH:
                if (!neighbors.HasNorth)
                {
                    Move(CardinalDirection.NORTH);
                    return true;
                }
                break;

            case CardinalDirection.EAST:
                if (!neighbors.HasEast)
                {
                    Move(CardinalDirection.EAST);
                    return true;
                }
                break;

            case CardinalDirection.SOUTH:
                if (!neighbors.HasSouth)
                {
                    Move(CardinalDirection.SOUTH);
                    return true;
                }
                break;

            case CardinalDirection.WEST:
                if (!neighbors.HasWest)
                {
                    Move(CardinalDirection.WEST);
                    return true;
                }
                break;
            case CardinalDirection.NORTHWEST:
                if (!neighbors.HasNorthWest)
                {
                    Move(CardinalDirection.NORTHWEST);
                    return true;
                }
                break;

            case CardinalDirection.NORTHEAST:
                if (!neighbors.HasNorthEast)
                {
                    Move(CardinalDirection.NORTHEAST);
                    return true;
                }
                break;

            case CardinalDirection.SOUTHEAST:
                if (!neighbors.HasSouthEast)
                {
                    Move(CardinalDirection.SOUTHEAST);
                    return true;
                }
                break;

            case CardinalDirection.SOUTHWEST:
                if (!neighbors.HasSouthWest)
                {
                    Move(CardinalDirection.SOUTHWEST);
                    return true;
                }
                break;
        }
        return false;
    }

    public void SimulateSand(List<Particle> particles, CellNeighbors neighbors)
    {
        if (!neighbors.HasSouth || !neighbors.HasSouthEast || !neighbors.HasSouthWest) IsStable = false;
        if (IsStable) return;
        if (this.Position.y == space.size.y - 1) IsStable = true;


        if (CanMove(CardinalDirection.SOUTH, neighbors)) return;
        if (CanMove(CardinalDirection.SOUTHEAST, neighbors)) return;
        if (CanMove(CardinalDirection.SOUTHWEST, neighbors)) return;

        // --- Otherwise stay in place ---
        IsStable = true;
    }

    public void SimulateWater(List<Particle> particles, CellNeighbors neighbors)
    {
            if (!neighbors.HasSouth || !neighbors.HasSouthEast || !neighbors.HasSouthWest || !neighbors.HasEast || !neighbors.HasWest) IsStable = false;
            if (CanMove(CardinalDirection.SOUTH, neighbors)) return;

            bool leftFlow = rand.Next(2) == 0;

            if (leftFlow)
            {
                if (CanMove(CardinalDirection.SOUTHWEST, neighbors)) return;
                if (CanMove(CardinalDirection.WEST, neighbors)) return;
                if (CanMove(CardinalDirection.SOUTHEAST, neighbors)) return;
                if (CanMove(CardinalDirection.EAST, neighbors)) return;
            }
            else
            {
                if (CanMove(CardinalDirection.SOUTHEAST, neighbors)) return;
                if (CanMove(CardinalDirection.EAST, neighbors)) return;
                if (CanMove(CardinalDirection.SOUTHWEST, neighbors)) return;
                if (CanMove(CardinalDirection.WEST, neighbors)) return;
            }
            // Otherwise stay in place
            IsStable = true;
    }

    private void HandleBorderCollisions()
    {
        if (Position.x < 0)
        {
            Position.x = 0;
            Velocity.x = -Velocity.x;
        }
        else if (Position.x >= space.size.x)
        {
            Position.x = space.size.x - 1;
            Velocity.x = -Velocity.x;
        }

        if (Position.y < 0)
        {
            Position.y = 0;
            Velocity.y = -Velocity.y;
        }
        else if (Position.y >= space.size.y)
        {
            Position.y = space.size.y - 1;
            Velocity.y = -Velocity.y;
        }
    }

}