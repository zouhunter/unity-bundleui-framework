using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public abstract class RunTimeUIPanel : UIBehaviour,IRunTimeButton,IRunTimeEvent,IRunTimeMessage,IRunTimeToggle {

    [SerializeField]
    private UnityEvent m_OnOpen;
    [SerializeField]
    private Toggle.ToggleEvent m_OpenClose;

    public event UnityAction OnDelete;

    public virtual Button Btn
    {
        set
        {
            throw new NotImplementedException();
        }
    }

    public virtual Toggle toggle
    {
        set
        {
            throw new NotImplementedException();
        }
    }

    public virtual void HandleMessage(object message)
    {
        throw new NotImplementedException();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if(OnDelete != null) OnDelete();
    }
}
