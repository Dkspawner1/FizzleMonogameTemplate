using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using FizzleMonogameTemplate.DebugGUI;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Screens;

namespace FizzleGame.Scenes;

public abstract class SceneBase : GameScreen, IDebuggable
{
    protected World world;
    protected new Game1 Game => (Game1)base.Game;
    protected SpriteBatch SpriteBatch { get; private set; }
    private readonly List<Func<SpriteBatch, ISystem>> pendingSystems;
    protected WorldBuilder worldBuilder;

    // protected SceneBase(Game1 game, [Optional] IEnumerable<ISystem> systems) : base(game) => world = (systems ?? Enumerable.Empty<ISystem>())
    //         .Aggregate(new WorldBuilder(), (builder, system) => builder.AddSystem(system))
    //         .Build();
    protected SceneBase(Game1 game, [Optional] IEnumerable<Func<SpriteBatch, ISystem>> systems) : base(game)
    {
        pendingSystems = systems?.ToList() ?? new List<Func<SpriteBatch, ISystem>>();
        worldBuilder = new WorldBuilder();
    }

    public override void Initialize()
    {

    }
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

    public override void Update(GameTime gameTime) => world.Update(gameTime);

    public override void Draw(GameTime gameTime) => world.Draw(gameTime);
    public override void UnloadContent() => SpriteBatch?.Dispose();

    public virtual void DrawDebugInfo(SpriteBatch spriteBatch, SpriteFont spriteFont)
    {

    }

    // public virtual void Update(GameTime gameTime) => world.Update(gameTime);
    // public virtual void Draw(GameTime gameTime) => world.Draw(gameTime);

}
