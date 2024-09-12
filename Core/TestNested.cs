
using FizzleGame.ECS.Systems;
using FizzleMonogameTemplate.DebugGUI;
using FizzleMonogameTemplate.DebugGUI.Attributes;

namespace FizzleMonogameTemplate.Core
{
    public class TestNested : IDebuggable
    {

        [DebugVariable(true)]
        public string SceneName { get; set; } = "Default Scene";

        [DebugVariable]
        public int EntityCount { get; private set; } = 0;

        [DebugVariable(true)]
        public Vector2 PlayerPosition { get; set; } = new Vector2(0, 0);

        [DebugVariable]
        public RenderSystem RenderSystem { get; } = new RenderSystem();

        [DebugVariable(true)]
        public Color AmbientLight { get; set; } = Color.White;

        public void AddEntity() => EntityCount++;
    }
}