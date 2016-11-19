using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public partial class RunTimeObjectBehaiver
{
#if UNITY_EDITOR
    public GameObject prefab;
    [InspectorButton("QuickLoadObject")]
    public int QuickLoad;
    void QuickLoadObject()
    {
        string name = prefab.name;
        string assetPath = UnityEditor.AssetDatabase.GetAssetPath(prefab);// UnityEditor.AssetDatabase.GetTextMetaFilePathFromAssetPath(.asset(prefab)[0];
        UnityEditor.AssetImporter importer = UnityEditor.AssetImporter.GetAtPath(assetPath);
        string assetBundle = importer.assetBundleName;

        if (string.IsNullOrEmpty(assetBundle))
        {
            UnityEditor.EditorUtility.DisplayDialog("提示", "预制体没有assetBundle标记", "确认");
            return;
        }
        if (bundles.Find((x) => x.assetName == name) != null)
        {
            UnityEditor.EditorUtility.DisplayDialog("提示", "信息已经加载到集合中", "确认");
            return;
        }

        RunTimeBundleInfo info = new RunTimeBundleInfo();
        info.assetName = name;
        info.bundleName = assetBundle;
        info.isWorld = !prefab.GetComponent<RectTransform>();
        info.parent = transform;
        bundles.Add(info);
        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif
}
public partial class RunTimeObjectBehaiver : MonoBehaviour
{
    [Space(20)]
    public string assetBundleFile;

    [Header("动态面版")]
    public List<RunTimeBundleInfo> bundles = new List<RunTimeBundleInfo>();
    private event UnityAction onSceneSwitch;
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
        onSceneSwitch += () => { Facade.Instance.RemoveEvent(trigger.message, action); };
    }

    private void RegisterMessageEvents(RunTimeBundleInfo trigger)
    {
        UnityAction<object> action = (x) =>
        {
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
        onSceneSwitch += () => { Facade.Instance.RemoveEvent<object>(trigger.message, action); };
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
        onSceneSwitch += () =>
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
        onSceneSwitch += () => { trigger.button.onClick.RemoveAllListeners(); };
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
        if (onSceneSwitch != null) onSceneSwitch();
    }

    //private void RegisterButtonEvents()
    //{
    //    for (int i = 0; i < btnTriggers.Count; i++)
    //    {
    //        ButtonTrigger btnTrigger = btnTriggers[i];
    //        UnityAction CreateByButton = () =>
    //        {
    //            Controller.GetGameObjectFromBundle(btnTrigger);
    //        };
    //        btnTrigger.button.onClick.AddListener(CreateByButton);
    //        onSceneSwitch += () => { btnTrigger.button.onClick.RemoveAllListeners(); };
    //        btnTrigger.OnCreate = (x) =>
    //        {
    //            x.GetComponent<IRunTimeButton>().Btn = btnTrigger.button;
    //            btnTrigger.button.onClick.RemoveListener(CreateByButton);
    //        };
    //    }
    //}

    //private void RegisterMessageEvents()
    //{
    //    for (int i = 0; i < msgTriggers.Count; i++)
    //    {
    //        MessageTrigger trigger = msgTriggers[i];
    //        UnityAction<object> action = (x) =>
    //        {
    //            trigger.data = x;
    //            Controller.GetGameObjectFromBundle(trigger);
    //        };
    //        trigger.OnCreate = (x) =>
    //        {
    //            IRunTimeMessage irm = x.GetComponent<IRunTimeMessage>();
    //            irm.HandleMessage(trigger.data);
    //            Facade.Instance.RemoveEvent<object>(trigger.messageKey, action);
    //            irm.OnDelete += () =>
    //            {
    //                Facade.Instance.RegisterEvent<object>(trigger.messageKey, action);
    //            };
    //        };

    //        Facade.Instance.RegisterEvent<object>(trigger.messageKey, action);
    //        onSceneSwitch += () => { Facade.Instance.RemoveEvent<object>(trigger.messageKey, action); };
    //    }
    //}

    //private void RegisterEventEvents()
    //{
    //    for (int i = 0; i < evtTriggers.Count; i++)
    //    {
    //        ActionTrigger trigger = evtTriggers[i];
    //        UnityAction action = () =>
    //        {
    //            Controller.GetGameObjectFromBundle(trigger);
    //        };
    //        trigger.OnCreate = (x) =>
    //        {
    //            IRunTimeEvent irm = x.GetComponent<IRunTimeEvent>();
    //            Facade.Instance.RemoveEvent(trigger.messageKey, action);
    //            irm.OnDelete += () =>
    //            {
    //                Facade.Instance.RegisterEvent(trigger.messageKey, action);
    //            };
    //        };

    //        Facade.Instance.RegisterEvent(trigger.messageKey, action);
    //        onSceneSwitch += () => { Facade.Instance.RemoveEvent(trigger.messageKey, action); };
    //    }
    //}

}
