using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;


public class Demo : MonoBehaviour {
    void Awake()
    {
        GameManager.Instance.LunchFrameWork();
    }
}
