using System;
using System.Collections.Generic;
using System.Reflection;
using FizzleMonogameTemplate.DebugGUI.Attributes;

namespace FizzleMonogameTemplate.DebugGUI
{
    public static class DebuggableHelper
    {
        public static List<DebugProperty> GetDebugProperties(object obj)
        {
            var properties = new List<DebugProperty>();
            var type = obj.GetType();

            var members = type.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var member in members)
            {
                var attribute = member.GetCustomAttribute<DebugVariableAttribute>();

                if (attribute != null)
                {
                    string name = member.Name;
                    Type memberType = null;
                    Func<object> getValue = null;
                    Action<object> setValue = null;

                    if (member is FieldInfo field)
                    {
                        memberType = field.FieldType;
                        getValue = () => field.GetValue(obj);
                        if (attribute.IsEditable)
                        {
                            setValue = newValue => field.SetValue(obj, newValue);
                        }
                    }
                    else if (member is PropertyInfo property)
                    {
                        if (property.CanRead)
                        {
                            memberType = property.PropertyType;
                            getValue = () => property.GetValue(obj);
                            if (attribute.IsEditable && property.CanWrite)
                            {
                                setValue = newValue => property.SetValue(obj, newValue);
                            }
                        }
                    }

                    if (memberType != null && getValue != null)
                    {
                        properties.Add(new DebugProperty(name, memberType, getValue, setValue));
                    }
                }
            }

            return properties;
        }
    }
}