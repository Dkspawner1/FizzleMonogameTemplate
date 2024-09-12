using FizzleGame.ECS.Systems;
using FizzleMonogameTemplate.DebugGUI.Attributes;

namespace FizzleGame.Scenes;
public class GameScene : SceneBase
{
    [DebugVariable]
    Vector2 test;
    public GameScene() : base([new RenderSystem()])
    {

    }
    public override void Initialize()
    {
    }
    public override void LoadContent()
    {
    }
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
    }
}
