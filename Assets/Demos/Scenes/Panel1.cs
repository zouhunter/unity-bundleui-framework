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
    public Panel1Data(string arg0)
    {
        this.arg0 = arg0;
    }
    public static implicit operator JSONObject(Panel1Data s)
    {
        return new JSONObject( JsonUtility.ToJson(s));
    }
    public static implicit operator Panel1Data(JSONObject s)
    {
        Debug.Log(s.ToString());
        return JsonUtility.FromJson<Panel1Data>(s.ToString());
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
    public override void HandleData(JSONObject obj)
    {
        Panel1Data data = obj;
        Debug.Log(data.arg0);
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