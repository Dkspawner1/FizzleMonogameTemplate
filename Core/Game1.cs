namespace FizzleGame.Core;

using System.Diagnostics;
using FizzleGame.Singletons.Classes;
using FizzleGame.Managers;
using static FizzleGame.Core.Data;
using System;

public class Game1 : Game
{
    private readonly SceneManager sceneManager;

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

    private void WindowClientSizeChanged(object sender, EventArgs e) => ScreenManager.UpdateScreenSize(SpriteBatchSingleton.Instance.SpriteBatch.GraphicsDevice);

    protected override void Initialize()
    {
        ScreenManager.Initialize(GraphicsDevice);
        ContentLoaderSingleton.Initialize(Content);
        SpriteBatchSingleton.Initialize(GraphicsDevice);

        sceneManager.Initialize();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        sceneManager.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        if (window.Exit)
            Exit();
        sceneManager.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.DeepPink);

        sceneManager.Draw(gameTime);

        base.Draw(gameTime);
    }
}
