using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public partial class RunTimeObjectBehaiver : MonoBehaviour
{
    [Space(20)]
    public string assetBundleFile = "AssetBundle";

    [Header("动态面版")]
    public List<RunTimeBundleInfo> bundles = new List<RunTimeBundleInfo>();
    private event UnityAction onDestroy;
    public IRunTimeLoadCtrl Controller;
    void Start()
    {
        Controller = new RunTimeLoadController(assetBundleFile);
        RegisterBundleEvents();
    }

    private void RegisterBundleEvents()
    {
        for (int i = 0; i < bundles.Count; i++)
        {
            RunTimeBundleInfo trigger = bundles[i];
            switch (trigger.type)
            {
                case RunTimeBundleInfo.Type.Button:
                    RegisterButtonEvents(trigger);
                    break;
                case RunTimeBundleInfo.Type.Toggle:
                    RegisterToggleEvents(trigger);
                    break;
                case RunTimeBundleInfo.Type.Message:
                    RegisterMessageEvents(trigger);
                    break;
                case RunTimeBundleInfo.Type.Action:
                    RegisterActionEvents(trigger);
                    break;
                default:
                    break;
            }
        }
    }

    private void RegisterActionEvents(RunTimeBundleInfo trigger)
    {
        UnityAction action = () =>
        {
            Controller.GetGameObjectFromBundle(trigger);
        };

        trigger.OnCreate = (x) =>
        {
            IRunTimeEvent irm = x.GetComponent<IRunTimeEvent>();
            if (irm != null)
            {
                Facade.Instance.RemoveEvent(trigger.message, action);
                irm.OnDelete += () =>
                {
                    Facade.Instance.RegisterEvent(trigger.message, action);
                };
            }
        };

        Facade.Instance.RegisterEvent(trigger.message, action);
        onDestroy += () => { Facade.Instance.RemoveEvent(trigger.message, action); };
    }

    private void RegisterMessageEvents(RunTimeBundleInfo trigger)
    {
        UnityAction<object> action = (x) =>
        {
            //防止重复加载
            trigger.Data = x;
            Controller.GetGameObjectFromBundle(trigger);
        };

        trigger.OnCreate = (x) =>
        {
            IRunTimeMessage irm = x.GetComponent<IRunTimeMessage>();
            if (irm != null)
            {
                irm.HandleMessage(trigger.Data);
                Facade.Instance.RemoveEvent<object>(trigger.message, action);
                irm.OnDelete += () =>
                {
                    Facade.Instance.RegisterEvent<object>(trigger.message, action);
                };
            }
        };

        Facade.Instance.RegisterEvent<object>(trigger.message, action);
        onDestroy += () =>
        {
            Facade.Instance.RemoveEvent<object>(trigger.message, action);
        };
    }

    private void RegisterToggleEvents(RunTimeBundleInfo trigger)
    {
        UnityAction<bool> CreateByToggle = (x) =>
        {
            if (x)
            {
                Controller.GetGameObjectFromBundle(trigger);
            }
            else
            {
                if (trigger.Data != null && trigger.Data is GameObject)
                {
                    Destroy((GameObject)trigger.Data);
                }
            }
        };
        trigger.toggle.onValueChanged.AddListener(CreateByToggle);
        onDestroy += () =>
        {
            trigger.toggle.onValueChanged.RemoveAllListeners();
        };

        trigger.OnCreate = (x) =>
        {
            trigger.Data = x;
            IRunTimeToggle it = x.GetComponent<IRunTimeToggle>();
            if (it != null)
            {
                it.toggle = trigger.toggle;

                trigger.toggle.onValueChanged.RemoveListener(CreateByToggle);

                it.OnDelete += () =>
                {
                    trigger.toggle.onValueChanged.AddListener(CreateByToggle);
                };
            }
        };
    }

    private void RegisterButtonEvents(RunTimeBundleInfo trigger)
    {
        UnityAction CreateByButton = () =>
        {
            Controller.GetGameObjectFromBundle(trigger);
        };
        trigger.button.onClick.AddListener(CreateByButton);
        onDestroy += () => { trigger.button.onClick.RemoveAllListeners(); };
        trigger.OnCreate = (x) =>
        {
            IRunTimeButton ib = x.GetComponent<IRunTimeButton>();
            if (ib != null)
            {
                ib.Btn = trigger.button;
                trigger.button.onClick.RemoveListener(CreateByButton);

                ib.OnDelete += () =>
                {
                    trigger.button.onClick.AddListener(CreateByButton);
                };
            }
        };
    }

    void OnDestroy()
    {
        if (onDestroy != null) onDestroy();
    }
}
