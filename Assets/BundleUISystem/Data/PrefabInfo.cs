using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace BundleUISystem
{
    [System.Serializable]
    public class PrefabInfo: ItemInfoBase
    {
        public string assetName;
        public GameObject prefab;
        public Type type;
        public Button button;
        public Toggle toggle;
        public override string IDName { get { return assetName + "(clone)"; } }
        public enum Type
        {
            Button,
            Toggle,
            Enable,
            Name
        }
    }
}
