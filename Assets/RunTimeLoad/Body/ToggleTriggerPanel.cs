using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

public class ToggleTriggerPanel : TriggerPanel {

    public Toggle toggle;

    protected override void Start()
    {
        base.Start();
        if (toggle != null) toggle.onValueChanged.AddListener(CreateByToggle);
    }

    void CreateByToggle(bool bo)
    {
        toggle.onValueChanged.RemoveListener(CreateByToggle);
        Controller.GetPanelFromBundle<IRunTimeToggle>(assetName, (x) => {
            x.toggle = toggle;
        });
    }
}
