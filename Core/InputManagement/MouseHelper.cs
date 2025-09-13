using System;
using System.ComponentModel;
using AtomixMG.Game;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public enum MouseButton
{
    Left,
    Right,
    Middle,
    XButton1,
    XButton2
}

public static class MouseHelper
{

    private static MouseState mouseState;

    /// <summary>
    /// Returns true if the mouse cursor is within the active window's viewport.
    /// </summary>
    public static bool IsInsideViewport(GraphicsDevice graphicsDevice)
    {
        var mousePos = Mouse.GetState().Position;
        var vp = graphicsDevice.Viewport;

        return mousePos.X >= vp.X &&
            mousePos.X < vp.X + vp.Width &&
            mousePos.Y >= vp.Y &&
            mousePos.Y < vp.Y + vp.Height;
    }

    /// <summary>
    /// Gets whether the specified mouse button is currently pressed.
    /// </summary>
    public static bool IsPressed(MouseButton button)
    {
        var state = Mouse.GetState();
        return GetButtonState(state, button) == ButtonState.Pressed;
    }

    /// <summary>
    /// Gets whether the specified mouse button is currently released.
    /// </summary>
    public static bool IsReleased(MouseButton button)
    {
        var state = Mouse.GetState();
        return GetButtonState(state, button) == ButtonState.Released;
    }

    public static Vec2i ScreenPosition() => new Vec2i(mouseState.X, mouseState.Y);

    /// <summary>
    /// Internal helper that maps a MouseButton to the corresponding ButtonState.
    /// </summary>
    private static ButtonState GetButtonState(MouseState state, MouseButton button) => button switch
    {
        MouseButton.Left => state.LeftButton,
        MouseButton.Right => state.RightButton,
        MouseButton.Middle => state.MiddleButton,
        MouseButton.XButton1 => state.XButton1,
        MouseButton.XButton2 => state.XButton2,
        _ => ButtonState.Released // safe fallback
    };

    private static void Update()
    {
        mouseState = Mouse.GetState();
    }

}
