
using System;

namespace FizzleMonogameTemplate.DebugGUI;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true)]
public class DebugVariableAttribute : Attribute { }