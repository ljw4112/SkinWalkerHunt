using Options;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Utils
{
    public static T GetData<T>(UIOptionsKey key, Dictionary<UIOptionsKey, object> options)
    {
        T result = default(T);

        if (options == null) return result;
        if (!options.ContainsKey(key)) return result;

        return (T)options[key];
    }

    public static string GetFilePath(string path, string fileName)
    {
        return $"{path}/{fileName}";
    }
}
