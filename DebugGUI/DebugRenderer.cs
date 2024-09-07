using System;
using System.Collections.Generic;
using ImGuiNET;

namespace FizzleMonogameTemplate.DebugGUI;
public class DebugRenderer
{
    private static readonly Dictionary<Type, Action<DebugProperty, object>> PropertyHandlers = new()
    {
        {typeof(float), HandleFloatProperty},
        {typeof(int), HandleIntProperty}

    };
    public static Action<string, IDebuggable> CreateDebugMenu = (menuName, debuggable)
    =>
    {
        if (ImGui.CollapsingHeader(menuName))
            RenderDebugProperties(debuggable.GetDebugProperties());
    };

    public static void RenderDebugProperties(List<DebugProperty> properties)
    {
        foreach (var property in properties)
        {
            var value = property.Getter();
            if (PropertyHandlers.TryGetValue(property.Type, out var handler))
                handler(property, value);
            else
                ImGui.Text($"{property.Name}: {value}");
        }
    }
    private static void HandleFloatProperty(DebugProperty property, object value)
    {
        float floatValue = (float)value;
        if (ImGui.DragFloat(property.Name, ref floatValue, 0.01f))
            property.Setter(floatValue);
    }
    private static void HandleIntProperty(DebugProperty property, object value)
    {
        int intValue = (int)value;
        if (ImGui.DragInt(property.Name, ref intValue))
            property.Setter(intValue);
    }

}