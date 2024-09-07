
using System;

namespace FizzleMonogameTemplate.DebugGUI;
public class DebugProperty(string name, Type type, Func<object> getter, Action<object> setter)
{
    public string Name { get; set; } = name;
    public Type Type { get; set; } = type;
    public Func<object> Getter { get; set; } = getter;
    public Action<object> Setter { get; set; } = setter;

}