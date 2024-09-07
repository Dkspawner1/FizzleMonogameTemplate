using System;
using System.Collections.Generic;
using System.Reflection;
using FizzleMonogameTemplate.DebugGUI;

public abstract class DebuggableObject : IDebuggable
{
    private List<DebugProperty> cachedProperties;

    public List<DebugProperty> GetDebugProperties()
    {
        return DebuggableHelper.GetDebugProperties(this);
    }
    private void AddDebugProperties(IEnumerable<MemberInfo> members)
    {
        foreach (var member in members)
        {
            if (member.GetCustomAttribute<DebugVariableAttribute>() == null)
                continue;

            Type memberType = member is FieldInfo field ? field.FieldType : ((PropertyInfo)member).PropertyType;

            Func<object> getter = () => member is FieldInfo f
                ? f.GetValue(this)
                : ((PropertyInfo)member).GetValue(this);

            Action<object> setter = value =>
            {
                if (member is FieldInfo f)
                    f.SetValue(this, value);
                else
                    ((PropertyInfo)member).SetValue(this, value);
            };

            var property = new DebugProperty(member.Name, memberType, getter, setter);

            cachedProperties.Add(property);
        }
    }
}