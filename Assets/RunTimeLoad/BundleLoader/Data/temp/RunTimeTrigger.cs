using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public abstract class RunTimeTrigger { 
    public string assetName;
    public string bundleName;
    public bool isWorld;
    public Transform parent;
    public string IDName { get { return bundleName + assetName; } }
    public UnityAction<GameObject> OnCreate;

}
