using FizzleGame.Managers;

namespace FizzleGame.Scenes;

public class MenuScene : SceneBase
{
    public MenuScene(Game1 game, SceneManager sceneManager) : base(game, sceneManager, [])
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

    public override void DrawDebugInfo(SpriteBatch spriteBatch, SpriteFont spriteFont)
    {
        throw new System.NotImplementedException();
    }
}
