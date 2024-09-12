using FizzleMonogameTemplate.Services;
using Microsoft.Xna.Framework.Content;

namespace FizzleMonogameTemplate.Core;
public class SplashScreen(Texture2D texture)
{
    private Texture2D splashImage = texture;
    private float timer;
    private const float DURATION = 3f; // Duration in seconds

    public void Update(GameTime gameTime)
    {
        timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (timer >= DURATION)
        {
            // Transition to a gamestate


        }

    }
    public void Draw() => ServiceLocator.GetService<SpriteBatch>().Draw(splashImage, Vector2.Zero, Color.White);


}