using System;
using MonoGame.Extended;
using MonoGame.Extended.Screens.Transitions;

namespace FizzleMonogameTemplate.Transitions
{
    public class FadeOutInTransition(GraphicsDevice graphicsDevice, Color color, float duration = 1f) : Transition(duration)
    {
        private readonly GraphicsDevice graphicsDevice = graphicsDevice ?? throw new ArgumentNullException(nameof(graphicsDevice));
        private readonly SpriteBatch spriteBatch = new(graphicsDevice);
        private readonly Color color = color;

        public override void Draw(GameTime gameTime)
        {
            // Start from black (alpha = 1) and fade to transparent (alpha = 0)
            float alpha = Value;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            spriteBatch.FillRectangle(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height, color * alpha);
            spriteBatch.End();
        }

        public override void Dispose() => spriteBatch.Dispose();
    }
}