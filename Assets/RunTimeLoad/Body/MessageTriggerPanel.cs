using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;


public class MessageTriggerPanel : TriggerPanel
{
    public string messageKey;
    private bool isMessageRecevied;
    void OnEnable()
    {
        isMessageRecevied = false;
    }

    protected override void Start()
    {
        base.Start();
        Facade.Instance.RegisterEvent<object>(messageKey, OnMessageReceived);
    }

    void OnMessageReceived<T>(T body)
    {
        if (isMessageRecevied == false)
        {
            isMessageRecevied = true;

            Controller.GetPanelFromBundle<IRunTimeMessage>(assetName, (x) =>
            {
                if (x != null)
                {
                    x.HandleMessage(body);
                    if (destroyType == DestroyType.Hide)
                    {
                        x.OnDelete += OnCreatedClose;
                    }
                }
            });
        }
    }
    void OnCreatedClose()
    {
        if (destroyType == DestroyType.Hide)
        {
            gameObject.SetActive(true);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (Facade.Instance!=null && !isMessageRecevied)
            Facade.Instance.RemoveEvent<object>(messageKey, OnMessageReceived);
    }
}
