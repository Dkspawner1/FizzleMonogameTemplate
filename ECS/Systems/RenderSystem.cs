using FizzleGame.Singletons.Classes;
using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Graphics;

namespace FizzleGame.ECS.Systems;

public class RenderSystem : EntityDrawSystem
{
    private static SpriteBatch spriteBatch => SpriteBatchSingleton.Instance.SpriteBatch;
    public RenderSystem() : base(Aspect.All(typeof(Sprite)))
    { }
    public override void Initialize(IComponentMapperService mapperService)
    {
    }
    public override void Draw(GameTime gameTime)
    {
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
        spriteBatch.End();

    }
}
