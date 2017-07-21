using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using BundleUISystem;
[CreateAssetMenu(menuName ="生成/UI组")]
public class UIGroupObj : ScriptableObject {
    public string assetUrl;
    public string menu;
    public List<UIBundleInfo> bundles = new List<UIBundleInfo>();
    public List<PrefabInfo> prefabs = new List<PrefabInfo>();
    public List<BundleInfo> rbundles = new List<BundleInfo>();
}
