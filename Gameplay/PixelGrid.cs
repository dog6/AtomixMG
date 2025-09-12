using System;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

class PixelGrid
{
    public bool[,] hasPixel { get; private set; }

    private int width, height;

    public PixelGrid(int width, int height)
    {
        this.width = width;
        this.height = height;
        this.hasPixel = new bool[width, height];
    }

    public void CreatePixel(int x, int y, GraphicsDevice graphicsDevice)
    {
        if (!MouseHelper.IsInsideViewport(graphicsDevice)) return;
        if (hasPixel[x, y]) return;

        // populate empty cell
        hasPixel[x, y] = true;
    }

    public void SetGrid(bool[,] pixelGrid) => this.hasPixel = pixelGrid;

    public void ClearPixel(int x, int y)
    {
        if (x < GetWidth() && y < GetHeight() && x > 0 && y > 0)
        {
            if (!hasPixel[x, y]) return; // cell already empty
                                         // clear cell
            hasPixel[x, y] = false;
        }
    }

    public int CountNeighbors(int x, int y, int maskWidth = 3, int maskHeight = 3)
    {
        int result = 0;
        if (maskWidth <= 0 || maskHeight <= 0)
        {
            Debug.Write("mask width and height must be greater than 0");
            return -1;
        }

        // Pre-calculate bounds to avoid repeated calculations
        int halfMaskWidth = maskWidth >> 1;  // Bit shift is faster than division
        int halfMaskHeight = maskHeight >> 1;
        int startX = x - halfMaskWidth;
        int endX = x + halfMaskWidth;
        int startY = y - halfMaskHeight;
        int endY = y + halfMaskHeight;

        // Clamp bounds to valid range
        int clampedStartX = Math.Max(0, startX);
        int clampedEndX = Math.Min(hasPixel.GetLength(0) - 1, endX);
        int clampedStartY = Math.Max(0, startY);
        int clampedEndY = Math.Min(hasPixel.GetLength(1) - 1, endY);

        // Loop with pre-clamped bounds
        for (int nx = clampedStartX; nx <= clampedEndX; nx++)
        {
            for (int ny = clampedStartY; ny <= clampedEndY; ny++)
            {
                // Skip center cell
                if (nx == x && ny == y)
                    continue;

                // Direct array access without bounds checking since we pre-clamped
                if (hasPixel[nx, ny])
                {
                    result++;
                }
            }
        }

        return result;
    }

    public int GetWidth() => width;
    public int GetHeight() => height;

}