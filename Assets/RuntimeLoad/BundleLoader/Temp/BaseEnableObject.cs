using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class BaseEnableObject :MonoBehaviour, IRunTimeEnable
{
    public event UnityAction OnDelete;

    protected virtual void OnDestroy()
    {
        OnDelete.Invoke();
    }
}
