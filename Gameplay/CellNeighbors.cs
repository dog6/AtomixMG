
using System.Collections.Generic;

public enum CardinalDirection
{
    NORTH,
    NORTHEAST,
    EAST,
    SOUTHEAST,
    SOUTH,
    SOUTHWEST,
    WEST,
    NORTHWEST
}

public struct CellNeighbors
{
    public bool HasNorth,
                HasSouth,
                HasWest,
                HasEast,
                HasNorthWest,
                HasNorthEast,
                HasSouthWest,
                HasSouthEast;

    private Vec2i Position;

    public CellNeighbors(Vec2i originPos, List<Particle> particles)
    {
        this.Position = originPos;

        this.HasNorth = HasNeighbor(CardinalDirection.NORTH, particles);
        this.HasSouth = HasNeighbor(CardinalDirection.SOUTH, particles);
        this.HasWest = HasNeighbor(CardinalDirection.WEST, particles);
        this.HasEast = HasNeighbor(CardinalDirection.EAST, particles);
        this.HasNorthWest = HasNeighbor(CardinalDirection.NORTHWEST, particles);
        this.HasNorthEast = HasNeighbor(CardinalDirection.NORTHEAST, particles);
        this.HasSouthWest = HasNeighbor(CardinalDirection.SOUTHWEST, particles);
        this.HasSouthEast = HasNeighbor(CardinalDirection.SOUTHEAST, particles);
    }

    private bool HasNeighbor(CardinalDirection dir, List<Particle> particles)
    {
        var targetPos = this.Position.Add(Vec2i.GetCardinalDirection(dir));
        return particles.Exists(p => p.Position.Equals(targetPos));

        /* alternative method, may be faster
        var targetPos = this.Position.Add(Vec2i.GetCardinalDirection(dir));
        HashSet<Vec2i> occupied = new HashSet<Vec2i>(particles.Select(p => p.Position));
        return occupied.Contains(targetPos);
        */
    }

}