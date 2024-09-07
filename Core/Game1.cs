namespace FizzleGame.Core;

using System.Diagnostics;
using FizzleGame.Singletons.Classes;
using FizzleGame.Managers;
using static FizzleGame.Core.Data;
using System;
using FizzleMonogameTemplate.DebugGUI;
using System.Collections.Generic;

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
    private Color backgroundColor = Color.CornflowerBlue;
    public List<DebugProperty> GetDebugProperties()
    {
        return new List<DebugProperty>
        {
            new DebugProperty("Game Speed", typeof(float), () => gameSpeed, value => gameSpeed = (float)value),
            new DebugProperty("Debug Mode", typeof(bool), () => debugMode, value => debugMode = (bool)value),
            new DebugProperty("Player Position", typeof(Vector2), () => playerPosition, value => playerPosition = (Vector2)value),
            new DebugProperty("Background Color", typeof(Color), () => backgroundColor, value => backgroundColor = (Color)value)
        };
    }
    public Game1()
    {

        _ = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = window.Width,
            PreferredBackBufferHeight = window.Height
        };

        Window.Title = window.Title;
        Window.AllowUserResizing = true;
        Window.AllowAltF4 = true;
        Window.ClientSizeChanged += WindowClientSizeChanged;

        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        sceneManager = new();
    }

    private void WindowClientSizeChanged(object sender, EventArgs e)
    {
        ScreenManager.UpdateScreenSize(SpriteBatchSingleton.Instance.SpriteBatch.GraphicsDevice);
        Trace.WriteLine($"Screen's new size: {GraphicsDevice.Viewport.Bounds} Updated to ScreenManager: {ScreenManager.ScreenSize}");
    }
    protected override void Initialize()
    {
        ScreenManager.Initialize(GraphicsDevice);
        ContentLoaderSingleton.Initialize(Content);
        SpriteBatchSingleton.Initialize(GraphicsDevice);

        sceneManager.Initialize();

        DebugGUI<Game1>.Initialize(this);
        DebugGUI<Game1>.RegisterDebuggable("Game", this);

        base.Initialize();
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
        if (window.Exit)
            Exit();
        sceneManager.Update(gameTime);

        // Use _gameSpeed to adjust update speed
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
        GraphicsDevice.Clear(Color.DeepPink);

        sceneManager.Draw(gameTime);

        // Draw a rectangle representing the player

        SpriteBatchSingleton.Instance.SpriteBatch.Begin();
        SpriteBatchSingleton.Instance.SpriteBatch.Draw(pixel, new Rectangle((int)playerPosition.X, (int)playerPosition.Y, 32, 32), Color.Red);

        SpriteBatchSingleton.Instance.SpriteBatch.End();



        base.Draw(gameTime);
    }
}
