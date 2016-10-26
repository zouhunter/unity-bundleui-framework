using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;


public class ToggleSwitchAblePanel : MonoBehaviour, IRunTimeToggle
{
    [Header("Panel开关")]
    public Toggle toggle;
    public GameObject panel;

    Toggle IRunTimeToggle.toggle
    {
        set
        {
            toggle = value;
        }
    }

    void Start()
    {
        if (toggle) toggle.onValueChanged.AddListener((x) => {
            panel.SetActive(x);
        });
    }
}
