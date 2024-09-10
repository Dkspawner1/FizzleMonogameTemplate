using System;
using System.Linq;
using System.Collections.Concurrent;
using FizzleMonogameTemplate.Services;
using ImGuiNET;
using MonoGame.ImGuiNet;
using ImGuiVector2 = System.Numerics.Vector2;
using System.Collections.Generic;

namespace FizzleMonogameTemplate.DebugGUI;

public class DebugGUI<T> where T : IDebuggable
{
    private readonly T debuggable;
    private Dictionary<Type, List<DebugProperty>> groupedProperties;

    public DebugGUI(T debuggable)
    {
        this.debuggable = debuggable;
        RefreshProperties();
    }

    private void RefreshProperties()
    {
        var allProperties = DebuggableHelper.GetDebugProperties(debuggable, true);
        groupedProperties = allProperties.GroupBy(p => p.DeclaringType)
                                         .ToDictionary(g => g.Key, g => g.ToList());
    }

    public static ImGuiRenderer GuiRenderer { get; private set; }

    private static readonly ConcurrentDictionary<string, IDebuggable> debuggableObjects = new();

    public static void SetDebugWindowSize(ImGuiVector2 size) => debugWindowSize = size;
    public static void SetDebugWindowPosition(ImGuiVector2 position) => debugWindowPosition = position;
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
    public static void ToggleDebugWindow() => showDebugWindow = !showDebugWindow;

    public static void RegisterDebuggable(string name, IDebuggable debuggable)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Name cannot be null or empty", nameof(name));
        if (debuggable == null)
            throw new ArgumentNullException(nameof(debuggable));

        debuggableObjects[name] = debuggable;
    }

    public static void UnregisterDebuggable(string name) => debuggableObjects.TryRemove(name, out _);

    public static void Draw(GameTime gameTime)
    {
        ImGui.SetNextWindowSize(debugWindowSize, ImGuiCond.FirstUseEver);
        ImGui.SetNextWindowPos(debugWindowPosition, ImGuiCond.FirstUseEver);
        GuiRenderer.BeginLayout(gameTime);

        if (ImGui.BeginMainMenuBar())
        {
            if (ImGui.BeginMenu("Debug"))
            {
                ImGui.MenuItem("Show Debug Window", "", ref showDebugWindow);
                ImGui.EndMenu();
            }
            ImGui.EndMainMenuBar();
        }

        if (showDebugWindow)
        {
            ImGui.Begin("Debug Menu");
            ImGui.Text($"FPS: {1 / gameTime.ElapsedGameTime.TotalSeconds:F2}");

            foreach (var debuggablePair in debuggableObjects)
            {
                if (ImGui.CollapsingHeader(debuggablePair.Key))
                {
                    var debuggable = debuggablePair.Value;
                    var debugGui = new DebugGUI<IDebuggable>(debuggable);
                    debugGui.RenderGroupedProperties();
                }
            }

            ImGui.End();
        }

        GuiRenderer.EndLayout();
    }

    private void RenderGroupedProperties()
    {
        foreach (var group in groupedProperties)
        {
            if (ImGui.TreeNode(group.Key.Name))
            {
                DebugRenderer.RenderDebugProperties(group.Value);
                ImGui.TreePop();
            }
        }
    }
}