using System;
using System.Diagnostics;
using AtomixMG.Game;

class PixelGrid : Game1
{
    public bool[,] hasPixel { get; private set; }

    public int gridWidth, gridHeight;

    public PixelGrid(int width, int height)
    {
        this.gridWidth = width;
        this.gridHeight = height;
        this.hasPixel = new bool[width, height];
    }

    public void CreatePixel(int x, int y)
    {
        if (!MouseHelper.IsInsideViewport(GraphicsDevice)) return;
        if (hasPixel[x, y]) return;

        // populate empty cell
        hasPixel[x, y] = true;
    }

    public void SetGrid(bool[,] pixelGrid) => this.hasPixel = pixelGrid;

    public void ClearPixel(int x, int y)
    {
        if (x < gridWidth && y < gridHeight && x > 0 && y > 0)
            if (!hasPixel[x, y]) return; // cell already empty
            hasPixel[x, y] = false; // clear cell
    }

    /// <summary>
    /// Counts neighbors in surrounding rectangle.<br/>
    /// X and Y are center position.
    /// </summary>
    /// <param name="x">X position of cell</param>
    /// <param name="y">Y position of cell</param>
    /// <param name="maskWidth">Width of mask</param>
    /// <param name="maskHeight">Height of mask</param>
    /// <returns></returns>
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
        int clampedEndX = Math.Min(gridWidth - 1, endX);
        int clampedStartY = Math.Max(0, startY);
        int clampedEndY = Math.Min(gridHeight - 1, endY);

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

}