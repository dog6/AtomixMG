using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class ParticleSimulator
{

    const ParticleType DEFAULT_PARTICLE_TYPE = ParticleType.Sand;

    // Physics2DScene variables
    public Color[] pixelData { get; private set; }
    public ParticleType ParticleSpawnType;

    private List<Particle> particles;
    private Vec2i initParticleAcceleration = new Vec2i(0, 1);
    private Vec2i sceneGravity = new Vec2i(0, 1);
    private Space2D worldSpace;
    private int airDensity = 1;
    private static int screenWidth, screenHeight;

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
        Console.WriteLine($"Adding particle @ {spawnPos.x}, {spawnPos.y} #{particles.Count}");
        particles.Add(new Particle(spawnPos, worldSpace, ParticleSpawnType, 1, initParticleAcceleration, sceneGravity));
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
        foreach (var particle in particles)
        {
            particle.Simulate(particles);
        }


        // Step 2: handle particle-to-particle collisions (pairwise)
        int count = particles.Count;
        for (int i = 0; i < count; i++)
        {
            var p1 = particles[i];
            for (int j = i + 1; j < count; j++)
            {
                var p2 = particles[j];

                if (p1.Position.Equals(p2.Position))
                {
                    // Simple elastic collision: swap velocities
                    Vec2i temp = p1.Velocity;
                    p1.Velocity = p2.Velocity;
                    p2.Velocity = temp;
                }
            }
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
        pixelData = new Color[screenWidth * screenHeight];

        List<Particle> particlesToRemove = new List<Particle>();
        foreach (var particle in particles)
        {
            int pixelIndex = particle.Position.y * screenWidth + particle.Position.x;

            if (pixelIndex < 0 || pixelIndex > screenWidth * screenHeight)
            {
                // skip, pixel off screen
                particlesToRemove.Add(particle); // add for removal
                continue;
            }

            AssignPixelColor(pixelIndex, particle);

        }

        foreach (var particle in particlesToRemove)
        {
            particles.Remove(particle);
        }
    }

    private void AssignPixelColor(int pixelIndex, Particle particle)
    {
        pixelData[pixelIndex] = particle.ParticleType switch
        {
            ParticleType.Sand => Color.Yellow,
            ParticleType.Water => Color.Cyan,
            ParticleType.Stone => Color.Gray,
            _ => Color.White
        };

        /*if (particle.IsStable) {
            pixelData[pixelIndex] = Color.LimeGreen; // show stable particles as green
        }*/
    }

}