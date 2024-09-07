using System.Collections.Generic;
using System.Reflection;

namespace FizzleMonogameTemplate.DebugGUI
{
    public static class DebuggableHelper
    {
        public static List<DebugProperty> GetDebugProperties(object obj)
        {
            var properties = new List<DebugProperty>();
            var members = obj.GetType().GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var member in members)
            {
                if (member.GetCustomAttribute<DebugVariableAttribute>() != null)
                {
                    if (member is PropertyInfo property)
                    {
                        properties.Add(new DebugProperty(
                            property.Name,
                            property.PropertyType,
                            () => property.GetValue(obj),
                            value => property.SetValue(obj, value)
                        ));
                    }
                    else if (member is FieldInfo field)
                    {
                        properties.Add(new DebugProperty(
                            field.Name,
                            field.FieldType,
                            () => field.GetValue(obj),
                            value => field.SetValue(obj, value)
                        ));
                    }
                }
            }

            return properties;
        }
    }
}