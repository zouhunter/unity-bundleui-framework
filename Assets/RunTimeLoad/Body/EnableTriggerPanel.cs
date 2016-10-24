using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

public class EnableTriggerPanel : TriggerPanel
{
    private bool isLoading;
    private GameObject loadedPanel;

    void OnEnable()
    {
        LoadBundlePanel();
    }

    void OnDisable()
    {
        DeleteBundlePanel();
    }

    public void LoadBundlePanel()
    {
        isLoading = true;
        Debug.Log(assetName);
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
        else if(isLoading)
        {
            isLoading = false;
        }
    }
}
