using System.Collections.Generic;
using UnityEngine;
using Options;

public class UIManager : MonoSingleton<UIManager>
{
    private Stack<UIBase> uiStack = new Stack<UIBase>();                                        // UI-Depth ����
    private Dictionary<string, UIBase> cachedUITable = new Dictionary<string, UIBase>();        // UI Ǯ����

    /// <summary>
    /// UI ���
    /// </summary>
    /// <typeparam name="T"> UIBase�� ��ӹ��� UI </typeparam>
    /// <param name="options"> UI ���� �� �Ѱ��� �����͵� </param>
    public UIBase Show<T>(Dictionary<UIOptionsKey, object> options)
    {
        UIBase ui;

        // ĳ�̵Ǿ� ������ �װ� �״�� ������ ��
        if (cachedUITable.ContainsKey(typeof(T).ToString()))
        {
            ui = cachedUITable[typeof(T).ToString()];
            ui.transform.SetAsLastSibling();
        }
        else
        {
            // ������ �ϳ� ���θ���
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

        ui.Initialize(options);                         // UI ������ �ʱ�ȭ
        ui.UpdateUI(UIStatus.CompleteShow);             // UI �׷��� ��ü �ʱ�ȭ

        return ui;
    }

    /// <summary>
    /// �ֻ�� UI ��������
    /// </summary>
    /// <returns> �ֻ�� UI �⺻�� </returns>
    public UIBase GetTopUI()
    {
        if (uiStack.Count == 0) return null;
        return uiStack.Peek();
    }

    /// <summary>
    /// �ֻ�� UI ��������
    /// </summary>
    /// <returns> �ֻ�� UI </returns>
    public T GetTopUI<T>() where T : UIBase
    {
        if (uiStack.Count == 0) return null;
        return uiStack.Peek() as T;
    }

    /// <summary>
    /// ���� �����ִ� Ư�� UI ��������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetUI<T>() where T : UIBase
    {
        if (cachedUITable == null || cachedUITable.Count == 0) return null;
        
        // ���������� ����
        if (cachedUITable[typeof(T).ToString()].gameObject.activeInHierarchy)
            return cachedUITable[typeof(T).ToString()] as T;

        return null;
    }
}