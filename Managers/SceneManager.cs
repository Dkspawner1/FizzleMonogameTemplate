using System;
using System.Collections.Generic;
using System.Linq;
using FizzleGame.Scenes;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;

namespace FizzleGame.Managers;

public class SceneManager
{
    private readonly Game1 game;
    private readonly ScreenManager screenManager;
    private readonly Dictionary<SCENES, SceneBase> scenes;
    public SceneManager(Game1 game, ScreenManager screenManager)
    {
        this.game = game;
        this.screenManager = screenManager;
    }
    public void ChangeScene(SCENES sceneType, Transition transition = null)
    {
        SceneBase newScene = sceneType switch
        {
            SCENES.MENU => new MenuScene(game, this),
            SCENES.GAME => new GameScene(game, this),
            _ => throw new ArgumentException("Invalid scene type", nameof(sceneType))
        };

        if (transition != null)
            screenManager.LoadScreen(newScene, transition);
        else
            screenManager.LoadScreen(newScene);
    }
}