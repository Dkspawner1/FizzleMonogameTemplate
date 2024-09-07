using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace FizzleGame.Singletons.Classes;

public class ContentLoaderSingleton(ContentManager content)
{
    private static ContentLoaderSingleton instance;
    public ContentManager Content => content;
    private Dictionary<string, object> loadedContent = [];

    public static ContentLoaderSingleton Instance
    {
        get
        {
            if (instance is null)
                throw new InvalidOperationException("ContentLoaderSingleton must be initialized with a ContentManager before use.");
            return instance;
        }
    }
    public static void Initialize(ContentManager content) => instance = new(content);

    public T Load<T>(string assetName)
    {
        if (loadedContent.TryGetValue(assetName, out object asset))
            return (T)asset;
        T loadedAsset = Content.Load<T>(assetName);
        loadedContent[assetName] = loadedAsset;
        return loadedAsset;
    }
    public void Unload(string assetName)
    {
        if (loadedContent.Remove(assetName, out object asset))
            if (asset is IDisposable disposable)
                disposable.Dispose();
    }
    public void UnloadAll()
    {
        foreach (var asset in loadedContent.Values)
            if (asset is IDisposable disposable)
                disposable.Dispose();
        loadedContent.Clear();
        content.Unload();
    }
    public static void ForceGarbageCollection()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
    }
}
