using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;


public class RunTimeObjectBehaiver : MonoBehaviour
{
#if UNITY_EDITOR
    public string InEditorassetName;
    public string InEditorassetbundleName;
    [InspectorButton("LoadItem")]
    public bool 加载 = true;
    public void LoadItem()
    {
        string path = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(InEditorassetbundleName, InEditorassetName)[0];
        GameObject perfab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
        GameObject item = UnityEditor.PrefabUtility.InstantiatePrefab(perfab) as GameObject;
        if (perfab.GetComponent<RectTransform>() == null){
            item.transform.SetParent(null, true);
        }
        else
        {
            item.transform.SetParent(transform, false);
        }
        item.SetActive(true);
        UnityEditor.Selection.activeGameObject = item;
    }
#endif


    public IRunTimeLoadCtrl Controller
    {
        get { return RunTimeLoadController.Instance; }
    }

    [Array,Header("按扭触发")]
    public List<ButtonTrigger> btnTriggers;
    [Array, Header("信息触发")]
    public List<MessageTrigger> msgTriggers;
    [Array, Header("事件触发"),Tooltip("不带参数")]
    public List<EventTrigger> evtTriggers;
    void Start()
    {
        RegisterButtonEvents();
        RegisterMessageEvents();
        RegisterEventEvents();
    }

    private void RegisterButtonEvents()
    {
        ButtonTrigger btnTrigger;
        for (int i = 0; i < btnTriggers.Count; i++)
        {
            btnTrigger = btnTriggers[i];
            UnityAction CreateByButton = () =>
            {
                Controller.GetGameObjectFromBundle(btnTrigger);
            };

            btnTrigger.button.onClick.AddListener(CreateByButton);
            btnTrigger.OnCreate = (x) =>
            {
                x.GetComponent<IRunTimeButton>().Btn = btnTrigger.button;
                btnTrigger.button.onClick.RemoveListener(CreateByButton);
            };
        }
    }

    private void RegisterMessageEvents()
    {
        MessageTrigger trigger;
        for (int i = 0; i < msgTriggers.Count; i++)
        {
            trigger = msgTriggers[i];
            UnityAction<object> action = (x) =>
            {
                trigger.data = x;
                Controller.GetGameObjectFromBundle(trigger);
            };
            trigger.OnCreate = (x) =>
            {
                IRunTimeMessage irm = x.GetComponent<IRunTimeMessage>();
                irm.HandleMessage(trigger.data);
                Facade.Instance.RemoveEvent<object>(trigger.messageKey, action);
                irm.OnDelete += () =>
                {
                    Facade.Instance.RegisterEvent<object>(trigger.messageKey, action);
                };
            };

            Facade.Instance.RegisterEvent<object>(trigger.messageKey, action);
        }
    }

    private void RegisterEventEvents()
    {
        EventTrigger trigger;
        for (int i = 0; i < evtTriggers.Count; i++)
        {
            trigger = evtTriggers[i];
            UnityAction action = () =>
            {
                Controller.GetGameObjectFromBundle(trigger);
            };
            trigger.OnCreate = (x) =>
            {
                IRunTimeEvent irm = x.GetComponent<IRunTimeEvent>();
                Facade.Instance.RemoveEvent(trigger.messageKey, action);
                irm.OnDelete += () =>
                {
                    Facade.Instance.RegisterEvent(trigger.messageKey, action);
                };
            };

            Facade.Instance.RegisterEvent(trigger.messageKey, action);
        }
    }

}
