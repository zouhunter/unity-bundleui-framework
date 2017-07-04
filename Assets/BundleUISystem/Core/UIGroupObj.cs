using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using BundleUISystem;
[CreateAssetMenu(menuName ="生成/UI组")]
public class UIGroupObj : ScriptableObject {
    public bool defult;
    public List<UIBundleInfo> bundles = new List<UIBundleInfo>();
    public List<UIGroupObj> groupObjs = new List<UIGroupObj>();
}
