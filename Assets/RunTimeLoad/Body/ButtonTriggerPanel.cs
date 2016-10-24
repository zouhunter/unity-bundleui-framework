using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

public class ButtonTriggerPanel : TriggerPanel
{
    public Button button;

    protected override void Start()
    {
        base.Start();
        if (button != null) button.onClick.AddListener(CreateByButton);
    }

    void CreateByButton()
    {
        button.onClick.RemoveListener(CreateByButton);
        Controller.GetPanelFromBundle<IRunTimeButton>(assetName, (x) =>
        {
            x.btn = button;
        });
    }
}
