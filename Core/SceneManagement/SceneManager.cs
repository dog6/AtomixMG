

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

public static class SceneManager
{

    public static int currentSceneId { get; private set; }
    public static IScene currentScene; // active scene being rendered/updated
    private static List<IScene> Scenes;
    private static GraphicsDevice graphicsDevice;

    public static void Initialize(GraphicsDevice _graphicsDevice)
    {
        Scenes = new List<IScene>();
        currentSceneId = -1; // no scene, return -1
        graphicsDevice = _graphicsDevice;
    }


    // Get scene by scene id
    public static IScene GetSceneById(int id) => Scenes.Where(scene => scene.GetId() == id).FirstOrDefault();

    // Get scene by scene name
    public static IScene GetSceneByName(string name) => Scenes.Where(scene => scene.GetName() == name).FirstOrDefault();

    // Adds scenes
    public static void AddScene(params IScene[] scenes)
    {
        foreach (var scene in scenes)
        {
            if (scene == null)
            {
                Console.WriteLine("Failed to add NULL scene");
                continue;
            }

            if (!Scenes.Contains(scene))
            {
                Scenes.Add(scene);
                continue;
            }
            else
            {
                Console.WriteLine($"Failed to add scene with conflicting id: {scene.GetId()}");
            }
            Console.WriteLine($"Failed to add scene {scene.GetName()}");
        }
    }

    public static void RemoveScene(IScene scene)
    {
        if (scene != null)
        {
            if (Scenes.Contains(scene))
            {
                Scenes.Remove(scene);
                return;
            }
            else
            {
                Console.WriteLine($"Failed to remove scene, it has not been added to the SceneManager");
                return;
            }
        }
        Console.WriteLine("Failed to remove NULL scene");
    }

    public static void SetSceneById(int id)
    {
        IScene nextScene = GetSceneById(id);
        if (nextScene == null)
        {
            Console.WriteLine($"Failed to set scene. No scene with id {id}");
            return;
        }

        if (currentScene != nextScene)
        {
            nextScene.Initialize(graphicsDevice);
            currentScene = nextScene;
        }

    }
    public static void LoadCurrentScene()
    {
        if (currentScene == null)
        {
            Console.WriteLine("Cannot load current scene, current scene is NULL");
            return;
        }
        currentScene.Load();
    }
    public static void UpdateCurrentScene()
    {
        if (currentScene == null)
        {
            Console.WriteLine("Cannot call Update() on current scene, current scene is NULL");
            return;
        }
        currentScene.Update();
    }
    public static void FixedUpdateCurrentScene(float dt)
    {
        if (currentScene == null)
        {
            Console.WriteLine("Cannot call FixedUpdate() on current scene, current scene is NULL");
            return;
        }
        currentScene.FixedUpdate(dt);
    }
    public static void RenderCurrentScene(SpriteBatch sb)
    {
        if (currentScene == null)
        {
            Console.WriteLine("Cannot call Render() on current scene, current scene is NULL");
            return;
        }
        currentScene.Render(sb);
    }

}