using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class ParticleSimulator
{

    const ParticleType DEFAULT_PARTICLE_TYPE = ParticleType.Sand;
    public bool ColorStableParticles = false;

    // Physics2DScene variables
    public Color[] pixelData { get; private set; }
    public ParticleType ParticleSpawnType;

    public List<Particle> particles { get; private set; }
    private Vec2i initParticleAcceleration = new Vec2i(0, 1);
    private Vec2i sceneGravity = new Vec2i(0, 1);
    private Space2D worldSpace;
    private int airDensity = 1;
    private static int screenWidth, screenHeight;
    private int lastSpawnPos;
    private List<Particle> particlesToSimulate;

    public ParticleSimulator(Vec2i screenSize)
    {
        screenWidth = screenSize.x;
        screenHeight = screenSize.y;

        particles = new List<Particle>();

        pixelData = new Color[screenWidth * screenHeight];
        Array.Fill(pixelData, Color.Transparent);

        worldSpace = new Space2D(screenSize, null, airDensity);

        ParticleSpawnType = DEFAULT_PARTICLE_TYPE;
    }

    //--------------------- Particles
    public void AddParticle(Vec2i spawnPos)
    {
        if (spawnPos.x > 0 && spawnPos.x < worldSpace.size.x && spawnPos.y > 0 && spawnPos.y < worldSpace.size.y)
        {
            Console.WriteLine($"Adding particle @ {spawnPos.x}, {spawnPos.y} #{particles.Count}");
            particles.Add(new Particle(spawnPos, worldSpace, ParticleSpawnType, 1, initParticleAcceleration, sceneGravity));
            particlesToSimulate = particles.Where(p => !p.IsStable).ToList();
        }
    }

    // Draw lines of particles
    public void DrawParticleLine(Vec2i startPos, Vec2i endPos)
    {
        int x0 = startPos.x;
        int y0 = startPos.y;
        int x1 = endPos.x;
        int y1 = endPos.y;

        int dx = Math.Abs(x1 - x0);
        int dy = Math.Abs(y1 - y0);
        int sx = (x0 < x1) ? 1 : -1;
        int sy = (y0 < y1) ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            AddParticle(new Vec2i(x0, y0));  // spawn at current pixel

            if (x0 == x1 && y0 == y1)
                break;

            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }
    }

    public void RemoveParticleAtPosition(Vec2i targetPos)
    {
        var targetParticle = particles.Where(particle => particle.Position.x == targetPos.x && particle.Position.y == targetPos.y).FirstOrDefault();
        if (targetParticle == null)
        {
            return;
        }
        particles.Remove(targetParticle);
    }

    public void SimulateParticles(float dt)
    {   
        // Step 1: update all particles
        pixelData = new Color[screenWidth * screenHeight];
        if (particlesToSimulate == null || particlesToSimulate.Count() == 0) return;
        foreach (var particle in particlesToSimulate)
        {
            particle.Simulate(particles);
        }
    }

    public void ClearAllParticles() => particles = new List<Particle>();

    public void RefreshSimulator(Texture2D gameTexture)
    {
        UpdatePixelData(gameTexture);
        gameTexture.SetData(pixelData); // update grid texture w/ pixel data
    }

    private void UpdatePixelData(Texture2D gameTexture)
    {
        Array.Clear(pixelData, 0, pixelData.Length); // reuse instead of new

        foreach (var particle in particles)
        {
            int pixelIndex = particle.Position.y * screenWidth + particle.Position.x;
            AssignPixelColor(pixelIndex, particle);
            pixelData[pixelIndex] = GetParticleColor(particle);
        }

        gameTexture.SetData(pixelData); // still uploads full texture
    }

    private Color GetParticleColor(Particle particle) => particle.ParticleType switch
    {
        ParticleType.Sand => Color.Yellow,
        ParticleType.Water => Color.Cyan,
        ParticleType.Stone => Color.Gray,
        _ => Color.White
    };

    private void AssignPixelColor(int pixelIndex, Particle particle)
    {
        pixelData[pixelIndex] = GetParticleColor(particle);

        if (ColorStableParticles && particle.IsStable)
        {
            pixelData[pixelIndex] = Color.LimeGreen; // show stable particles as green
        }
    }

}