using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using BundleUISystem.Internal;
namespace BundleUISystem
{
    public class UILoadCtrl : IUILoadCtrl
    {
        private AssetBundleManager assetLoader { get { return AssetBundleManager.GetInstance(); } }
        private List<string> _loadingKeys = new List<string>();
        private List<string> _cansaleKeys = new List<string>();
        public string Menu
        {
            get;
            private set;
        }

        public UILoadCtrl(string menu)
        {
            this.Menu = menu;
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetName"></param>
        /// <param name="onCreate"></param>
        public void GetGameObjectFromBundle(UIBundleInfo trigger)
        {
            if (_cansaleKeys.Contains(trigger.assetName)) _cansaleKeys.RemoveAll(x => x == trigger.assetName);
			
            if (!_loadingKeys.Contains(trigger.IDName))
            {
                _loadingKeys.Add(trigger.IDName);
                assetLoader.LoadAssetFromUrlAsync<GameObject>(trigger.bundleName, trigger.assetName, (x) =>
                {
                    if (x != null)
                    {
                        CreateInstance(x, trigger);
                        _loadingKeys.Remove(trigger.IDName);
                    }
                    else
                    {
                        Debug.Log(trigger.bundleName + ".." + trigger.assetName +  "-->空");
                    }
                });
            }
            else
            {
                Debug.Log("asset:" + trigger.IDName + "isLoading");
            }
        }
        /// <summary>
        /// 取消创建对象
        /// </summary>
        /// <param name="assetName"></param>
        public void CansaleLoadObject(string assetName)
        {
            _cansaleKeys.Add(assetName);
        }
        /// <summary>
        /// 获取对象实例
        /// </summary>
        void CreateInstance(GameObject prefab, UIBundleInfo trigger)
        {
            if (_cansaleKeys.Contains(trigger.assetName))
            {
                _cansaleKeys.Remove(trigger.assetName);
                return;
            }

            if (prefab == null || trigger == null)
            {
                return;
            }

            GameObject go = GameObject.Instantiate(prefab);

            go.SetActive(true);
            bool isworld = trigger.parent == null ? true : !trigger.parent.GetComponent<RectTransform>();
            go.transform.SetParent(trigger.parent, isworld);
            if (trigger.reset)
            {
                go.transform.position = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
            }
            if (trigger.OnCreate != null) trigger.OnCreate(go);
        }

    }
}