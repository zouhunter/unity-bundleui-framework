using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public partial class RTObjGroup : MonoBehaviour
{
    [Space(20)]
    public string assetBundleFile = "AssetBundle";

    [Header("动态面版")]
    public List<RTBundleInfo> bundles = new List<RTBundleInfo>();
    private event UnityAction onDestroy;
    private event UnityAction onEnable;
    private event UnityAction onDisable;
    public IRunTimeLoadCtrl Controller;
    void Awake()
    {
        Controller = new RTLoadCtrl(assetBundleFile);
        RegisterBundleEvents();
    }

    private void RegisterBundleEvents()
    {
        for (int i = 0; i < bundles.Count; i++)
        {
            RTBundleInfo trigger = bundles[i];
            switch (trigger.type)
            {
                case RTBundleInfo.Type.Button:
                    RegisterButtonEvents(trigger);
                    break;
                case RTBundleInfo.Type.Toggle:
                    RegisterToggleEvents(trigger);
                    break;
                case RTBundleInfo.Type.Name:
                    RegisterMessageEvents(trigger);
                    break;
                case RTBundleInfo.Type.Enable:
                    RegisterEnableEvents(trigger);
                    break;
                default:
                    break;
            }
        }
    }
    private void RegisterMessageEvents(RTBundleInfo trigger)
    {
        UnityAction<object> action = (x) =>
        {
            //防止重复加载
            trigger.Data = x;
            Controller.GetGameObjectFromBundle(trigger);
        };

        trigger.OnCreate = (x) =>
        {
            IRTMessage irm = x.GetComponent<IRTMessage>();
            if (irm != null)
            {
                irm.HandleMessage(trigger.Data);
                RTObjUtility.Remove(trigger.assetName, action);
                irm.OnDelete += () =>
                {
                    RTObjUtility.Record(trigger.assetName, action);
                };
            }
        };

        RTObjUtility.Record(trigger.assetName, action);
        onDestroy += () =>
        {
            RTObjUtility.Remove(trigger.assetName, action);
        };
    }

    private void RegisterToggleEvents(RTBundleInfo trigger)
    {
        UnityAction<bool> CreateByToggle = (x) =>
        {
            if (x)
            {
                trigger.toggle.interactable = false;
                Controller.GetGameObjectFromBundle(trigger);
            }
            else
            {
                Destroy((GameObject)trigger.Data);
            }
        };
        trigger.toggle.onValueChanged.AddListener(CreateByToggle);

        onDestroy += () =>
        {
            trigger.toggle.onValueChanged.RemoveAllListeners();
        };

        trigger.OnCreate = (x) =>
        {
            trigger.toggle.interactable = true;

            trigger.Data = x;
            IRTToggle it = x.GetComponent<IRTToggle>();
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

    private void RegisterButtonEvents(RTBundleInfo trigger)
    {
        UnityAction CreateByButton = () =>
        {
            Controller.GetGameObjectFromBundle(trigger);
        };
        trigger.button.onClick.AddListener(CreateByButton);
        onDestroy += () => { trigger.button.onClick.RemoveAllListeners(); };
        trigger.OnCreate = (x) =>
        {
            IRTButton ib = x.GetComponent<IRTButton>();
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
    private void RegisterEnableEvents(RTBundleInfo trigger)
    {
        UnityAction onEnableAction = () =>
        {
            Controller.GetGameObjectFromBundle(trigger);
        };

        trigger.OnCreate = (x) =>
        {
            trigger.Data = x;
            IRTEnable irm = x.GetComponent<IRTEnable>();
            if (irm != null)
            {
                onEnable -= onEnableAction;

                irm.OnDelete += () =>
                {
                    onEnable += onEnableAction;
                };
            }
            else
            {
                onDisable += () =>
                {
                    if (trigger.Data != null && trigger.Data is GameObject)
                    {
                        Destroy((GameObject)trigger.Data);
                    }
                };
            }
        };

        onEnable += onEnableAction;
    }

    private void OnEnable()
    {
        if (onEnable != null)
        {
            onEnable.Invoke();
        }
    }
    private void OnDisable()
    {
        if (onDisable != null)
        {
            onDisable.Invoke();
        }
    }

    void OnDestroy()
    {
        if (onDestroy != null)
        {
            onDestroy.Invoke();
        }
    }
}
