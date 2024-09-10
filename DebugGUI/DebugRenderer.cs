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
    private static readonly Dictionary<Type, Action<string, object, Action<object>, bool>> PropertyHandlers = new()
    {
        { typeof(float), HandleNumericProperty<float> },
        { typeof(int), HandleNumericProperty<int> },
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
                handler(property.Name, value, property.Setter, isEditable);
            }
            else
            {
                ImGui.Text($"{property.Name}: {value}");
            }
        }
    }

    private static void HandleNumericProperty<T>(string name, object value, Action<object> setter, bool isEditable) where T : struct
    {
        T typedValue = (T)value;
        if (isEditable)
        {
            if (typeof(T) == typeof(float))
            {
                float floatValue = Convert.ToSingle(typedValue);
                if (ImGui.DragFloat(name, ref floatValue, 0.01f))
                {
                    setter(floatValue);
                }
            }
            else if (typeof(T) == typeof(int))
            {
                int intValue = Convert.ToInt32(typedValue);
                if (ImGui.DragInt(name, ref intValue))
                {
                    setter(intValue);
                }
            }
        }
        else
        {
            ImGui.Text($"{name}: {typedValue}");
        }
    }

    private static void HandleBoolProperty(string name, object value, Action<object> setter, bool isEditable)
    {
        bool boolValue = (bool)value;
        if (isEditable)
        {
            if (ImGui.Checkbox(name, ref boolValue))
            {
                setter(boolValue);
            }
        }
        else
        {
            ImGui.Text($"{name}: {boolValue}");
        }
    }

    private static void HandleStringProperty(string name, object value, Action<object> setter, bool isEditable)
    {
        string stringValue = (string)value ?? string.Empty;
        if (isEditable)
        {
            if (ImGui.InputText(name, ref stringValue, 100))
            {
                setter(stringValue);
            }
        }
        else
        {
            ImGui.Text($"{name}: {stringValue}");
        }
    }

    private static void HandleVector2Property(string name, object value, Action<object> setter, bool isEditable)
    {
        XnaVector2 xnaVector2Value = (XnaVector2)value;
        ImGuiVector2 imguiVector2Value = new ImGuiVector2(xnaVector2Value.X, xnaVector2Value.Y);
        if (isEditable)
        {
            if (ImGui.DragFloat2(name, ref imguiVector2Value))
            {
                setter(new XnaVector2(imguiVector2Value.X, imguiVector2Value.Y));
            }
        }
        else
        {
            ImGui.Text($"{name}: X:{xnaVector2Value.X} Y:{xnaVector2Value.Y}");
        }
    }

    private static void HandleVector3Property(string name, object value, Action<object> setter, bool isEditable)
    {
        XnaVector3 xnaVector3Value = (XnaVector3)value;
        ImGuiVector3 imguiVector3Value = new ImGuiVector3(xnaVector3Value.X, xnaVector3Value.Y, xnaVector3Value.Z);
        if (isEditable)
        {
            if (ImGui.DragFloat3(name, ref imguiVector3Value))
            {
                setter(new XnaVector3(imguiVector3Value.X, imguiVector3Value.Y, imguiVector3Value.Z));
            }
        }
        else
        {
            ImGui.Text($"{name}: X:{xnaVector3Value.X} Y:{xnaVector3Value.Y} Z:{xnaVector3Value.Z}");
        }
    }

    private static void HandleColorProperty(string name, object value, Action<object> setter, bool isEditable)
    {
        XnaColor xnaColorValue = (XnaColor)value;
        ImGuiVector4 colorVector = new ImGuiVector4(xnaColorValue.R / 255f, xnaColorValue.G / 255f, xnaColorValue.B / 255f, xnaColorValue.A / 255f);
        if (isEditable)
        {
            if (ImGui.ColorEdit4(name, ref colorVector))
            {
                XnaColor newColor = new XnaColor((int)(colorVector.X * 255), (int)(colorVector.Y * 255), (int)(colorVector.Z * 255), (int)(colorVector.W * 255));
                setter(newColor);
            }
        }
        else
        {
            ImGui.Text($"{name}: R:{xnaColorValue.R} G:{xnaColorValue.G} B:{xnaColorValue.B} A:{xnaColorValue.A}");
        }
    }
}