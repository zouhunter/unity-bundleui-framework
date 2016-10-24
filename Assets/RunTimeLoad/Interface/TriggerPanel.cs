using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class TriggerPanel : MonoBehaviour {
    public enum DestroyType
    {
        DestroyGameObject,
        DestroyThis,
        DontDestroy,
        Hide,
    }
    public string bundleName;
    public string assetName;
    public bool autoUnLoad;
    public bool isWorld;
    public DestroyType destroyType;
    public IRunTimePanelCtrl Controller
    {
        get { return RuntimePanelController.Instance; }
    }

    protected virtual void Start()
    {
        Controller.RegisterRuntimePanel(this);
    }
    protected virtual void OnDestroy()
    {
        Controller.RemoveRuntimePanel(assetName);
    }
   
}
