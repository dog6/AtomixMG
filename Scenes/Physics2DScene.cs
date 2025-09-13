using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AtomixMG.Game.Scene;

public struct PhysicsPixel
{
    public bool isActive;
    public Vector2 velocity;
    public float mass;
    public Color color;
    public int materialType; // 0=sand, 1=water, 2=stone
}

public class Physics2DScene : IScene
{
    private const int sceneId = 1;
    private Texture2D pixelTexture, gridTexture;
    private GraphicsDevice graphicsDevice;
    private bool simulationPaused = false;
    private Color[] pixelData;
    private List<Particle> particles;
    private Vec2i initParticleAcceleration = new Vec2i(0,1);

    private int gridWidth, gridHeight;
    private int screenWidth, screenHeight;

    private void AddParticle(Vec2i spawnPos)
    {
        particles.Add(new Particle(spawnPos, 1, initParticleAcceleration));
    }

    private void RemoveParticleAtPosition(Vec2i targetPos)
    {
        var targetParticle = particles.Where(particle => particle.position.x == targetPos.x && particle.position.y == targetPos.y).FirstOrDefault();
        if (targetParticle == null)
        {
            return;
        }
        particles.Remove(targetParticle);
    }

    private void SimulateParticles()
    {
        particles.ForEach((particle) =>
        {
            // Simulate particle

        });


    }

    public void Initialize(GraphicsDevice _graphicsDevice)
    {
        graphicsDevice = _graphicsDevice;

        screenWidth = graphicsDevice.Viewport.Width;
        screenHeight = graphicsDevice.Viewport.Height;

        gridWidth = screenWidth;
        gridHeight = screenHeight;

        particles = new List<Particle>();
        pixelData = new Color[screenWidth * screenHeight];

        // Initialize grids
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                pixelData[y * gridWidth + x] = Color.Transparent;
            }
        }
    }
    
    public void Load()
    {
    }
    
    public void Update()
    {
        // Toggle simulation
        if (KeyboardHelper.JustPressed(Keys.Space))
            simulationPaused = !simulationPaused;

        // Clear simulation
        if (KeyboardHelper.JustPressed(Keys.C))
        {
            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    pixelData[y * gridWidth + x] = Color.Transparent;
                }
            }
        }

        // Mouse actions
        var mousePos = MouseHelper.ScreenPosition();

        // Add particles
        if (MouseHelper.IsPressed(MouseButton.Left))
        {
            AddParticle(mousePos);
        }

        // Remove particles
        if (MouseHelper.IsPressed(MouseButton.Right))
        {
            RemoveParticleAtPosition(mousePos);
        }

        gridTexture.SetData(pixelData);
    }
    
    public void FixedUpdate(float deltaTime)
    {
       
    }
    
    private Color GetMaterialColor(int materialType)
    {
        return materialType switch
        {
            0 => Color.Yellow,     // Sand
            1 => Color.Cyan,       // Water  
            2 => Color.Gray,       // Stone
            _ => Color.White
        };
    }

    public void Render(SpriteBatch sb)
    {
        // Draw one texture instead of thousands of pixels
        sb.Draw(gridTexture, Vector2.Zero, Color.White);
    }
    
    public string GetName() => "Physics2DScene";
    
    int IScene.GetId() => sceneId;

}