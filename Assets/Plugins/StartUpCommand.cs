using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

public class StartUpCommand : Command
{
    public override void Execute(INotification notification)
    {
        Debug.Log("StartUpCommand");
        AssetBundleManager.Instance.AddConnect("file:///" + Application.streamingAssetsPath, "AssetBundle");
        RunTimeLoadController.Instance.InitlizeCreater("AssetBundle");
    }
}
