using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using FizzleGame.Managers;
using FizzleGame.Scenes;
using FizzleMonogameTemplate.DebugGUI;
using FizzleMonogameTemplate.DebugGUI.Attributes;
using System;
using MonoGame.Extended.Screens.Transitions;

namespace FizzleGame.Core;

public class Game1 : Game, IDebuggable
{
    private SceneManager sceneManager;
    private readonly ScreenManager screenManager;
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    private Texture2D pixel;

    [DebugVariable(true)]
    private float gameSpeed = 1.0f;
    [DebugVariable]
    private bool debugMode = true;
    [DebugVariable(true)]
    private Vector2 playerPosition = new(100, 100);
    [DebugVariable(true)]
    private Color backgroundColor = Color.DeepPink;

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = Data.Window.Width,
            PreferredBackBufferHeight = Data.Window.Height,
            SynchronizeWithVerticalRetrace = true,
        };
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        IsFixedTimeStep = true;

        Window.Title = Data.Window.Title;
        Window.AllowUserResizing = true;
        Window.AllowAltF4 = true;

        screenManager = new ScreenManager();
        Components.Add(screenManager);
    }



    protected override void Initialize()
    {

        spriteBatch = new SpriteBatch(GraphicsDevice);

        sceneManager = new SceneManager(this, screenManager);
        sceneManager.Initialize();

        DebugGUI<Game1>.Initialize(this);
        DebugGUI<Game1>.RegisterDebuggable("Game", this);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        pixel = new Texture2D(GraphicsDevice, 1, 1);
        pixel.SetData(new[] { Color.White });

        sceneManager.LoadContent();

        sceneManager.ChangeScene(SCENES.MENU, new FadeTransition(GraphicsDevice, Color.Black));
        sceneManager.ChangeScene(SCENES.MENU);

        DebugGUI<Game1>.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        if (Data.Window.Exit)
        {
            DebugGUI<Game1>.UnregisterDebuggable("Game");
            Exit();
        }

        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds * gameSpeed;

        UpdatePlayerPosition(deltaTime);

        base.Update(gameTime);
    }

    private void UpdatePlayerPosition(float deltaTime)
    {
        KeyboardState keyState = Keyboard.GetState();
        if (keyState.IsKeyDown(Keys.W)) playerPosition.Y -= 100 * deltaTime;
        if (keyState.IsKeyDown(Keys.S)) playerPosition.Y += 100 * deltaTime;
        if (keyState.IsKeyDown(Keys.A)) playerPosition.X -= 100 * deltaTime;
        if (keyState.IsKeyDown(Keys.D)) playerPosition.X += 100 * deltaTime;
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(backgroundColor);

        spriteBatch.Begin();
        spriteBatch.Draw(pixel, new Rectangle((int)playerPosition.X, (int)playerPosition.Y, 32, 32), Color.Red);
        spriteBatch.End();

        DebugGUI<Game1>.Draw(gameTime);

        base.Draw(gameTime);
    }
}