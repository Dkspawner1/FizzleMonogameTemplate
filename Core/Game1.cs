
using System.Diagnostics;
using FizzleGame.Managers;
using System;
using FizzleMonogameTemplate.DebugGUI;
using System.Collections.Generic;
using FizzleMonogameTemplate.Services;

namespace FizzleGame.Core;
public class Game1 : Game, IDebuggable
{
    private readonly SceneManager sceneManager;

    // Test debug variables
    [DebugVariable]
    private float gameSpeed = 1.0f;
    [DebugVariable]
    private bool debugMode = true;
    [DebugVariable]
    private Vector2 playerPosition = new Vector2(100, 100);
    [DebugVariable]
    private Color backgroundColor = Color.DeepPink;
    public List<DebugProperty> GetDebugProperties() => DebuggableHelper.GetDebugProperties(this);
    public Game1()
    {
        var graphicsDeviceManager = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = Data.Window.Width,
            PreferredBackBufferHeight = Data.Window.Height,
            SynchronizeWithVerticalRetrace = true,
        };
        IsFixedTimeStep = true;

        ServiceLocator.RegisterService(graphicsDeviceManager);

        Window.Title = Data.Window.Title;
        Window.AllowUserResizing = true;
        Window.AllowAltF4 = true;
        Window.ClientSizeChanged += WindowClientSizeChanged;

        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        sceneManager = new();
    }

    private void WindowClientSizeChanged(object sender, EventArgs e)
    {
        ScreenManager.UpdateScreenSize(ServiceLocator.GetService<GraphicsDevice>());
        Trace.WriteLine($"Screen's new size: {GraphicsDevice.Viewport.Bounds} Updated to ScreenManager: {ScreenManager.ScreenSize}");
    }
    protected override void Initialize()
    {
        try
        {
            ServiceLocator.RegisterService(GraphicsDevice);
            ServiceLocator.RegisterService(Content);
            ServiceLocator.RegisterService(new SpriteBatch(GraphicsDevice));
            ServiceLocator.RegisterService(sceneManager);
            ServiceLocator.RegisterService(this);

            ScreenManager.Initialize(GraphicsDevice);
            sceneManager.Initialize();
            DebugGUI<Game1>.Initialize();
            DebugGUI<Game1>.RegisterDebuggable("Game", this);

            base.Initialize();
        }
        catch (Exception ex)
        {
            // Log the exception or handle it appropriately
            Console.WriteLine($"Error during initialization: {ex.Message}");
            throw;
        }
    }

    protected override void LoadContent()
    {
        // Create a 1x1 white texture for drawing shapes
        pixel = new Texture2D(GraphicsDevice, 1, 1);
        pixel.SetData(new[] { Color.White });

        sceneManager.LoadContent();

        DebugGUI<Game1>.LoadContent();

    }

    protected override void Update(GameTime gameTime)
    {
        if (Data.Window.Exit){
                DebugGUI<Game1>.UnregisterDebuggable("Game");
            Exit();
        }
        sceneManager.Update(gameTime);

        // Use gameSpeed to adjust update speed
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds * gameSpeed;

        // Update player position (example)
        KeyboardState keyState = Keyboard.GetState();
        if (keyState.IsKeyDown(Keys.W)) playerPosition.Y -= 100 * deltaTime;
        if (keyState.IsKeyDown(Keys.S)) playerPosition.Y += 100 * deltaTime;
        if (keyState.IsKeyDown(Keys.A)) playerPosition.X -= 100 * deltaTime;
        if (keyState.IsKeyDown(Keys.D)) playerPosition.X += 100 * deltaTime;


        base.Update(gameTime);
    }
    private Texture2D pixel;
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(backgroundColor);

        sceneManager.Draw(gameTime);

        // Draw a rectangle representing the player
        var spriteBatch = ServiceLocator.GetService<SpriteBatch>();
        spriteBatch.Begin();
        spriteBatch.Draw(pixel, new Rectangle((int)playerPosition.X, (int)playerPosition.Y, 32, 32), Color.Red);
        spriteBatch.End();

        DebugGUI<Game1>.Draw(gameTime);


        base.Draw(gameTime);
    }
}
