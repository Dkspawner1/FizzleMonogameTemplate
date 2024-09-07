using System.Collections.Generic;
using System.Reflection;
using FizzleMonogameTemplate.DebugGUI;

public abstract class DebuggableObject : IDebuggable
{
    private List<DebugProperty> _cachedProperties;

    public List<DebugProperty> GetDebugProperties()
    {
        if (_cachedProperties != null)
            return _cachedProperties;

        _cachedProperties = new List<DebugProperty>();
        var type = GetType();

        AddDebugProperties(type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public));
        AddDebugProperties(type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public));

        return _cachedProperties;
    }

    private void AddDebugProperties(IEnumerable<MemberInfo> members)
    {
        foreach (var member in members)
        {
            if (member.GetCustomAttribute<DebugVariableAttribute>() == null)
                continue;

            var property = new DebugProperty
            {
                Name = member.Name,
                Type = member is FieldInfo field ? field.FieldType : ((PropertyInfo)member).PropertyType,
                Getter = () => member is FieldInfo f ? f.GetValue(this) : ((PropertyInfo)member).GetValue(this),
                Setter = value =>
                {
                    if (member is FieldInfo f)
                        f.SetValue(this, value);
                    else
                        ((PropertyInfo)member).SetValue(this, value);
                }
            };

            _cachedProperties.Add(property);
        }
    }
}