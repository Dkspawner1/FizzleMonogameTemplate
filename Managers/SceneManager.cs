using System;
using FizzleGame.Scenes;
using FizzleMonogameTemplate.Transitions;
using MonoGame.Extended.Screens;

namespace FizzleGame.Managers;

public class SceneManager(Game1 game, ScreenManager screenManager)
{
    private readonly Game1 game = game;
    private readonly ScreenManager screenManager = screenManager;

    public SceneBase CurrentScene { get; private set; }

    public void ChangeScene(SCENES sceneType)
    {
        SceneBase newScene = CreateScene(sceneType);
        var transition = new FadeOutInTransition(game.GraphicsDevice, Color.Black, 3f);
        transition.StateChanged += (s, e) => game.SetTransitionState(false);
        screenManager.LoadScreen(newScene, transition);
        CurrentScene = newScene;
    }
    public SCENES GetCurrentSceneType() => CurrentScene switch
    {
        MenuScene => SCENES.MENU,
        GameScene => SCENES.GAME,
        _ => throw new InvalidOperationException("Unknown scene type")
    };

    private SceneBase CreateScene(SCENES sceneType) => sceneType switch
    {
        SCENES.MENU => new MenuScene(game, this),
        SCENES.GAME => new GameScene(game, this),
        _ => throw new ArgumentException("Invalid scene type", nameof(sceneType))
    };
}