using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public abstract class ItemInfoBase {
    public string assetName;
    public abstract string IDName { get; }
    public Button button;
    public Toggle toggle;
    public GameObject instence;
    public bool reset;
    public int parentLayer;
    public Type type;
    public object Data { get; set; }
    public UnityAction<GameObject> OnCreate;
    public enum Type
    {
        Button = 0,
        Toggle = 1,
        Enable = 2,
        Name = 3
    }
}
