using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AtomixMG.Game.Scene;

public class Physics2DScene : IScene
{
    // Scene vars
    private const int sceneId = 1;
    // private int screenWidth, screenHeight;
    private Vec2i screenSize;
    private GraphicsDevice graphicsDevice;
    private ParticleSimulator partsim;


    public bool IsSimulationPaused = false;
    private Texture2D pixelTexture, gridTexture;


    //--------------------- Scene
    public void Initialize(GraphicsDevice _graphicsDevice)
    {
        graphicsDevice = _graphicsDevice;
        screenSize = new Vec2i(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);

        partsim = new ParticleSimulator(screenSize);

    }

    public void Load()
    {
        // Load pixel texture
        pixelTexture = new Texture2D(graphicsDevice, 1, 1);
        pixelTexture.SetData(new[] { Color.White });

        // Load grid texture
        gridTexture = new Texture2D(graphicsDevice, screenSize.x, screenSize.y);
    }

    public void Update()
    {

        // Toggle simulation
        if (KeyboardHelper.JustPressed(Keys.Space))
        {
            IsSimulationPaused = !IsSimulationPaused;
            Console.WriteLine(IsSimulationPaused ? "Sim paused" : "Sim running");
        }

        // Clear simulation
        if (KeyboardHelper.JustPressed(Keys.C))
        {
            partsim.ClearAllParticles();
            Console.WriteLine("Cleared particles from screen");
        }

        if (KeyboardHelper.JustPressed(Keys.D1))
        {
            partsim.ParticleSpawnType = ParticleType.Sand;
            Console.WriteLine("Selected sand particles");
        }

        if (KeyboardHelper.JustPressed(Keys.D2))
        {
            partsim.ParticleSpawnType = ParticleType.Water;
            Console.WriteLine("Selected water particles");
        }

        if (KeyboardHelper.JustPressed(Keys.D3))
        {
            partsim.ParticleSpawnType = ParticleType.Stone;
            Console.WriteLine("Selected stone particle");
        }

        // Mouse actions
        var mousePos = MouseHelper.ScreenPosition();

        // Add particles
        if (MouseHelper.IsPressed(MouseButton.Left))
        {
            partsim.AddParticle(mousePos);
        }

        // Remove particles
        if (MouseHelper.IsPressed(MouseButton.Right))
        {
            partsim.RemoveParticleAtPosition(mousePos);
        }

        // gridTexture.SetData(partsim.pixelData);
        partsim.RefreshSimulator(gridTexture);
    }
    
    public void FixedUpdate(float deltaTime)
    {
        // partsim.RefreshSimulator(gridTexture); // refresh pixels in simulator
        partsim.SimulateParticles(deltaTime); // update physics tick
    }

    public void Render(SpriteBatch sb)
    {
        // Draw one texture instead of thousands of pixels
        sb.Draw(gridTexture, Vector2.Zero, Color.White);
    }
    public string GetName() => "Physics2DScene";
    
    int IScene.GetId() => sceneId;

}