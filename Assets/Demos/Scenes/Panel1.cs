using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using BundleUISystem;
public class Panel1 : UIPanelTemp
{
    public override void HandleData(JSONObject data)
    {
        Debug.Log(data.str);
    }
    public override JSONObject CallBackState
    {
        get
        {
            return "[返回状态]";
        }
    }
}