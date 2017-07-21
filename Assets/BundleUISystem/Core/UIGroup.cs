using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using BundleUISystem.Internal;

namespace BundleUISystem
{
    public class UIGroup : MonoBehaviour
    {
        public List<UIBundleInfo> bundles = new List<UIBundleInfo>();
        public List<BundleInfo> rbundles = new List<BundleInfo>();
        public List<PrefabInfo> prefabs = new List<PrefabInfo>();
        public List<UIGroupObj> groupObjs = new List<UIGroupObj>();
        public string assetUrl;
        public string menu;
        private EventHold eventHold = new EventHold();
        private event UnityAction onDestroy;
        private event UnityAction onEnable;
        private event UnityAction onDisable;
        private IUILoadCtrl uibundleLoadCtrl;
        private IUILoadCtrl prefabLoadCtrl;
        private IUILoadCtrl remoteLoadCtrl;
        private const string addClose = "close";

        private static List<IUILoadCtrl> controllers = new List<IUILoadCtrl>();
        private static List<EventHold> eventHolders = new List<EventHold>();
        public static UnityEngine.Events.UnityAction<string> MessageNotHandled;

        void Awake()
        {
            InitLoadControllers();
            eventHolders.Add(eventHold);
            RegistUIEvents();
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
        private void OnDestroy()
        {
            if (onDestroy != null){
                onDestroy.Invoke();
            }
            controllers.Remove(uibundleLoadCtrl);
            eventHolders.Remove(eventHold);
        }
        private void InitLoadControllers()
        {
            uibundleLoadCtrl = new UILoadCtrl(transform);
            remoteLoadCtrl = new UILoadCtrl(assetUrl, menu, transform);
            controllers.Add(uibundleLoadCtrl);
            controllers.Add(remoteLoadCtrl);
        }

        private void RegistUIEvents()
        {
            RegisterPrefabEvents(prefabs);
            RegisterBundleEvents(bundles);

            foreach (var item in groupObjs)
            {
                RegisterPrefabEvents(item.prefabs);
                RegisterBundleEvents(item.bundles);
            }
        }
        #region 本地prefab
        private void RegisterPrefabEvents(List<PrefabInfo> prefabs)
        {

        }
        #endregion
        #region 本地bundle
        private void RegisterBundleEvents(List<UIBundleInfo> bundles)
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
                uibundleLoadCtrl.GetGameObjectFromBundle(trigger);
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
                    eventHold.Remove(trigger.assetName, createAction);
                    eventHold.Record(trigger.assetName, handInfoAction);
                    irm.OnDelete += () =>
                    {
                        trigger.instence = null;
                        eventHold.Remove(trigger.assetName, handInfoAction);
                        eventHold.Record(trigger.assetName, createAction);
                    };
                }
                RegisterDestoryAction(trigger.assetName, x);
            };

            eventHold.Record(trigger.assetName, createAction);

            onDestroy += () =>
            {
                eventHold.Remove(trigger.assetName, createAction);
            };
        }
        private void RegisterToggleEvents(UIBundleInfo trigger)
        {
            UnityAction<bool> CreateByToggle = (x) =>
            {
                if (x)
                {
                    trigger.toggle.interactable = false;
                    uibundleLoadCtrl.GetGameObjectFromBundle(trigger);
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
                uibundleLoadCtrl.GetGameObjectFromBundle(trigger);
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
                uibundleLoadCtrl.GetGameObjectFromBundle(trigger);
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
        private void RegisterDestoryAction(string assetName, GameObject x)
        {
            string key = addClose + assetName;
            eventHold.Remove(key);
            eventHold.Record(key, new UnityAction<object>((y) => {
                if (x != null) Destroy(x);
            }));
        }
        #endregion

        #region 触发事件
        public static void Open(string assetName, object data = null)
        {
            bool handled = true;
            TraverseHold((eventHold) =>
            {
                handled |= eventHold.NotifyObserver(assetName, data);
            });
            if (!handled)
            {
                NoMessageHandle(assetName);
            }
        }
        public static void Open<T>(object data = null) where T : UIPanelTemp
        {
            string assetName = typeof(T).ToString();
            Open(assetName, data);
        }
        public static void Close(string assetName)
        {
            foreach (var item in controllers)
            {
                if (item != null)
                {
                    item.CansaleLoadObject(assetName);
                }
            }

            var key = (addClose + assetName);

            TraverseHold((eventHold) =>
            {
                eventHold.NotifyObserver(key);
            });
        }
        public static void Close<T>() where T : UIPanelTemp
        {
            string assetName = typeof(T).ToString();
            Close(assetName);
        }
        private static void TraverseHold(UnityAction<EventHold> OnGet)
        {
            var list = new List<EventHold>(eventHolders);
            foreach (var item in list)
            {
                OnGet(item);
            }
        }
        public static void NoMessageHandle(string rMessage)
        {
            if (MessageNotHandled == null)
            {
                Debug.LogWarning("MessageDispatcher: Unhandled Message of type " + rMessage);
            }
            else
            {
                MessageNotHandled(rMessage);
            }
        }

        #endregion

    }
}