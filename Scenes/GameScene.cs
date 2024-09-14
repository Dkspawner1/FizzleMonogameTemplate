using System;
using FizzleGame.ECS.Systems;
using FizzleGame.Managers;
using FizzleMonogameTemplate.DebugGUI.Attributes;
using MonoGame.Extended.ECS.Systems;

namespace FizzleGame.Scenes;
public class GameScene : SceneBase
{
    [DebugVariable]
    Vector2 test;

    public GameScene(Game1 game, SceneManager sceneManager)
       : base(game, sceneManager, [spriteBatch => new RenderSystem(spriteBatch),])
    {
    }
    public override void Initialize()
    {
        base.Initialize();
    }
    public override void LoadContent()
    {
        base.LoadContent();
    }
    protected override void UpdateScene(GameTime gameTime)
    {
        // Add game-specific update logic here
        // This will only be called after the fade-in transition is complete
    }

    protected override void DrawScene(GameTime gameTime)
    {
        // Add game-specific drawing logic here
        // This will only be called after the fade-in transition is complete
    }


    public override void DrawDebugInfo(SpriteBatch spriteBatch, SpriteFont spriteFont)
    {
        // Implement your debug drawing logic here
        // For example:
        spriteBatch.DrawString(spriteFont, $"Test Vector: {test}", new Vector2(10, 10), Color.White);
    }

}
