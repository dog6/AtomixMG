

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

public static class SceneManager
{

    public static int currentSceneId { get; private set; }
    public static IScene currentScene; // active scene being rendered/updated
    // private static List<IScene> Scenes { get; private ; }
    private static List<IScene> Scenes;
    private static GraphicsDevice graphicsDevice;

    public static void Initialize(GraphicsDevice _graphicsDevice)
    {
        Scenes = new List<IScene>();
        currentSceneId = -1; // no scene, return -1
        graphicsDevice = _graphicsDevice;
    }


    // Get scene by scene id
    public static IScene GetSceneById(int id) => Scenes.Where(scene => scene.GetId() == id).First();

    // Get scene by scene name
    public static IScene GetSceneByName(string name) => Scenes.Where(scene => scene.GetName() == name).First();

    // Adds scenes
    public static void AddScene(IScene scene)
    {
        if (scene == null)
        {
            Console.WriteLine("Failed to add NULL scene");
            return;
        }

        if (!Scenes.Contains(scene))
        {
            Scenes.Add(scene);
            return;
        }

        Console.WriteLine($"Failed to add scene {scene.GetName()}");
    }

    public static void RemoveScene(IScene scene)
    {
        if (scene != null && Scenes.Contains(scene))
        {
            Scenes.Remove(scene);
            return;
        }
        Console.WriteLine("Failed to remove NULL scene");
    }

    public static void SetSceneById(int id)
    {
        IScene nextScene = GetSceneById(id);
        if (nextScene == null)
        {
            Console.WriteLine("Cannot set scene to null");
            return;
        }

        if (currentScene != nextScene)
        {
            nextScene.Initialize(graphicsDevice);
            currentScene = null; // unload current scene
            currentScene = nextScene;
            // LoadCurrentScene();
            // nextScene.Load();

            // Prepare next scene

        }

    }


    /// <summary>
    /// Loads graphical resources for scene.
    /// </summary>
    /// <param name="scene">IScene that will be loaded</param>
    private static void LoadScene(IScene scene) => scene.Load();

    public static void LoadCurrentScene()
    {
        if (currentScene == null)
        {
            Console.WriteLine("Cannot load current scene, current scene is NULL");
            return;
        }
        currentScene.Load();
    }

    /// <summary>
    /// Loads graphical resources for scene.<br/>
    /// Finds scene using sceneId
    /// </summary>
    /// <param name="id">Id of targeted scene</param>
    public static void LoadSceneById(int id) => GetSceneById(id).Load();

    public static void UpdateCurrentScene()
    {
        if (currentScene == null)
        {
            Console.WriteLine("Cannot call Update() on current scene, current scene is NULL");
            return;
        }
        currentScene.Update();
    }

    public static void UpdateScene(IScene scene) => scene.Update();

    public static void FixedUpdateCurrentScene(float dt)
    {
        if (currentScene == null)
        {
            Console.WriteLine("Cannot call FixedUpdate() on current scene, current scene is NULL");
            return;
        }
        currentScene.FixedUpdate(dt);
    }
    public static void FixedUpdate(IScene scene, float dt) => scene.FixedUpdate(dt);
     
    public static void RenderCurrentScene(SpriteBatch sb)
    {
        if (currentScene == null)
        {
            Console.WriteLine("Cannot call Render() on current scene, current scene is NULL");
            return;
        }
        currentScene.Render(sb);
    }

    public static void Render(IScene scene, SpriteBatch sb) => scene.Render(sb);


}