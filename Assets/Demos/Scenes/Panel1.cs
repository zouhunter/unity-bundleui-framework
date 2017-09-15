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
        base.HandleData(data);
        Debug.Log(data.str);
        Debug.Log("注意：现在可实现关闭不销毁");
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        CallBack("[返回状态]");
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        CallBack(eventData.clickTime);
        gameObject.SetActive(false);
    }
}