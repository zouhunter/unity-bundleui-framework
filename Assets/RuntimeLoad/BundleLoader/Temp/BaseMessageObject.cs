using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;

public class BaseMessageObject : MonoBehaviour , IRunTimeMessage,IRunTimeEvent{

    public event UnityAction OnDelete;

    public virtual void HandleMessage(object message)
    {
        //throw new NotImplementedException();
    }

    protected virtual void OnDestroy()
    {
        if(OnDelete != null) OnDelete();
    }

}
