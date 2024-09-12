
using System;
using FizzleMonogameTemplate.Services;

namespace FizzleMonogameTemplate.Managers;
public class TransitionManager
{
    private static TransitionManager instance;
    public static TransitionManager Instance => instance ?? (instance = new());

    private Texture2D fadeTexture;
    private float fadeAlpha;
    private bool isFading;
    private Action onTransitionComplete;

    public void Initialize()
    {
        fadeTexture = new Texture2D(ServiceLocator.GetService<GraphicsDevice>(), 1, 1);
        fadeTexture.SetData(new[] { Color.Black });
    }
    public void StartTransition(Action onComplete)
    {

        isFading = true;
        fadeAlpha = 0f;
        onTransitionComplete = onComplete;
    }

    public void Update(in GameTime gameTime)
    {
        if (isFading)
        {

            fadeAlpha += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (fadeAlpha >= 1f)
            {
                isFading = false;
                onTransitionComplete?.Invoke();
            }
        }
    }
    public void Draw()
    {
        if (fadeAlpha > 0f)
        {
            var viewport = ServiceLocator.GetService<GraphicsDevice>().Viewport;
            ServiceLocator.GetService<SpriteBatch>().Draw(fadeTexture, new Rectangle(0, 0, viewport.Width, viewport.Height), Color.Black * fadeAlpha);
        }
    }
}