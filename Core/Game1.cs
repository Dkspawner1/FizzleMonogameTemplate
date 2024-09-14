using MonoGame.Extended.Screens;
using FizzleMonogameTemplate.DebugGUI;
using FizzleMonogameTemplate.DebugGUI.Attributes;
using FizzleGame.Scenes;
using FizzleGame.Managers;
using MonoGame.Extended.Screens.Transitions;
using FizzleMonogameTemplate.Managers;

namespace FizzleGame.Core;

public class Game1 : Game, IDebuggable
{
    private SceneManager sceneManager;
    private readonly ScreenManager screenManager;
    private SpriteBatch spriteBatch;
    private Texture2D pixel;
    private TransitionManager transitionManager;



    [DebugVariable(true)]
    private float gameSpeed = 1.5f;
    [DebugVariable]
    private bool debugMode = true;
    [DebugVariable(true)]
    private Vector2 playerPosition = new(100, 100);
    [DebugVariable(true)]
    private Color backgroundColor = Color.DeepPink;

    public Game1()
    {
        _ = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = Data.Window.Width,
            PreferredBackBufferHeight = Data.Window.Height,
            SynchronizeWithVerticalRetrace = true,
        };
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        IsFixedTimeStep = true;

        Window.Title = Data.Window.Title;
        Window.AllowUserResizing = true;
        Window.AllowAltF4 = true;

        screenManager = new ScreenManager();
        Components.Add(screenManager);
    }

    protected override void Initialize()
    {

        DebugGUI<Game1>.Initialize(this);
        DebugGUI<Game1>.RegisterDebuggable("Game", this);

        transitionManager = new(this);
        transitionManager.TargetBackgroundColor = backgroundColor;

        sceneManager = new(this, screenManager);
        sceneManager.ChangeScene(SCENES.GAME);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);

        pixel = new Texture2D(GraphicsDevice, 1, 1);
        pixel.SetData(new[] { Color.White });

        DebugGUI<Game1>.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        if (Data.Window.Exit)
        {
            DebugGUI<Game1>.UnregisterDebuggable("Game");
            Exit();
        }
        transitionManager.Update(gameTime);


        if (!transitionManager.IsTransitioning())
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds * gameSpeed;
            UpdatePlayerPosition(deltaTime);
        }

        base.Update(gameTime);
    }

    private void UpdatePlayerPosition(float deltaTime)
    {
        KeyboardState keyState = Keyboard.GetState();
        if (keyState.IsKeyDown(Keys.W)) playerPosition.Y -= 100 * deltaTime;
        if (keyState.IsKeyDown(Keys.S)) playerPosition.Y += 100 * deltaTime;
        if (keyState.IsKeyDown(Keys.A)) playerPosition.X -= 100 * deltaTime;
        if (keyState.IsKeyDown(Keys.D)) playerPosition.X += 100 * deltaTime;
    }

    protected override void Draw(GameTime gameTime)
    {
        transitionManager.Draw(spriteBatch);

        if (!transitionManager.IsTransitioning())
        {
            spriteBatch.Begin();
            spriteBatch.Draw(pixel, new Rectangle((int)playerPosition.X, (int)playerPosition.Y, 32, 32), Color.Red);
            spriteBatch.End();

            DebugGUI<Game1>.Draw(gameTime);
        }
        base.Draw(gameTime);
    }
    public void SetTransitionState(bool transitioning) => transitionManager.SetTransitionState(transitioning);
}