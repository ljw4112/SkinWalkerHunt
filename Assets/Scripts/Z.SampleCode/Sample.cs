using Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample : MonoBehaviour
{
    private void Awake()
    {
        Dictionary<UIOptionsKey, object> options = new Dictionary<UIOptionsKey, object>()
        {
            { UIOptionsKey.Data, 15 }
        };

        UIManager.Instance.Show<UIMenu>(options);
    }
}
