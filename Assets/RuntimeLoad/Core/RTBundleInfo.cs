using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class RTBundleInfo {
#if UNITY_EDITOR
    public GameObject prefab;
#endif
    public GameObject instence;
    public string assetName;
    public string bundleName;
    public bool reset;
    public Transform parent;
    public Type type;

    public Button button;
    public Toggle toggle;

    public object Data { get; set; }
    public UnityAction<GameObject> OnCreate;
    public string IDName { get { return bundleName + assetName; } }
    public enum Type
    {
        Button,
        Toggle,
        Enable,
        Name
    }
}
