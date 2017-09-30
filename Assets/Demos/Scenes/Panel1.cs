using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using BundleUISystem;
using System;
using System.Collections;


[System.Serializable]
public class Panel1Data
{
    public string arg0;
    public Panel1Data(string ar0)
    {
        this.arg0 = "你好";
    }
}

public class Panel1 : UIPanelTemp,IPointerClickHandler
{
    public object obj;
    private void Start()
    {
        var table = new Hashtable();
        table["a"] = 1;
    }
    public override void HandleData(UIData obj)
    {
        var table = obj.Table;
        //Panel1Data data = (Panel1Data)obj.Data;
        Debug.Log(table["tableItem1"]);
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