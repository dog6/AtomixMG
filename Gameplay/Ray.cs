using System;
using Microsoft.Xna.Framework;

public class Ray
{

    public Vec2i origin;
    public Vec2i direction;
    private Vec2i gridSize;
    // public int length; // length of ray to another particle


    public Ray(Vec2i originPos, Vec2i dir, Vec2i gridSize)
    {
        this.origin = originPos;
        this.direction = dir;
        this.gridSize = gridSize;
    }

    public Vec2i GetLength(Color[] pixelData)
    {
        Vec2i accum = new Vec2i();
        if (direction.Equals(Vec2i.North()))
        {
            for (int y = origin.y-1; y > 0; y--)
            {
                int pixelIndex = y * gridSize.x + origin.x;
                if (pixelData[pixelIndex] == Color.Transparent)
                {
                    accum.y--;
                }
                else
                {
                    break;
                }
            }
        }
        else if (direction.Equals(Vec2i.South()))
        {
            for (int y = origin.y+1; y < gridSize.y; y++)
            {
                int pixelIndex = y * gridSize.y + origin.x;
                if (pixelData[pixelIndex] == Color.Transparent)
                {
                    accum.y++;
                }
                else
                {
                    break;
                }
            }
        }
        else if (direction.Equals(Vec2i.East()))
        {
            for (int x = origin.x+1; x < gridSize.x; x++)
            {
                int pixelIndex = origin.y * gridSize.y + x;
                if (pixelData[pixelIndex] == Color.Transparent)
                {
                    accum.x++;
                }
                else
                {
                    break;
                }
            }
        }
        else if (direction.Equals(Vec2i.West()))
        {
            for (int x = origin.x-1; x < gridSize.x; x++)
            {
                int pixelIndex = origin.y * gridSize.y + x;
                if (pixelData[pixelIndex] == Color.Transparent)
                {
                    accum.x--;
                }
                else
                {
                    break;
                }
            }
        }
        else
        {
            Console.WriteLine($"Failed to get length from ray, unable to check direction ({direction.x}, {direction.y})");
            return new Vec2i();
        }



        return accum;
    }

}