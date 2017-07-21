using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public abstract class ItemInfoBase {
    public abstract string IDName { get; }
    public GameObject instence;
    public bool reset;
    public int parentLayer;
    public object Data { get; set; }
    public UnityAction<GameObject> OnCreate;
}
