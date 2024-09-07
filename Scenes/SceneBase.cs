using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using MonoGame.Extended.ECS.Systems;

namespace FizzleGame.Scenes;

public abstract class SceneBase
{
    protected World world;
    public SceneBase([Optional] IEnumerable<ISystem> systems) => world =
    (systems ?? Enumerable.Empty<ISystem>())
        .Aggregate(new WorldBuilder(), (builder, system) => builder.AddSystem(system))
        .Build();
    public abstract void Initialize();
    public abstract void LoadContent();
    public virtual void Update(GameTime gameTime) => world.Update(gameTime);
    public virtual void Draw(GameTime gameTime) => world.Draw(gameTime);

}
