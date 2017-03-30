using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class RTObjTemp :MonoBehaviour,IRTButton,IRTMessage,IRTToggle {

    [SerializeField]
    protected UnityEvent m_OnOpen;
    protected Button m_Btn;
    [SerializeField]
    protected Toggle.ToggleEvent m_OpenClose;
    protected Toggle m_Tog;
    public event UnityAction OnDelete;

    public virtual Button Btn
    {
        set
        {
            m_Btn = value;
        }
    }

    public virtual Toggle toggle
    {
        set
        {
            m_Tog = value;
            m_Tog.onValueChanged.AddListener((x) => { gameObject.SetActive(x); });
        }
    }

    public virtual void HandleMessage(object message)
    {
        Debug.Log("打开面板" + this.name);
    }

    protected virtual void OnDestroy()
    {
        if(OnDelete != null) OnDelete();
    }
}
