using System;
using FizzleGame.Scenes;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;

namespace FizzleGame.Managers;

public class SceneManager(Game1 game, ScreenManager screenManager)
{
    private readonly Game1 game = game;
    private readonly ScreenManager screenManager = screenManager;
    public SceneBase CurrentScene { get; private set; }

    public void ChangeScene(SCENES sceneType)
    {
        SceneBase newScene = CreateScene(sceneType);
        Transition transition = CreateTransition();
        screenManager.LoadScreen(newScene, transition);
        CurrentScene = newScene;
    }
    public SCENES GetCurrentSceneType()
    {
        return CurrentScene switch
        {
            MenuScene => SCENES.MENU,
            GameScene => SCENES.GAME,
            _ => throw new InvalidOperationException("Unknown scene type")
        };
    }
    private SceneBase CreateScene(SCENES sceneType)
    {
        return sceneType switch
        {
            SCENES.MENU => new MenuScene(game, this),
            SCENES.GAME => new GameScene(game, this),
            _ => throw new ArgumentException("Invalid scene type", nameof(sceneType))
        };
    }
    private Transition CreateTransition()
    {
        return new FadeTransition(game.GraphicsDevice, Color.Black, 5f);
    }
}