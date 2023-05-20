using System.Collections.Generic;
using UnityEngine;
using Options;
using UnityEditor;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private UIStack uiStack = new UIStack();                                   // UI-Depth ����
    private Dictionary<string, UIBase> cachedUITable = new Dictionary<string, UIBase>();        // UI Ǯ����
    private Dictionary<string, Data> cachedUIData = new Dictionary<string, Data>();             // UI ���� ������

    /// <summary>
    /// UI ���
    /// </summary>
    /// <typeparam name="T"> UIBase�� ��ӹ��� UI </typeparam>
    /// <param name="options"> UI ���� �� �Ѱ��� �����͵� </param>
    public UIBase Show<T>(Data data)
    {
        UIBase ui;

        // ĳ�̵Ǿ� ������ �װ� �״�� ������ ��
        if (cachedUITable.ContainsKey(typeof(T).ToString()))
        {
            ui = cachedUITable[typeof(T).ToString()];
        }
        else
        {
            // ������ �ϳ� ���θ���
            GameObject go = Resources.Load<GameObject>(Utils.GetFilePath(ResourcePath.UIPrefab, typeof(T).ToString()));
            GameObject gameObject = Instantiate(go, transform);
            gameObject.name = go.name;

            if (go.TryGetComponent(out ui))
            {
                uiStack.Push(ui);
                cachedUITable.Add(typeof(T).ToString(), ui);
            }
        }

        if (ui == null)
        {
            Debug.LogError("!!! UI Prefab doesn't exists !!!");
            return null;
        }

        ui.Initialize(data);                         // UI ������ �ʱ�ȭ
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

    /// <summary>
    /// �ش� UI �����Ͱ� �̹� ��������ִ��� �Ǵ��ϰ� ������ ��������ִ� �� �����.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Data GetCachedUIData<T>() where T : UIBase
    {
        string name = $"Data_{typeof(T)}";
        if (cachedUIData.ContainsKey(name))
        {
            Debug.Log($"�̹� ����������� Data_{typeof(T)}");
            cachedUIData[name].Initialize();
            return cachedUIData[name];
        }

        Debug.Log($"���� �����ߵ� Data_{typeof(T)}");
        return null;
    }

    public void AddUIData(string name, Data data)
    {
        if (string.IsNullOrEmpty(name)) return;
        if (data == null) return;

        if (!cachedUIData.TryAdd(name, data))
        {
            Debug.LogError("Error where add UIData");
        }
    }

    #region UI Data Structure
    public class UIStack
    {
        private List<UIBase> uiStack = new List<UIBase>();
        public int Count => uiStack.Count;
        public string this[int i]
        {
            get { return uiStack[i].name; }
        }

        public UIStack()
        {

        }

        public void Push(UIBase ui, bool check = true)
        {
            if (check)
            {
                int index = Contains(ui);
                if (index < 0)
                    uiStack.Add(ui);
                else
                {
                    uiStack.RemoveAt(index);
                    Push(ui, false);
                }
            }
            else
                uiStack.Add(ui);
        }

        public void Pop()
        {
            uiStack.RemoveAt(uiStack.Count - 1);
        }

        public UIBase Peek()
        {
            return uiStack[uiStack.Count - 1];
        }

        private int Contains(UIBase ui)
        {
            for (int i = 0; i < uiStack.Count; i++)
            {
                UIBase tmpUI = uiStack[i];
                if (tmpUI.name == ui.name)
                    return i;
            }

            return -1;
        }
    }
    #endregion

    #region Editor
    // This class is inside the TestClass so it could access its private fields
    // this custom editor will show up on any object with TestScript attached to it
    // you don't need (and can't) attach this class to a gameobject
    [CustomEditor(typeof(UIManager))]
    public class StackPreview : Editor
    {
        public override void OnInspectorGUI()
        {

            // get the target script as TestScript and get the stack from it
            var ts = (UIManager)target;
            var stack = ts.uiStack;

            // add a label for each item, you can add more properties
            // you can even access components inside each item and display them
            // for example if every item had a sprite we could easily show it 
            for (int i = 0; i < stack.Count; i++)
            {
                GUILayout.Label(stack[i]);
            }
        }
    }
    #endregion
}