using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BaseToggleObject : MonoBehaviour ,IRunTimeToggle{
    public Toggle toggle
    {
        set
        {
            m_tog = value;
            m_tog.onValueChanged.AddListener(togEvent.Invoke);
        }
    }
    private Toggle m_tog;
    public Toggle.ToggleEvent togEvent;
    public event UnityAction OnDelete;

    protected virtual void OnDestroy()
    {
        if(m_tog) m_tog.onValueChanged.RemoveListener(togEvent.Invoke);
        if(OnDelete != null) OnDelete.Invoke();
    }
}
