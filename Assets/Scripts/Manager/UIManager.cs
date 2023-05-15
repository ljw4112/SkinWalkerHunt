using System.Collections.Generic;
using UnityEngine;
using Options;

public class UIManager : MonoSingleton<UIManager>
{
    private Stack<UIBase> uiStack = new Stack<UIBase>();                                        // UI-Depth 관리
    private Dictionary<string, UIBase> cachedUITable = new Dictionary<string, UIBase>();        // UI 풀링용

    /// <summary>
    /// UI 출력
    /// </summary>
    /// <typeparam name="T"> UIBase에 상속받은 UI </typeparam>
    /// <param name="options"> UI 켜질 때 넘겨줄 데이터들 </param>
    public UIBase Show<T>(Dictionary<UIOptionsKey, object> options)
    {
        UIBase ui;

        // 캐싱되어 있으면 그거 그대로 가져다 씀
        if (cachedUITable.ContainsKey(typeof(T).ToString()))
        {
            ui = cachedUITable[typeof(T).ToString()];
            ui.transform.SetAsLastSibling();
        }
        else
        {
            // 없으면 하나 새로만듬
            GameObject go = Resources.Load<GameObject>(Utils.GetFilePath(ResourcePath.UIPrefab, typeof(T).ToString()));
            GameObject gameObject = Instantiate(go, transform);
            gameObject.name = go.name;

            if (go.TryGetComponent(out ui))
                uiStack.Push(ui);
        }

        if (ui == null)
        {
            Debug.LogError("!!! UI Prefab doesn't exists !!!");
            return null;
        }

        ui.Initialize(options);                         // UI 데이터 초기화
        ui.UpdateUI(UIStatus.CompleteShow);             // UI 그래픽 객체 초기화

        return ui;
    }

    /// <summary>
    /// 최상단 UI 가져오기
    /// </summary>
    /// <returns> 최상단 UI 기본형 </returns>
    public UIBase GetTopUI()
    {
        if (uiStack.Count == 0) return null;
        return uiStack.Peek();
    }

    /// <summary>
    /// 최상단 UI 가져오기
    /// </summary>
    /// <returns> 최상단 UI </returns>
    public T GetTopUI<T>() where T : UIBase
    {
        if (uiStack.Count == 0) return null;
        return uiStack.Peek() as T;
    }

    /// <summary>
    /// 현재 열려있는 특정 UI 가져오기
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetUI<T>() where T : UIBase
    {
        if (cachedUITable == null || cachedUITable.Count == 0) return null;
        
        // 켜져있으면 리턴
        if (cachedUITable[typeof(T).ToString()].gameObject.activeInHierarchy)
            return cachedUITable[typeof(T).ToString()] as T;

        return null;
    }
}