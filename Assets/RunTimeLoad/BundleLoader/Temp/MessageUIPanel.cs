using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;

public class MessageUIPanel : UIBehaviour , IRunTimeMessage,IRunTimeEvent{

    public event UnityAction OnDelete;

    public virtual void HandleMessage(object message)
    {
        throw new NotImplementedException();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        OnDelete();
    }

}
