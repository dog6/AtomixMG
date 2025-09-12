using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using AtomixMG.Game.Scene;

namespace AtomixMG.Game;

public class Game1 : Microsoft.Xna.Framework.Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private const float fixedTimeStep = 1f / 16f;
    private float timeStepAccum = 0f;

    DemoScene demoScene = new DemoScene();

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        demoScene.Start();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        demoScene.Load(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        KeyboardHelper.Update();

        // Listen for exit
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        demoScene.Update();

        // Calculate fixed update
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        timeStepAccum += dt;
        while (timeStepAccum >= fixedTimeStep)
        {
        demoScene.FixedUpdate(fixedTimeStep);
        timeStepAccum -= fixedTimeStep;
        }

        // TODO: Add your update logic here
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        // TODO: Add your drawing code here
        _spriteBatch.Begin();

        demoScene.Render(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
