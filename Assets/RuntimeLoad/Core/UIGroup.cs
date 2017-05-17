using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using BundleUISystem.Internal;

namespace BundleUISystem
{
    /// <summary>
    /// 静态
    /// </summary>
    public partial class UIGroup
    {
        public static System.Action<string> MessageNotHandled;
        public static Dictionary<string, UnityAction<object>> m_needHandle = new Dictionary<string, UnityAction<object>>();
        public static void NoMessageHandle(string rMessage)
        {
            if (MessageNotHandled == null)
            {
                Debug.Log("action not registed!!!" + rMessage);
            }
            else
            {
                MessageNotHandled(rMessage);
            }
        }

        #region 注册注销事件

        public static void Record(string key, UnityAction<object> handle)
        {
            if (!m_needHandle.ContainsKey(key))
            {
                m_needHandle.Add(key, handle);
            }
            else
            {
                m_needHandle[key] += handle;
            }
        }
        public static bool Remove(string key, UnityAction<object> handle)
        {
            if (m_needHandle.ContainsKey(key))
            {
                m_needHandle[key] -= handle;
                if (m_needHandle[key] == null)
                {
                    m_needHandle.Remove(key);
                    return false;
                }
            }
            return true;
        }
        public static void RemoveEvents(string key)
        {
            if (m_needHandle.ContainsKey(key))
            {
                m_needHandle.Remove(key);
            }
        }
        public static bool HaveEvent(string key)
        {
            return m_needHandle.ContainsKey(key);
        }
        #endregion

        #region 触发事件
        public static void Open(string assetName, object data = null)
        {
            InvokeEvent(assetName, data);
        }
        public static void Open<T>(object data = null) where T : UIPanelTemp
        {
            string assetName = typeof(T).ToString();
            InvokeEvent(assetName, data);
        }
        public static void Close(string assetName)
        {
            foreach (var item in controllers)
            {
                if(item != null){
                    item.CansaleLoadObject(assetName);
                }
            }
            InvokeEvent(addClose + assetName);
        }
        public static void Close<T>() where T : UIPanelTemp
        {
            string assetName = typeof(T).ToString();
            Close(assetName);
        }

        private static void InvokeEvent(string key)
        {
            bool lReportMissingRecipient = true;
            if (m_needHandle.ContainsKey(key))
            {
                m_needHandle[key].DynamicInvoke();
                lReportMissingRecipient = false;
            }

            if (lReportMissingRecipient)
            {
                NoMessageHandle(key);
            }
        }
        private static void InvokeEvent(string key,object body)
        {
            bool lReportMissingRecipient = true;
            if (m_needHandle.ContainsKey(key))
            {
                m_needHandle[key].DynamicInvoke(body);
                lReportMissingRecipient = false;
            }

            if (lReportMissingRecipient)
            {
                NoMessageHandle(key);
            }
        }
        #endregion
    }
    /// <summary>
    /// 动态
    /// </summary>
    public partial class UIGroup : MonoBehaviour
    {
        [Space(20), SerializeField]
        private string assetBundleFile = "AssetBundle";

        [Header("动态面版")]
        public List<UIBundleInfo> bundles = new List<UIBundleInfo>();


        private event UnityAction onDestroy;
        private event UnityAction onEnable;
        private event UnityAction onDisable;
        private IUILoadCtrl Controller;
        private const string addClose = "close";
        private static List<IUILoadCtrl> controllers = new List<IUILoadCtrl>();
        void Awake()
        {
            Controller = new UILoadCtrl(assetBundleFile);
            controllers.Add(Controller);
            RegisterBundleEvents();
        }

        private void RegisterBundleEvents()
        {
            for (int i = 0; i < bundles.Count; i++)
            {
                UIBundleInfo trigger = bundles[i];
                switch (trigger.type)
                {
                    case UIBundleInfo.Type.Button:
                        RegisterButtonEvents(trigger);
                        break;
                    case UIBundleInfo.Type.Toggle:
                        RegisterToggleEvents(trigger);
                        break;
                    case UIBundleInfo.Type.Name:
                        RegisterMessageEvents(trigger);
                        break;
                    case UIBundleInfo.Type.Enable:
                        RegisterEnableEvents(trigger);
                        break;
                    default:
                        break;
                }
            }
        }
        private void RegisterMessageEvents(UIBundleInfo trigger)
        {
            UnityAction<object> createAction = (x) =>
            {
                trigger.Data = x;
                Controller.GetGameObjectFromBundle(trigger);
            };

            UnityAction<object> handInfoAction = (data) =>
            {
                trigger.Data = data;
                IPanelName irm = trigger.instence.GetComponent<IPanelName>();
                irm.HandleData(trigger.Data);
            };

            trigger.OnCreate = (x) =>
            {
                IPanelName irm = x.GetComponent<IPanelName>();
                if (irm != null)
                {
                    trigger.instence = x;
                    irm.HandleData(trigger.Data);
                    Remove(trigger.assetName, createAction);
                    Record(trigger.assetName, handInfoAction);
                    irm.OnDelete += () =>
                    {
                        trigger.instence = null;
                        Remove(trigger.assetName, handInfoAction);
                        Record(trigger.assetName, createAction);
                    };
                }
                RegisterDestoryAction(trigger.assetName, x);
            };

            Record(trigger.assetName, createAction);

            onDestroy += () =>
            {
                Remove(trigger.assetName, createAction);
            };
        }

        private void RegisterToggleEvents(UIBundleInfo trigger)
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
                IPanelToggle it = x.GetComponent<IPanelToggle>();
                if (it != null)
                {
                    it.toggle = trigger.toggle;

                    trigger.toggle.onValueChanged.RemoveListener(CreateByToggle);

                    it.OnDelete += () =>
                    {
                        trigger.toggle.onValueChanged.AddListener(CreateByToggle);
                    };
                }
                RegisterDestoryAction(trigger.assetName, x);
            };
        }

        private void RegisterButtonEvents(UIBundleInfo trigger)
        {
            UnityAction CreateByButton = () =>
            {
                Controller.GetGameObjectFromBundle(trigger);
            };
            trigger.button.onClick.AddListener(CreateByButton);
            onDestroy += () => { trigger.button.onClick.RemoveAllListeners(); };
            trigger.OnCreate = (x) =>
            {
                IPanelButton ib = x.GetComponent<IPanelButton>();
                if (ib != null)
                {
                    ib.Btn = trigger.button;
                    trigger.button.onClick.RemoveListener(CreateByButton);

                    ib.OnDelete += () =>
                    {
                        trigger.button.onClick.AddListener(CreateByButton);
                    };
                }
                RegisterDestoryAction(trigger.assetName, x);
            };
        }
        private void RegisterEnableEvents(UIBundleInfo trigger)
        {
            UnityAction onEnableAction = () =>
            {
                Controller.GetGameObjectFromBundle(trigger);
            };

            trigger.OnCreate = (x) =>
            {
                trigger.Data = x;
                IPanelEnable irm = x.GetComponent<IPanelEnable>();
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
                RegisterDestoryAction(trigger.assetName, x);
            };

            onEnable += onEnableAction;
        }

        private void RegisterDestoryAction(string assetName,GameObject x)
        {
            string key = addClose + assetName;
            RemoveEvents(key);
            Record(key, new UnityAction<object>((y)=> {
                if (x != null) Destroy(x);
            }));
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
            controllers.Remove(Controller);
        }
    }
}