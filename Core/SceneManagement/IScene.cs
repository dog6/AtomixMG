
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public interface IScene
{
    // Loads graphical resources needed by scene
    public void Load(ContentManager content);

    // Initialize scene and load any non-graphical resources needed by scene
    public void Initialize(GraphicsDevice graphicsDevice);
    public void Update();
    public void Render(SpriteBatch sb); // draw loop
    public void FixedUpdate(float deltaTime);
    public string GetName();
    public int GetId();
}