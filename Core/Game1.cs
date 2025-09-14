using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using AtomixMG.Game.Scene;

namespace AtomixMG.Game;

public class Game1 : Microsoft.Xna.Framework.Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private const float fixedTimeStep = 1f / 60f;
    private float timeStepAccum = 0f;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        SceneManager.Initialize(GraphicsDevice);
        SceneManager.AddScene(new DemoScene(), new Physics2DScene());
        SceneManager.SetSceneById(1);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice); // load spritebatch
        SceneManager.LoadCurrentScene();
    }

    private void FixedStep(GameTime gameTime)
    {
        // Calculate fixed update
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        timeStepAccum += dt;
        while (timeStepAccum >= fixedTimeStep)
        {
            SceneManager.FixedUpdateCurrentScene(fixedTimeStep);
            timeStepAccum -= fixedTimeStep;
        }
    }

    protected override void Update(GameTime gameTime)
    {
        KeyboardHelper.Update();
        MouseHelper.Update();
        // Listen for exit
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        SceneManager.UpdateCurrentScene();

        FixedStep(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin();

        SceneManager.RenderCurrentScene(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
