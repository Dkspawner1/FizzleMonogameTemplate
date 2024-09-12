using System.Collections.Generic;
using System.Linq;
using FizzleGame.Scenes;
using FizzleMonogameTemplate.Managers;

namespace FizzleGame.Managers;

public class SceneManager
{
    private readonly Dictionary<SCENES, SceneBase> scenes;
    private SceneBase currentScene;
    public SceneManager()
    {
        scenes = new()
        {
            { SCENES.MENU, new MenuScene() },
            { SCENES.GAME, new GameScene() }
        };
        currentScene = scenes[SCENES.GAME];
    }

    public void Initialize()
    {
        foreach (var scene in scenes.Values.Where(s => s is not null))
            scene.Initialize();
    }
    public void LoadContent()
    {
        foreach (var scene in scenes.Values.Where(s => s is not null))
            scene.LoadContent();
    }
    public void Update(GameTime gameTime)
    {
        currentScene?.Update(gameTime);
    }

    public void Draw(GameTime gameTime)
    {
        currentScene?.Draw(gameTime);
    }


    public void ChangeState(SceneBase newScene)
    {
        TransitionManager.Instance.StartTransition(() =>
        {
            currentScene = newScene;
            currentScene.LoadContent();
        });
    }

}
