using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using BundleUISystem;
using System;

public class Panel1 : UIPanelTemp,IPointerClickHandler
{
    public override void HandleData(JSONObject data)
    {
        Debug.Log(data.str);
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        CallBack("[返回状态]");
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        CallBack(eventData.clickTime);
    }
}