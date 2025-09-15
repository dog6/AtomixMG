using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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

    private int ActiveParticleCount = 0;
    public bool IsSimulationPaused = false;
    private Texture2D pixelTexture, gridTexture;
    private SpriteFont font;
    // private RenderTarget2D particleDataTex;

    private void ListenForInputs()
    {
        
    }


    //--------------------- Scene
    public void Initialize(GraphicsDevice _graphicsDevice)
    {
        graphicsDevice = _graphicsDevice;
        screenSize = new Vec2i(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);

        // particleDataTex
        partsim = new ParticleSimulator(screenSize);
    }

    public void Load(ContentManager content)
    {
        // Load pixel texture
        pixelTexture = new Texture2D(graphicsDevice, 1, 1);
        pixelTexture.SetData(new[] { Color.White });

        // Load grid texture
        gridTexture = new Texture2D(graphicsDevice, screenSize.x, screenSize.y);
        font = content.Load<SpriteFont>("Fonts/Arial");
        
    }

    public void Update()
    {

        // Toggle simulation
        if (KeyboardHelper.JustPressed(Keys.Space))
        {
            IsSimulationPaused = !IsSimulationPaused;
            Console.WriteLine(IsSimulationPaused ? "Sim paused" : "Sim running");
        }

        // Toggle coloring stable particles
        if (KeyboardHelper.JustPressed(Keys.H))
        {
            partsim.ColorStableParticles = !partsim.ColorStableParticles;
            Console.WriteLine(partsim.ColorStableParticles ? "Coloring stable particles" : "Using default particle colors");
        }

        // Clear simulation
        if (KeyboardHelper.JustPressed(Keys.C))
        {
            partsim.ClearAllParticles();
            Console.WriteLine("Cleared particles from screen");
        }

        // Switching to spawning sand particles
        if (KeyboardHelper.JustPressed(Keys.D1))
        {
            partsim.ParticleSpawnType = ParticleType.Sand;
            Console.WriteLine("Selected sand particles");
        }

        // Switching to spawning water particles
        if (KeyboardHelper.JustPressed(Keys.D2))
        {
            partsim.ParticleSpawnType = ParticleType.Water;
            Console.WriteLine("Selected water particles");
        }

        // Switching to spawning stone particles
        if (KeyboardHelper.JustPressed(Keys.D3))
        {
            partsim.ParticleSpawnType = ParticleType.Stone;
            Console.WriteLine("Selected stone particle");
        }

        // Mouse actions
        var mousePos = MouseHelper.MousePosition;

        // Add particles
        if (MouseHelper.IsPressed(MouseButton.Left))
        {
            if (!MouseHelper.MousePosition.Equals(MouseHelper.LastMousePosition))
                partsim.DrawParticleLine(MouseHelper.LastMousePosition, MouseHelper.MousePosition);
            else
                partsim.AddParticle(mousePos);
        }

        // Remove particles
        if (MouseHelper.IsPressed(MouseButton.Right))
        {
            partsim.RemoveParticleAtPosition(mousePos);
        }

    }

    public void FixedUpdate(float deltaTime)
    {
        if (!IsSimulationPaused)
        {
            partsim.SimulateParticles(deltaTime); // update physics tick
        }
        
        ActiveParticleCount = partsim.particles.Where(p => !p.IsStable).ToList().Count;
    }

    public void Render(SpriteBatch sb)
    {
        // Draw one texture instead of thousands of pixels
        partsim.RefreshSimulator(gridTexture);
        sb.Draw(gridTexture, Vector2.Zero, Color.White);
        sb.DrawString(font, $"Particles: {partsim.particles.Count}\nActive: {ActiveParticleCount}\nStable: {partsim.particles.Count - ActiveParticleCount}", new Vector2(10, 30), Color.White);
    }
    public string GetName() => "Physics2DScene";
    
    int IScene.GetId() => sceneId;

}