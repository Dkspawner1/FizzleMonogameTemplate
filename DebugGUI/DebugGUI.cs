using System;
using System.Linq;
using System.Collections.Concurrent;
using FizzleMonogameTemplate.Services;
using ImGuiNET;
using MonoGame.ImGuiNet;
using ImGuiVector2 = System.Numerics.Vector2;

namespace FizzleMonogameTemplate.DebugGUI;

public static class DebugGUI<Debuggable>
{
    #pragma warning disable CS0436 // Type conflicts with imported type
    public static ImGuiRenderer GuiRenderer { get; private set; }
    #pragma warning restore CS0436 // Type conflicts with imported type2
    
    private static readonly ConcurrentDictionary<string, IDebuggable> debuggableObjects = new();
    private static ImGuiVector2 debugWindowSize = new(300, 200);
    private static ImGuiVector2 debugWindowPosition = new(10, 10);
    private static bool showDebugWindow = true;

    public static void Initialize()
    {
        if (GuiRenderer != null)
            throw new InvalidOperationException("DebugGUI has already been initialized.");
        GuiRenderer = new(ServiceLocator.GetService<Game1>());
    }

    public static void LoadContent() => GuiRenderer.RebuildFontAtlas();

    public static void RegisterDebuggable(string name, IDebuggable debuggable)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Name cannot be null or empty", nameof(name));
        if (debuggable == null)
            throw new ArgumentNullException(nameof(debuggable));
        
        debuggableObjects[name] = debuggable;
    }

    public static void UnregisterDebuggable(string name)
    {
        debuggableObjects.TryRemove(name, out _);
    }

    public static void Draw(in GameTime gameTime)
    {
        ImGui.SetNextWindowSize(debugWindowSize, ImGuiCond.FirstUseEver);
        ImGui.SetNextWindowPos(debugWindowPosition, ImGuiCond.FirstUseEver);
        GuiRenderer.BeginLayout(gameTime);

        // Add the menu bar
        if (ImGui.BeginMainMenuBar())
        {
            if (ImGui.BeginMenu("Debug"))
            {
                ImGui.MenuItem("Show Debug Window", "", ref showDebugWindow);
                ImGui.EndMenu();
            }
            ImGui.EndMainMenuBar();
        }

        // Only show the debug window if toggled on
        if (showDebugWindow)
        {
            ImGui.Begin("Debug Menu");
            ImGui.Text($"FPS: {1 / gameTime.ElapsedGameTime.TotalSeconds:F2}");

            foreach (var kvp in debuggableObjects)
            {
                if (ImGui.CollapsingHeader(kvp.Key))
                    DebugRenderer.RenderDebugProperties(kvp.Value.GetDebugProperties());
            }

            ImGui.End();
        }

        GuiRenderer.EndLayout();
    }
}