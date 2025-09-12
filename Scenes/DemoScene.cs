using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AtomixMG.Game.Scene;

public class DemoScene : IScene
{

    private Texture2D pixelTexture, gridTexture;
    private GraphicsDevice graphicsDevice;

    private PixelGrid grid;
    private bool gamePaused = true;

    private Color[] pixelData;

    private int gridWidth, gridHeight;
    private int screenWidth, screenHeight;

    // Applies Conway's Game of Life rules to this specific cell using a mask
    // of the specified width and height.
    private void ApplyLifeRulesToCell(int x, int y, bool[,] bufferGrid)
    {
        int neighborCount = grid.CountNeighbors(x, y);

        // if cell on current grid is active, set new grid properly
        if (grid.hasPixel[x, y])
        {
            // Living cell survives with 2 or 3 neighbors
            bufferGrid[x, y] = neighborCount == 2 || neighborCount == 3;
        }
        else
        {
            // Dead cell becomes alive with exactly 3 neighbors
            bufferGrid[x, y] = neighborCount == 3;
        }

        // update pixel data using newGrid data
        // pixelData will be used to create next generations gridTexture, which is drawn on screen

    }

    public void Load(GraphicsDevice _graphicsDevice)
    {
        graphicsDevice = _graphicsDevice;

        // Load pixel texture
        pixelTexture = new Texture2D(graphicsDevice, 1, 1);
        pixelTexture.SetData(new[] { Color.White });

        screenWidth = graphicsDevice.Viewport.Width;
        screenHeight = graphicsDevice.Viewport.Height;

        gridTexture = new Texture2D(graphicsDevice, screenWidth, screenHeight);
        pixelData = new Color[screenWidth * screenHeight];

        // Load pixel grid
        grid = new PixelGrid(screenWidth, screenHeight);
        gridWidth = grid.GetWidth();
        gridHeight = grid.GetHeight();
    }

    public void Start()
    {

    }

    public void Update()
    {
        // Listen for pause
        if (KeyboardHelper.JustPressed(Keys.Space)) gamePaused = !gamePaused;

        // Mouse actions
        var mousePos = MouseHelper.ScreenPos();

        // Listen for populating cells
        if (MouseHelper.IsPressed(MouseButton.Left))
        {
            int x = mousePos.Item1;
            int y = mousePos.Item2;

            if (x < gridWidth && y < gridHeight && x > 0 && y > 0)
            {
                grid.CreatePixel(x, y, graphicsDevice);
                pixelData[y * gridWidth + x] = grid.hasPixel[x, y] ? Color.White : Color.Black;
            }
        }

        // Listen for clearing cells
        if (MouseHelper.IsPressed(MouseButton.Right))
        {
            grid.ClearPixel(mousePos.Item1, mousePos.Item2);
        }

        gridTexture.SetData(pixelData); // update grid texture w/ pixel data

    }

    public void FixedUpdate(float deltaTime)
    {
        if (gamePaused) return;

        // Deep copy current grid
        bool[,] bufferGrid = new bool[gridWidth, gridHeight];

        // Update new copy of grid to next generation
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                // examine each cell
                if (x >= 3 && y >= 3 && x < gridWidth - 3 && y < gridHeight - 3)
                {
                    // Apply CGOL logic
                    ApplyLifeRulesToCell(x, y, bufferGrid);
                }
            }
        }

        grid.SetGrid(bufferGrid);

        // THEN update pixel data
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                pixelData[y * gridWidth + x] = bufferGrid[x, y] ? Color.White : Color.Black;
            }
        }

    }

    public void Render(SpriteBatch sb)
    {
        // Draw one texture instead of thousands of pixels
        sb.Draw(gridTexture, Vector2.Zero, Color.White);
    }

}