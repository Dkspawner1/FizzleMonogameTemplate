namespace FizzleGame.Managers;
public static class ScreenManager
{
    private static Vector2 screenSize;
    public static Vector2 ScreenSize => screenSize;

    public static void Initialize(GraphicsDevice graphics) => screenSize = new(graphics.Viewport.Width, graphics.Viewport.Height);
    public static void UpdateScreenSize(GraphicsDevice graphics) => screenSize = new(graphics.Viewport.Width, graphics.Viewport.Height);

}
