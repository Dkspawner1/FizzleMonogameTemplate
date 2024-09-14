using MonoGame.Extended.ECS.Systems;
using MonoGame.Extended.Graphics;

namespace FizzleGame.ECS.Systems;

public class RenderSystem : EntityDrawSystem
{
    private readonly SpriteBatch spriteBatch;
    public RenderSystem(SpriteBatch spriteBatch) : base(Aspect.All(typeof(Sprite))) => this.spriteBatch = spriteBatch;
    public override void Initialize(IComponentMapperService mapperService)
    {
    }
    public override void Draw(GameTime gameTime)
    {
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
        spriteBatch.End();

    }
}
