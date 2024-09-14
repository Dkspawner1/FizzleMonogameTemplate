using System;
using FizzleGame.ECS.Systems;
using FizzleMonogameTemplate.DebugGUI.Attributes;
using MonoGame.Extended.ECS.Systems;

namespace FizzleGame.Scenes;
public class GameScene : SceneBase
{
    [DebugVariable]
    Vector2 test;
    private readonly Game1 game;

    public GameScene(Game1 game) : base(game, [spriteBatch => new RenderSystem(spriteBatch),])
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
