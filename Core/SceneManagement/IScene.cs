
using Microsoft.Xna.Framework.Graphics;

public interface IScene
{

    public void Load(GraphicsDevice graphicsDevice);
    public void Start();
    public void Update();
    public void Render(SpriteBatch sb); // draw loop
    public void FixedUpdate(float deltaTime);

}