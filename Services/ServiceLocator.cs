
using System;
using System.Collections.Generic;

namespace FizzleMonogameTemplate.Services;
public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> services = [];
    public static void RegisterService<T>(T service) where T : class
    {
        if (service is null)
            throw new ArgumentNullException(nameof(service));
        Type type = typeof(T);
        if (services.ContainsKey(type))
            throw new InvalidOperationException($"Service of type {type.Name} is already registered.");
        services[type] = service;
    }
    public static T GetService<T>() where T : class
    {
        Type type = typeof(T);
        if (!services.TryGetValue(type, out object service))
            throw new InvalidOperationException($"Service of type {type.Name} is not registered.");

        return (T)service;
    }
    public static bool TryGetService<T>(out T service) where T : class
    {
        Type type = typeof(T);
        if (services.TryGetValue(type, out object serviceObj))
        {
            service = (T)serviceObj;
            return true;
        }
        service = null;
        return false;
    }

    public static void UnregisterService<T>() where T : class
    {
        Type type = typeof(T);
        if (!services.Remove(type))
            throw new InvalidOperationException($"Service of type {type.Name} is not registered.");
    }
    public static void Clear() => services.Clear();
    public static bool IsRegistered<T>() where T : class => services.ContainsKey(typeof(T));
}