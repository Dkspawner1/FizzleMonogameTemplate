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
        scenes = new()
        {
            { SCENES.MENU, new MenuScene(game) },
            { SCENES.GAME, new GameScene(game) }
        };
    }

    public void Initialize()
    {
        foreach (var scene in scenes.Values)
            scene.Initialize();
    }
    public void LoadContent()
    {
        foreach (var scene in scenes.Values)
            scene.LoadContent();
    }
    public void ChangeScene(SCENES sceneType, Transition transition = null)
    {

        if (scenes.TryGetValue(sceneType, out var newScene))
        {

            if (transition is not null)
                screenManager.LoadScreen(newScene, transition);
            else
                screenManager.LoadScreen(newScene);

        }

    }

}
