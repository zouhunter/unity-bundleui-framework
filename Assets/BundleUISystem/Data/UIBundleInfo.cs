using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
namespace BundleUISystem
{
    [System.Serializable]
    public class UIBundleInfo: BundleInfo
    {
#if UNITY_EDITOR
        public GameObject prefab;
#endif

    }
}