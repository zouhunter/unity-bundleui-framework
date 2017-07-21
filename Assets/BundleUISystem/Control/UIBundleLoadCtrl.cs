using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using BundleUISystem.Internal;
namespace BundleUISystem
{
    public class UIBundleLoadCtrl : IUILoadCtrl
    {
        private AssetBundleLoader assetLoader;
        private List<string> _loadingKeys = new List<string>();
        private List<string> _cansaleKeys = new List<string>();
        private static Dictionary<Transform, Dictionary<int, Transform>> _parentsDic = new Dictionary<Transform, Dictionary<int, Transform>>();
        private Transform _root;
        public UIBundleLoadCtrl(Transform root, bool isRoot = true)
        {
            _root = root;
            if (!_parentsDic.ContainsKey(_root)){
                _parentsDic[_root] = new Dictionary<int, Transform>();
            }
            assetLoader = AssetBundleLoader.Instence;
        }
        public UIBundleLoadCtrl(string url, string menu, Transform root, bool isRoot = true)
        {
            _root = root;
            if (!_parentsDic.ContainsKey(_root)) {
                _parentsDic[_root] = new Dictionary<int, Transform>();
            }
            assetLoader = AssetBundleLoader.GetInstance(url, menu);
        }
        /// <summary>
        /// 创建对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetName"></param>
        /// <param name="onCreate"></param>
        public void GetGameObjectInfo(ItemInfoBase itemInfo)
        {
            var trigger = itemInfo as BundleInfo;

            if (_cansaleKeys.Contains(trigger.assetName)) _cansaleKeys.RemoveAll(x => x == trigger.assetName);

            if (!_loadingKeys.Contains(trigger.IDName))
            {
                _loadingKeys.Add(trigger.IDName);
                assetLoader.LoadAssetFromUrlAsync<GameObject>(trigger.bundleName, trigger.assetName, (x) =>
                {
                    if(_root == null)
                    {
                        Debug.Log("父节点已销毁");
                    }
                    else if (x != null)
                    {
                        CreateInstance(x, trigger);
                        _loadingKeys.Remove(trigger.IDName);
                    }
                    else
                    {
                        Debug.Log(trigger.bundleName + ".." + trigger.assetName + "-->空");
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
        private void CreateInstance(GameObject prefab, BundleInfo trigger)
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
            SetParent(trigger.parentLayer, go.transform, trigger.reset);
            if (trigger.reset)
            {
                go.transform.position = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
            }
            if (trigger.OnCreate != null) trigger.OnCreate(go);
        }

        private void SetParent(int layer, Transform child, bool reset)
        {
            Transform parent = null;
            var _parents = _parentsDic[_root];
            if (!_parents.TryGetValue(layer, out parent))
            {
                parent = new GameObject(layer.ToString()).transform;
                if (_root is RectTransform)
                {
                    var rectParent = parent.gameObject.AddComponent<RectTransform>();
                    rectParent.anchorMin = Vector2.zero;
                    rectParent.anchorMax = Vector2.one;
                    rectParent.offsetMin = Vector3.zero;
                    rectParent.offsetMax = Vector3.zero;
                    parent = rectParent;
                    parent.SetParent(_root, false);
                }
                else
                {
                    parent.SetParent(_root, true);
                }

                if (_parents.Count > layer)
                {
                    parent.SetSiblingIndex(layer);
                }
                _parents.Add(layer, parent);
            }

            child.SetParent(parent, !(_root is RectTransform));

            if (reset)
            {
                child.transform.position = Vector3.zero;
                child.transform.localRotation = Quaternion.identity;
            }
        }
    }
}