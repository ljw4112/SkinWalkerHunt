using System.Collections.Generic;
using UnityEngine;
using Options;
using UnityEditor;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private UIStack uiStack = new UIStack();                                   // UI-Depth 관리
    private Dictionary<string, UIBase> cachedUITable = new Dictionary<string, UIBase>();        // UI 풀링용
    private Dictionary<string, Data> cachedUIData = new Dictionary<string, Data>();             // UI 전달 데이터

    /// <summary>
    /// UI 출력
    /// </summary>
    /// <typeparam name="T"> UIBase에 상속받은 UI </typeparam>
    /// <param name="options"> UI 켜질 때 넘겨줄 데이터들 </param>
    public UIBase Show<T>(Data data)
    {
        UIBase ui;

        // 캐싱되어 있으면 그거 그대로 가져다 씀
        if (cachedUITable.ContainsKey(typeof(T).ToString()))
        {
            ui = cachedUITable[typeof(T).ToString()];
        }
        else
        {
            // 없으면 하나 새로만듬
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

        ui.Initialize(data);                         // UI 데이터 초기화
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

    /// <summary>
    /// 해당 UI 데이터가 이미 만들어져있는지 판단하고 있으면 만들어져있는 걸 사용함.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Data GetCachedUIData<T>() where T : UIBase
    {
        string name = $"Data_{typeof(T)}";
        if (cachedUIData.ContainsKey(name))
        {
            Debug.Log($"이미 만들어져있음 Data_{typeof(T)}");
            cachedUIData[name].Initialize();
            return cachedUIData[name];
        }

        Debug.Log($"새로 만들어야됨 Data_{typeof(T)}");
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