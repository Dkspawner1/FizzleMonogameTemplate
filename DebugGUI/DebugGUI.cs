using System.Collections.Generic;

using ImGuiNET;
using MonoGame.ImGuiNet;
namespace FizzleMonogameTemplate.DebugGUI;
public static class DebugGUI<TGame> where TGame : Game
{

    public static ImGuiRenderer GuiRenderer { get; private set; }
    private static List<(string Name, IDebuggable Object)> debuggableObjects = [];
    public static void Initialize(TGame game) => GuiRenderer = new(game);

    public static void LOadContent() => GuiRenderer.RebuildFontAtlas();
    private static bool toolActive = true;

    public static void RegisterDebuggable(string name, IDebuggable debuggable) => debuggableObjects.Add((name, debuggable));
    public static void UnregisterDebuggable(string name) => debuggableObjects.RemoveAll(item => item.Name == name);
    public static void Draw(in GameTime gameTime)
    {
        GuiRenderer.BeginLayout(gameTime);

        if (ImGui.BeginMainMenuBar())
        {
            if (ImGui.BeginMenu("Debug"))
            {
                ImGui.MenuItem("Show Debug Window", "", ref toolActive);
                ImGui.EndMenu();
            }
            ImGui.EndMainMenuBar();
        }

        if (toolActive)
        {
            DrawDebugMenus(gameTime);
        }

        GuiRenderer.EndLayout();
    }
    private static void DrawDebugMenus(GameTime gameTime)
    {
        ImGui.Begin("Debug Menus");

        ImGui.Text($"FPS: {1 / gameTime.ElapsedGameTime.TotalSeconds:F2}");

        foreach (var (name, debuggable) in debuggableObjects)
        {
            if (ImGui.CollapsingHeader(name))
            {
                DebugRenderer.CreateDebugMenu(name, debuggable);
            }
        }

        ImGui.End();
    }

}