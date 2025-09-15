
// Describes an object that is effected by physics
using System.Collections.Generic;
public interface IPhysicsBody
{
    public void SimulateSand(List<Particle> particles, CellNeighbors neighbors);
}