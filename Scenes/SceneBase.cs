using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using FizzleGame.Managers;
using FizzleMonogameTemplate.DebugGUI;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Screens;

namespace FizzleGame.Scenes;

public abstract class SceneBase(Game1 game, SceneManager sceneManager, [Optional] IEnumerable<Func<SpriteBatch, ISystem>> systems) : GameScreen(game), IDebuggable
{
    protected World world;
    protected new Game1 Game => (Game1)base.Game;
    protected SpriteBatch SpriteBatch { get; private set; }
    private readonly List<Func<SpriteBatch, ISystem>> pendingSystems = systems?.ToList() ?? new List<Func<SpriteBatch, ISystem>>();
    protected WorldBuilder worldBuilder = new WorldBuilder();
    protected SceneManager SceneManager { get; } = sceneManager;

    public override void LoadContent()
    {
        SpriteBatch = new SpriteBatch(GraphicsDevice);
        InitializeSystems();
        world = worldBuilder.Build();
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

    public override void Update(GameTime gameTime) => world?.Update(gameTime);

    public override void Draw(GameTime gameTime) => world?.Draw(gameTime);
    public override void UnloadContent() => SpriteBatch?.Dispose();

    public virtual void DrawDebugInfo(SpriteBatch spriteBatch, SpriteFont spriteFont)
    {
    }
}
