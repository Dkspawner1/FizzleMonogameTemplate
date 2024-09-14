using System;

namespace FizzleMonogameTemplate.Managers;
public class TransitionManager(Game1 game)
{
    private readonly Game1 game = game;
    private Color currentBackgroundColor = Color.Black;
    private float backgroundTransitionProgress = 0f;
    private const float BackgroundTransitionDuration = 1f;
    private bool isTransitioning = true;
    public Color TargetBackgroundColor { get; set; } = Color.DeepPink;
    public void Update(GameTime gameTime)
    {
        if (!isTransitioning)
        {
            if (backgroundTransitionProgress < 1f)
            {
                backgroundTransitionProgress += (float)gameTime.ElapsedGameTime.TotalSeconds / BackgroundTransitionDuration;
                backgroundTransitionProgress = Math.Min(backgroundTransitionProgress, 1f);
                currentBackgroundColor = Color.Lerp(Color.Black, TargetBackgroundColor, backgroundTransitionProgress);
            }
        }
    }
    public void Draw(SpriteBatch spriteBatch) => game.GraphicsDevice.Clear(currentBackgroundColor);
    public void SetTransitionState(bool transitioning)
    {
        isTransitioning = transitioning;
        if (transitioning)
        {
            ResetBackgroundTransition();
        }
    }
    public void ResetBackgroundTransition()
    {
        currentBackgroundColor = Color.Black;
        backgroundTransitionProgress = 0f;
    }

    public bool IsTransitioning() => isTransitioning;
}