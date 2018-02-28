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

public class Panel1 : UIPanelTemp
{
    public object obj;

    public override void HandleData(object data)
    {
        base.HandleData(data);
        var table = (Hashtable)data;
        //Panel1Data data = (Panel1Data)obj.Data;
        Debug.Log(table["tableItem1"]);
        Debug.Log("注意：现在可实现任意时间调用CallBack不销毁");
        CallBack("[接收状态]");
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        CallBack("[返回状态]");
    }
}