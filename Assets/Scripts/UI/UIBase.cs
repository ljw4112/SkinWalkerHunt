using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Options;

public abstract class UIBase : MonoBehaviour
{
    /// <summary>
    /// UI에 필요한 데이터 셋팅
    /// </summary>
    /// <param name="options"></param>
    public abstract void Initialize(Data data);

    /// <summary>
    /// Initialize된 데이터를 바탕으로 UI 세팅
    /// </summary>
    /// <param name="status"></param>
    public virtual void UpdateUI(UIStatus status)
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// UI가 닫힐 때 실행되어야 할 것들
    /// </summary>
    public virtual void OnClickClose()
    {
        gameObject.SetActive(false);
    }
}
