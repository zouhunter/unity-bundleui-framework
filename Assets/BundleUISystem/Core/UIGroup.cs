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
#if UNITY_EDITOR
        public UILoadType defultType = UILoadType.LocalBundle;
#endif
        public List<UIBundleInfo> bundles = new List<UIBundleInfo>();
        public List<BundleInfo> rbundles = new List<BundleInfo>();
        public List<PrefabInfo> prefabs = new List<PrefabInfo>();
        public List<GroupObj> groupObjs = new List<GroupObj>();
        public string assetUrl;
        public string menu;
        private EventHold eventHold = new EventHold();


        private IUILoadCtrl _localLoader;
        private IUILoadCtrl _remoteLoader;
        private event UnityAction onDestroy;
        private event UnityAction onEnable;
        private event UnityAction onDisable;
        private const string _close = "close";
        private const string _hide = "hide";
        private const string _onCallBack = "onCallBack";

        private static List<IUILoadCtrl> controllers = new List<IUILoadCtrl>();
        private static List<EventHold> eventHolders = new List<EventHold>();
        public static UnityEngine.Events.UnityAction<string> MessageNotHandled;
        private IUILoadCtrl LocalLoader
        {
            get
            {
                if (_localLoader == null)
                {
                    _localLoader = new UIBundleLoadCtrl(transform);
                    controllers.Add(_localLoader);
                }
                return _localLoader;
            }
        }
        private IUILoadCtrl RemoteLoader
        {
            get
            {
                if (_remoteLoader == null)
                {
                    _remoteLoader = new UIBundleLoadCtrl(assetUrl, menu, transform);
                    controllers.Add(_remoteLoader);
                }
                return _remoteLoader;
            }
        }
        void Awake()
        {
            eventHolders.Add(eventHold);
            RegistBaseUIEvents();
            RegistSubUIEvents();
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
            if (onDestroy != null)
            {
                onDestroy.Invoke();
            }

            if (_localLoader != null) controllers.Remove(_localLoader);

            if (_remoteLoader != null) controllers.Remove(_remoteLoader);

            eventHolders.Remove(eventHold);
        }

        private void RegistBaseUIEvents()
        {
            if (prefabs.Count > 0)
            {
                RegisterBundleEvents(LocalLoader, prefabs.ConvertAll<ItemInfoBase>(x => x));
            }

            if (bundles.Count > 0)
            {
                RegisterBundleEvents(LocalLoader, bundles.ConvertAll<ItemInfoBase>(x => x));
            }

            if (rbundles.Count > 0)
            {
                RegisterBundleEvents(RemoteLoader, rbundles.ConvertAll<ItemInfoBase>(x => x));
            }
        }

        private void RegistSubUIEvents()
        {
            foreach (var item in groupObjs)
            {
                if (item.prefabs.Count > 0)
                {
                    RegisterBundleEvents(LocalLoader, item.prefabs.ConvertAll<ItemInfoBase>(x => x));
                }

                if (item.bundles.Count > 0)
                {
                    RegisterBundleEvents(LocalLoader, item.bundles.ConvertAll<ItemInfoBase>(x => x));
                }

                if (item.rbundles.Count > 0)
                {
                    RegisterBundleEvents(RemoteLoader, item.rbundles.ConvertAll<ItemInfoBase>(x => x));
                }
            }
        }
        #region 事件注册
        private void RegisterBundleEvents(IUILoadCtrl loadCtrl, List<ItemInfoBase> bundles)
        {
            for (int i = 0; i < bundles.Count; i++)
            {
                ItemInfoBase trigger = bundles[i];
                switch (trigger.type)
                {
                    case UIBundleInfo.Type.Button:
                        RegisterButtonEvents(loadCtrl, trigger);
                        break;
                    case UIBundleInfo.Type.Toggle:
                        RegisterToggleEvents(loadCtrl, trigger);
                        break;
                    case UIBundleInfo.Type.Name:
                        RegisterMessageEvents(loadCtrl, trigger);
                        break;
                    case UIBundleInfo.Type.Enable:
                        RegisterEnableEvents(loadCtrl, trigger);
                        break;
                    default:
                        break;
                }
            }
        }
        private void RegisterMessageEvents(IUILoadCtrl loadCtrl, ItemInfoBase trigger)
        {
            UnityAction<UIData> createAction = (x) =>
            {
                trigger.dataQueue.Enqueue(x);//
                loadCtrl.GetGameObjectInfo(trigger);
            };

            UnityAction<UIData> handInfoAction = (data) =>
            {
                IPanelName irm = trigger.instence.GetComponent<IPanelName>();
                irm.HandleData(data);
            };

            trigger.OnCreate = (x) =>
            {
                IPanelName irm = x.GetComponent<IPanelName>();
                if (irm != null)
                {
                    trigger.instence = x;
                    eventHold.Remove(trigger.assetName, createAction);
                    eventHold.Record(trigger.assetName, handInfoAction);
                    irm.onDelete += () =>
                    {
                        trigger.instence = null;
                        eventHold.Remove(trigger.assetName, handInfoAction);
                        eventHold.Record(trigger.assetName, createAction);
                    };
                    irm.onCallBack += (state) =>
                    {
                        InvokeCallBack(trigger.assetName, state);
                    };

                    while (trigger.dataQueue.Count > 0)
                    {
                        var data = trigger.dataQueue.Dequeue();
                        irm.HandleData(data);
                    }
                }
                RegisterHideAndDestroyAction(trigger.assetName, x);
            };

            eventHold.Record(trigger.assetName, createAction);

            onDestroy += () =>
            {
                eventHold.Remove(trigger.assetName, createAction);
            };
        }
        private void RegisterToggleEvents(IUILoadCtrl loadCtrl, ItemInfoBase trigger)
        {
            UnityAction<bool> CreateByToggle = (x) =>
            {
                if (x)
                {
                    trigger.toggle.interactable = false;
                    loadCtrl.GetGameObjectInfo(trigger);
                }
                else
                {
                    Destroy((GameObject)trigger.instence);
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

                trigger.instence = x;
                IPanelToggle it = x.GetComponent<IPanelToggle>();
                if (it != null)
                {
                    it.toggle = trigger.toggle;

                    trigger.toggle.onValueChanged.RemoveListener(CreateByToggle);

                    it.onDelete += () =>
                    {
                        trigger.toggle.onValueChanged.AddListener(CreateByToggle);
                    };
                }
                RegisterHideAndDestroyAction(trigger.assetName, x);
            };
        }
        private void RegisterButtonEvents(IUILoadCtrl loadCtrl, ItemInfoBase trigger)
        {
            UnityAction CreateByButton = () =>
            {
                loadCtrl.GetGameObjectInfo(trigger);
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

                    ib.onDelete += () =>
                    {
                        trigger.button.onClick.AddListener(CreateByButton);
                    };
                }
                RegisterHideAndDestroyAction(trigger.assetName, x);
            };
        }
        private void RegisterEnableEvents(IUILoadCtrl loadCtrl, ItemInfoBase trigger)
        {
            UnityAction onEnableAction = () =>
            {
                loadCtrl.GetGameObjectInfo(trigger);
            };

            trigger.OnCreate = (x) =>
            {
                trigger.instence = x;
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
                        if (trigger.instence != null && trigger.instence is GameObject)
                        {
                            Destroy((GameObject)trigger.instence);
                        }
                    };
                }
                RegisterHideAndDestroyAction(trigger.assetName, x);
            };

            onEnable += onEnableAction;
        }
        private void RegisterHideAndDestroyAction(string assetName, GameObject x)
        {
            string key = _close + assetName;
            eventHold.Remove(key);
            eventHold.Record(key, new UnityAction<UIData>((y) =>
            {
                if (x != null) Destroy(x);
            }));

            key = _hide + assetName;
            eventHold.Remove(key);
            eventHold.Record(key, new UnityAction<UIData>((y) =>
            {
                if (x != null) x.gameObject.SetActive(false);
            }));
        }
        private void InvokeCallBack(string assetName, UIData node)
        {
            var key = _onCallBack + assetName;
            eventHold.NotifyObserver(key, node);
        }
        #endregion

        #region 触发事件
     
        public static void Open(string assetName, UnityAction<UIData> onCallBack = null, UIData data = null)
        {
            List<EventHold> haveEventHolds = new List<EventHold>();
            TraverseHold((eventHold) =>
            {
                var handle = eventHold.HaveRecord(assetName);
                if (handle)
                {
                    haveEventHolds.Add(eventHold);
                }
            });

            if (haveEventHolds.Count == 0)
            {
                NoMessageHandle(assetName);
            }
            else
            {

                var callBackKey = _onCallBack + assetName;
                for (int i = 0; i < haveEventHolds.Count; i++)
                {
                    haveEventHolds[i].Remove(callBackKey);
                    if (onCallBack != null)
                    {
                        haveEventHolds[i].Record(callBackKey, onCallBack);
                    }
                }
                for (int i = 0; i < haveEventHolds.Count; i++)
                {
                    haveEventHolds[i].NotifyObserver(assetName, data);
                }
            }
        }
        public static void Open<T>(string assetName, UnityAction<UIData> onCallBack = null, T data = default(T))
        {
            UIData uidata = UIData.Allocate<T>(data);
            Open(assetName, onCallBack, uidata);
        }
        public static void Open<T>(string assetName, T data)
        {
            UIData uidata = UIData.Allocate<T>(data);
            Open(assetName, null, (UIData)uidata);
        }
        public static void Open(string assetName, UIData data)
        {
            Open(assetName, null, data);
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

            var key = (_close + assetName);

            TraverseHold((eventHold) =>
            {
                eventHold.NotifyObserver(key);
            });
        }
        public static void Hide(string assetName)
        {
            var key = (_hide + assetName);

            TraverseHold((eventHold) =>
            {
                eventHold.NotifyObserver(key);
            });
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