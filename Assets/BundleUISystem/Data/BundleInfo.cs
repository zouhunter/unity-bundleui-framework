using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
namespace BundleUISystem
{
    [System.Serializable]
    public class BundleInfo: ItemInfoBase
    {
        public string assetName;
        public string bundleName;
        public Type type;

        public Button button;
        public Toggle toggle;

        public override string IDName { get { return bundleName + assetName; } }
        public enum Type
        {
            Enable,
            Name
        }
    }
}
