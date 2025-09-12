using Microsoft.Xna.Framework.Input;

public static class KeyboardHelper
{
    private static KeyboardState _currentState;
    private static KeyboardState _previousState;

    /// <summary>
    /// Must be called once per frame to update the keyboard state.
    /// Call this at the start of Update().
    /// </summary>
    public static void Update()
    {
        _previousState = _currentState;
        _currentState = Keyboard.GetState();
    }

    /// <summary>
    /// Returns true while the key is being held down.
    /// </summary>
    public static bool IsHeld(Keys key) => _currentState.IsKeyDown(key);

    /// <summary>
    /// Returns true while the key is not being held down.
    /// </summary>
    public static bool IsReleased(Keys key) => _currentState.IsKeyUp(key);

    /// <summary>
    /// Returns true only on the frame the key is pressed (transition from Up → Down).
    /// </summary>
    public static bool JustPressed(Keys key) =>
        _currentState.IsKeyDown(key) && _previousState.IsKeyUp(key);

    /// <summary>
    /// Returns true only on the frame the key is released (transition from Down → Up).
    /// </summary>
    public static bool JustReleased(Keys key) =>
        _currentState.IsKeyUp(key) && _previousState.IsKeyDown(key);
}
