using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;


public class EventTriggerPanel : TriggerPanel
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
        Facade.Instance.RegisterEvent(messageKey, OnMessageReceived);
    }

    void OnMessageReceived()
    {
        if (isMessageRecevied == false)
        {
            isMessageRecevied = true;

            Controller.GetPanelFromBundle<IRunTimeEvent>(assetName, (x) =>
            {
                if (x != null)
                {
                    x.OnCreate();
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
            enabled = true;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (Facade.Instance != null && !isMessageRecevied)
            Facade.Instance.RemoveEvent(messageKey, OnMessageReceived);
    }
}
