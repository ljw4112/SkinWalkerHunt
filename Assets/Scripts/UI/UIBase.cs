using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Options;

public abstract class UIBase : MonoBehaviour
{
    /// <summary>
    /// UI�� �ʿ��� ������ ����
    /// </summary>
    /// <param name="options"></param>
    public abstract void Initialize(Data data);

    /// <summary>
    /// Initialize�� �����͸� �������� UI ����
    /// </summary>
    /// <param name="status"></param>
    public virtual void UpdateUI(UIStatus status)
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// UI�� ���� �� ����Ǿ�� �� �͵�
    /// </summary>
    public virtual void OnClickClose()
    {
        gameObject.SetActive(false);
    }
}
