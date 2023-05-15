using Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenu : UIBase
{
    public override void Initialize(Dictionary<UIOptionsKey, object> options)
    {
        Debug.Log(Utils.GetData<int>(UIOptionsKey.Data, options));
    }
}
