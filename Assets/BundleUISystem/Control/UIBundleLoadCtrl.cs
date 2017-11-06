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
#if AssetBundleTools
        private AssetBundleLoader assetLoader;
#endif
        private List<string> _loadingKeys = new List<string>();
        private List<string> _cansaleKeys = new List<string>();
        //private static Dictionary<Transform, Dictionary<int, Transform>> _parentsDic = new Dictionary<Transform, Dictionary<int, Transform>>();
        private Transform _root;

        public UIBundleLoadCtrl(Transform root)
        {
            _root = root;
            //if (!_parentsDic.ContainsKey(_root))
            //{
            //    //Debug.Log(_root);
            //    _parentsDic[_root] = new Dictionary<int, Transform>();
            //}
#if AssetBundleTools
            assetLoader = AssetBundleLoader.Instence;
#endif
        }
        public UIBundleLoadCtrl(string url, string menu, Transform root)
        {
            _root = root;
            //if (!_parentsDic.ContainsKey(_root))
            //{
            //    _parentsDic[_root] = new Dictionary<int, Transform>();
            //}
#if AssetBundleTools
            assetLoader = AssetBundleLoader.GetInstance(url, menu);
#endif
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetName"></param>
        /// <param name="onCreate"></param>
        public void GetGameObjectInfo(ItemInfoBase itemInfo)
        {
            if (_cansaleKeys.Contains(itemInfo.assetName)) _cansaleKeys.RemoveAll(x => x == itemInfo.assetName);

            if (!_loadingKeys.Contains(itemInfo.IDName))
            {
                _loadingKeys.Add(itemInfo.IDName);
                var bInfo = itemInfo as BundleInfo;
                var pInfo = itemInfo as PrefabInfo;

                if (bInfo != null)
                {
                    GetGameObjectInfo(bInfo);
                }
                else if (pInfo != null)
                {
                    GetGameObjectInfo(pInfo);
                }
            }
        }
        /// <summary>
        /// BundleInfo创建对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetName"></param>
        /// <param name="onCreate"></param>
        public void GetGameObjectInfo(BundleInfo itemInfo)
        {
            var trigger = itemInfo as BundleInfo;
#if AssetBundleTools
            assetLoader.LoadAssetFromUrlAsync<GameObject>(trigger.bundleName, trigger.assetName, (x) =>
            {
                if (_root == null)
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
#endif
        }
        /// <summary>
        /// PrefabInfo创建对象
        /// </summary>
        /// <param name="iteminfo"></param>
        public void GetGameObjectInfo(PrefabInfo iteminfo)
        {
            var trigger = iteminfo as PrefabInfo;

            if (trigger.prefab != null)
            {
                CreateInstance(trigger.prefab, trigger);
                _loadingKeys.Remove(trigger.IDName);
            }
            else
            {
                Debug.Log(trigger.assetName + "-->空");
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
        private void CreateInstance(GameObject prefab, ItemInfoBase trigger)
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
            SetTranform(go, trigger.parentLayer, _root);
            //SetParent(trigger.parentLayer, go.transform/*, trigger.rematrix*/);
            //if (trigger.rematrix)
            //{
            //    go.transform.position = Vector3.zero;
            //    go.transform.localRotation = Quaternion.identity;
            //}
            if (trigger.OnCreate != null) trigger.OnCreate(go);
        }
        /// <summary>
        /// 设置实例对象父级
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="child"></param>
        /// <param name="reset"></param>
        //private void SetParent(ItemInfoBase.Layer layer, Transform child/*, bool reset*/)
        //{
        //    Transform parent = null;
        //    var _parents = _parentsDic[_root];
        //    if (!_parents.TryGetValue((int)layer, out parent))
        //    {
        //        parent = new GameObject(LayerToString(layer)).transform;
        //        if (_root is RectTransform)
        //        {
        //            var rectParent = parent.gameObject.AddComponent<RectTransform>();
        //            rectParent.anchorMin = Vector2.zero;
        //            rectParent.anchorMax = Vector2.one;
        //            rectParent.offsetMin = Vector3.zero;
        //            rectParent.offsetMax = Vector3.zero;
        //            parent = rectParent;
        //            parent.SetParent(_root, false);
        //        }
        //        else
        //        {
        //            parent.SetParent(_root, true);
        //        }
        //        _parents.Add((int)layer, parent);

        //        ResortParents(_parents);
        //    }

        //    child.SetParent(parent, !(_root is RectTransform));
        //}
        public static void SetTranform(GameObject item, ItemInfoBase.Layer layer, Transform parent)
        {
            string rootName = UIBundleLoadCtrl.LayerToString(layer);// LayerToString();
            var root = parent.transform.Find(rootName);
            if (root == null)
            {
                root = new GameObject(rootName).transform;
                if (parent is RectTransform)
                {
                    var rectParent = root.gameObject.AddComponent<RectTransform>();
                    rectParent.anchorMin = Vector2.zero;
                    rectParent.anchorMax = Vector2.one;
                    rectParent.offsetMin = Vector3.zero;
                    rectParent.offsetMax = Vector3.zero;
                    root = rectParent;
                    root.SetParent(parent, false);
                }
                else
                {
                    root.SetParent(parent, true);
                }

                if (rootName.StartsWith("-1"))
                {
                    root.SetAsLastSibling();
                }
                else
                {
                    int i = 0;
                    for (; i < parent.childCount; i++)
                    {
                        var ritem = parent.GetChild(i);
                        if (ritem.name.StartsWith("-1"))
                        {
                            break;
                        }
                        if (string.Compare(rootName, ritem.name) < 0)
                        {
                            break;
                        }
                    }
                    root.SetSiblingIndex(i);
                }
            }
            item.transform.SetParent(root, !(item.GetComponent<Transform>() is RectTransform));
        }

        public static string LayerToString(ItemInfoBase.Layer layer,bool showint = true)
        {
            string str = "";
            if(showint) str += (int)layer + "|";

            if ((int)layer == -1)
            {
                str += "[Top]";
            }
            else if ((int)layer == 0)
            {
                str += "[Bottom]";
            }
            else
            {
                if ((layer &ItemInfoBase.Layer.Heap) == ItemInfoBase.Layer.Heap)
                {
                    str += "[H]";
                }
                if ((layer & ItemInfoBase.Layer.Mask) == ItemInfoBase.Layer.Mask)
                {
                    str += "[M]";
                }
                if ((layer & ItemInfoBase.Layer.Pop) == ItemInfoBase.Layer.Pop)
                {
                    str += "[P]";
                }
                if ((layer & ItemInfoBase.Layer.Tip) == ItemInfoBase.Layer.Tip)
                {
                    str += "[T]";
                }
            }
            return str;
        }

        /// <summary>
        /// 重新排序
        /// </summary>
        /// <param name="parentDic"></param>
        private void ResortParents(Dictionary<int, Transform> parentDic)
        {
            int[] keys = new int[parentDic.Count];
            parentDic.Keys.CopyTo(keys, 0);
            Array.Sort(keys);
            for (int i = 0; i < keys.Length; i++){
                parentDic[keys[i]].SetAsLastSibling();
            }
            if(parentDic.ContainsKey(-1)){
                parentDic[-1].SetAsLastSibling();
            }
        }
    }
}