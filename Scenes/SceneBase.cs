using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using FizzleMonogameTemplate.DebugGUI;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Screens;

namespace FizzleGame.Scenes;

public abstract class SceneBase : Screen, IDebuggable
{
    protected World world;

    public SceneBase([Optional] IEnumerable<ISystem> systems) => world =
    (systems ?? Enumerable.Empty<ISystem>())
        .Aggregate(new WorldBuilder(), (builder, system) => builder.AddSystem(system))
        .Build();
    public override void Initialize()
    {

    }
    public override void LoadContent()
    {
    }
    public override void Update(GameTime gameTime) => world.Update(gameTime);

    public override void Draw(GameTime gameTime) => world.Draw(gameTime);

    // public virtual void Update(GameTime gameTime) => world.Update(gameTime);
    // public virtual void Draw(GameTime gameTime) => world.Draw(gameTime);

}
