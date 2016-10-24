using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;


public class ControllTriggerPanel : TriggerPanel {
    private bool isLoading;
    private GameObject loadedPanel;

    protected override void Start()
    {
        base.Start();
        LoadBundlePanel();
    }

    public void LoadBundlePanel()
    {
        isLoading = true;
        Controller.GetPanelFromBundle<Transform>(assetName, (x) =>
        {
            if (isLoading)
            {
                loadedPanel = x.gameObject;
                isLoading = false;
            }
        });
    }

    public void DeleteBundlePanel()
    {
        if (loadedPanel != null)
        {
            Destroy(loadedPanel);
        }
        else if (isLoading)
        {
            isLoading = false;
        }
    }
}
