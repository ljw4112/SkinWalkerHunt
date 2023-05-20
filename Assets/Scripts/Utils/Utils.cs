using Options;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public static class Utils
{
    public static Data GetDataClass<T>() where T : UIBase
    {
        Data data = UIManager.Instance.GetCachedUIData<T>();
        if (data != null)
            return data;
        else
        {
            Type type = Type.GetType($"Data_{typeof(T)}");
            Data createdData = (Data)Activator.CreateInstance(type);
            UIManager.Instance.AddUIData(type.ToString(), createdData);

            return createdData;
        }
    }

    public static string GetFilePath(string path, string fileName)
    {
        return $"{path}/{fileName}";
    }
}
