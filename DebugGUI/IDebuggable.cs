using System.Collections.Generic;
namespace FizzleMonogameTemplate.DebugGUI;
public interface IDebuggable { List<DebugProperty> GetDebugProperties() => DebuggableHelper.GetDebugProperties(this); }
