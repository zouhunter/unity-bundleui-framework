using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using BundleUISystem;
public class Panel1 : UIPanelTemp
{
    public override void HandleData(JSONNode data)
    {
        Debug.Log(data.ToString());
    }
}