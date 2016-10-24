using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
[Serializable]
public class MediatorTriggerPanel : TriggerPanel
{
    [SerializeField]
    private string observerName;
    void OnEnable()
    {
        Facade.Instance.RegisterEvent<object>(observerName, ReceiveEvent);
    }

    void ReceiveEvent(object notify)
    {
        Facade.Instance.RemoveEvent<object>(observerName, ReceiveEvent);

        Controller.GetPanelFromBundle<IRunTimeMediator>(assetName, (x) =>
            {
                x.HandleNotification(notify);
            });
    }
    protected override void OnDestroy()
    {
        Facade.Instance.RemoveEvent<object>(observerName, ReceiveEvent);
        base.OnDestroy();
    }
}
