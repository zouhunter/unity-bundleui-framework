using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BaseButtonObject : MonoBehaviour,IRunTimeButton {
    public Button Btn
    {
        set
        {
            m_btn = value;
            m_btn.onClick.AddListener(buttonEvent.Invoke);
        }
    }
    private Button m_btn;
    public Button.ButtonClickedEvent buttonEvent;

    public event UnityAction OnDelete;

    protected virtual void OnDestroy()
    {
        if(m_btn) m_btn.onClick.RemoveListener(buttonEvent.Invoke);
        if (OnDelete != null) OnDelete.Invoke();
    }
}
