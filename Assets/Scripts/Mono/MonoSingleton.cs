using UnityEngine;
using Options;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T instance = null;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(T)) as T;
                if (instance == null)
                {
                    GameObject go = Resources.Load<GameObject>(Utils.GetFilePath(ResourcePath.Manager, typeof(T).ToString()));
                    if (go == null)
                    {
                        instance = new GameObject("@" + typeof(T).ToString(), typeof(T)).GetComponent<T>();
                    }
                    else
                    {
                        GameObject gameObject = Instantiate(go);
                        gameObject.name = "@" + typeof(T).ToString();
                        instance = gameObject.GetComponent<T>();
                    }

                    DontDestroyOnLoad(instance);
                }
            }

            return instance;
        }
    }
}
