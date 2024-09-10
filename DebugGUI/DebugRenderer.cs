using System;
using System.Collections.Generic;
using ImGuiNET;
using XnaVector2 = Microsoft.Xna.Framework.Vector2;
using XnaVector3 = Microsoft.Xna.Framework.Vector3;
using XnaVector4 = Microsoft.Xna.Framework.Vector4;
using XnaColor = Microsoft.Xna.Framework.Color;
using ImGuiVector2 = System.Numerics.Vector2;
using ImGuiVector3 = System.Numerics.Vector3;
using ImGuiVector4 = System.Numerics.Vector4;

namespace FizzleMonogameTemplate.DebugGUI;

public static class DebugRenderer
{
    private static readonly Dictionary<Type, Action<DebugProperty, object, bool>> PropertyHandlers = new()
    {
        { typeof(float), HandleFloatProperty },
        { typeof(int), HandleIntProperty },
        { typeof(bool), HandleBoolProperty },
        { typeof(string), HandleStringProperty },
        { typeof(XnaVector2), HandleVector2Property },
        { typeof(XnaVector3), HandleVector3Property },
        { typeof(XnaColor), HandleColorProperty }
    };

    public static void RenderDebugProperties(List<DebugProperty> properties)
    {
        foreach (var property in properties)
        {
            var value = property.Getter();
            bool isEditable = property.Setter != null;
            if (PropertyHandlers.TryGetValue(property.Type, out var handler))
            {
                handler(property, value, isEditable);
            }
            else
            {
                ImGui.Text($"{property.Name}: {value}");
            }
        }
    }

    private static void HandleFloatProperty(DebugProperty property, object value, bool isEditable)
    {
        float floatValue = (float)value;
        if (isEditable)
        {
            if (ImGui.DragFloat(property.Name, ref floatValue, 0.01f))
            {
                property.Setter(floatValue);
            }
        }
        else
        {
            ImGui.Text($"{property.Name}: {floatValue}");
        }
    }

    private static void HandleIntProperty(DebugProperty property, object value, bool isEditable)
    {
        int intValue = (int)value;
        if (isEditable)
        {
            if (ImGui.DragInt(property.Name, ref intValue))
            {
                property.Setter(intValue);
            }
        }
        else
        {
            ImGui.Text($"{property.Name}: {intValue}");
        }
    }

    private static void HandleBoolProperty(DebugProperty property, object value, bool isEditable)
    {
        bool boolValue = (bool)value;
        if (isEditable)
        {
            if (ImGui.Checkbox(property.Name, ref boolValue))
            {
                property.Setter(boolValue);
            }
        }
        else
        {
            ImGui.Text($"{property.Name}: {boolValue}");
        }
    }

    private static void HandleStringProperty(DebugProperty property, object value, bool isEditable)
    {
        string stringValue = (string)value ?? string.Empty;
        if (isEditable)
        {
            if (ImGui.InputText(property.Name, ref stringValue, 100))
            {
                property.Setter(stringValue);
            }
        }
        else
        {
            ImGui.Text($"{property.Name}: {stringValue}");
        }
    }

    private static void HandleVector2Property(DebugProperty property, object value, bool isEditable)
    {
        XnaVector2 xnaVector2Value = (XnaVector2)value;
        ImGuiVector2 imguiVector2Value = new ImGuiVector2(xnaVector2Value.X, xnaVector2Value.Y);
        if (isEditable)
        {
            if (ImGui.DragFloat2(property.Name, ref imguiVector2Value))
            {
                property.Setter(new XnaVector2(imguiVector2Value.X, imguiVector2Value.Y));
            }
        }
        else
        {
            ImGui.Text($"{property.Name}: X:{xnaVector2Value.X} Y:{xnaVector2Value.Y}");
        }
    }

    private static void HandleVector3Property(DebugProperty property, object value, bool isEditable)
    {
        XnaVector3 xnaVector3Value = (XnaVector3)value;
        ImGuiVector3 imguiVector3Value = new ImGuiVector3(xnaVector3Value.X, xnaVector3Value.Y, xnaVector3Value.Z);
        if (isEditable)
        {
            if (ImGui.DragFloat3(property.Name, ref imguiVector3Value))
            {
                property.Setter(new XnaVector3(imguiVector3Value.X, imguiVector3Value.Y, imguiVector3Value.Z));
            }
        }
        else
        {
            ImGui.Text($"{property.Name}: X:{xnaVector3Value.X} Y:{xnaVector3Value.Y} Z:{xnaVector3Value.Z}");
        }
    }

    private static void HandleColorProperty(DebugProperty property, object value, bool isEditable)
    {
        XnaColor xnaColorValue = (XnaColor)value;
        ImGuiVector4 colorVector = new ImGuiVector4(xnaColorValue.R / 255f, xnaColorValue.G / 255f, xnaColorValue.B / 255f, xnaColorValue.A / 255f);
        if (isEditable)
        {
            if (ImGui.ColorEdit4(property.Name, ref colorVector))
            {
                XnaColor newColor = new XnaColor((int)(colorVector.X * 255), (int)(colorVector.Y * 255), (int)(colorVector.Z * 255), (int)(colorVector.W * 255));
                property.Setter(newColor);
            }
        }
        else
        {
            ImGui.Text($"{property.Name}: R:{xnaColorValue.R} G:{xnaColorValue.G} B:{xnaColorValue.B} A:{xnaColorValue.A}");
        }
    }
}