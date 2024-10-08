using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using FizzleGame.Managers;
using FizzleMonogameTemplate.DebugGUI;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;

namespace FizzleGame.Scenes;

public abstract class SceneBase(Game1 game, SceneManager sceneManager, [Optional] IEnumerable<Func<SpriteBatch, ISystem>> systems) : Screen, IDebuggable
{
    protected World world;
    protected Game1 Game { get; } = game;
    protected SpriteBatch SpriteBatch { get; private set; }
    private readonly List<Func<SpriteBatch, ISystem>> pendingSystems = systems?.ToList() ?? new List<Func<SpriteBatch, ISystem>>();
    protected WorldBuilder worldBuilder = new WorldBuilder();
    protected SceneManager SceneManager { get; } = sceneManager;
    protected bool ContentReady { get; set; } = false;
    private bool disposedValue;

    public override void Initialize()
    {
        // Add any initialization logic here
    }

    public override void LoadContent()
    {
        SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
        InitializeSystems();
        world = worldBuilder.Build();
        ContentReady = true;
    }
    private void InitializeSystems()
    {
        foreach (var systemFactory in pendingSystems)
        {
            var system = systemFactory(SpriteBatch);
            worldBuilder.AddSystem(system);
        }
        pendingSystems.Clear();
    }

    public override void Update(GameTime gameTime)
    {
        if (ContentReady)
        {
            world?.Update(gameTime);
            UpdateScene(gameTime);
        }
    }
    public override void Draw(GameTime gameTime)
    {
        if (ContentReady)
        {
            world?.Draw(gameTime);
            DrawScene(gameTime);
        }
    }

    protected virtual void UpdateScene(GameTime gameTime) { }
    protected virtual void DrawScene(GameTime gameTime) { }
    public override void UnloadContent()
    {
        // Unload any content specific to this scene
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                SpriteBatch?.Dispose();
                (world as IDisposable)?.Dispose();
            }

            world = null;
            SpriteBatch = null;

            disposedValue = true;
        }
    }
    ~SceneBase()
    {
        Dispose(disposing: false);
    }

    // Do not change
    public override void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    public abstract void DrawDebugInfo(SpriteBatch spriteBatch, SpriteFont spriteFont);
}
