
using System;

namespace FizzleMonogameTemplate.DebugGUI;
public class DebugProperty
{
    public string Name { get; set; }
    public Type Type { get; set; }
    public Func<object> Getter { get; set; }
    public Action<object> Setter { get; set; }
}