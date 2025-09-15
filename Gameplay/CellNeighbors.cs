
using System.Collections.Generic;
using System.Linq;
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
    private HashSet<Vec2i> occupied;

    public CellNeighbors(Vec2i originPos, List<Particle> particles, bool[] skip = null)
    {

        if (skip == null)
        {
            skip = new bool[] { false,false,false,false,false,false,false,false };
        }

        this.Position = originPos;

        occupied = new HashSet<Vec2i>(particles.Select(p => p.Position));

        
        this.HasNorth = !skip[0] ? HasNeighbor(CardinalDirection.NORTH, particles) : true;
        this.HasNorthEast = !skip[1] ? HasNeighbor(CardinalDirection.NORTHEAST, particles) : true;
        this.HasEast = !skip[2] ? HasNeighbor(CardinalDirection.EAST, particles) : true;
        this.HasSouthEast = !skip[3] ? HasNeighbor(CardinalDirection.SOUTHEAST, particles) : true;
        this.HasSouth = !skip[4] ? HasNeighbor(CardinalDirection.SOUTH, particles) : true;
        this.HasSouthWest = !skip[5] ? HasNeighbor(CardinalDirection.SOUTHWEST, particles) : true;
        this.HasWest = !skip[6] ? HasNeighbor(CardinalDirection.WEST, particles) : true;
        this.HasNorthWest = !skip[7] ? HasNeighbor(CardinalDirection.NORTHWEST, particles) : true;
    }

    private bool HasNeighbor(CardinalDirection dir, List<Particle> particles)
    {
        //alternative method, may be faster
        var targetPos = this.Position.Add(Vec2i.GetCardinalDirection(dir));
        return occupied.Contains(targetPos);
    }

}