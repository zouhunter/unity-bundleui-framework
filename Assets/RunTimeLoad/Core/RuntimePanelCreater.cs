using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

public interface IRunTimePanelCtrl
{
    string Menu { get; }
    string Url { get; }
    void InitlizeCreater(string menu, string url);
    void RegisterRuntimePanel(TriggerPanel panel);
    void RemoveRuntimePanel(string key);
    bool GetPanelFromBundle<T>(string assetName, UnityAction<T> onCreate);
    bool RemovePanelByName(string assetName);
}

public class RuntimePanelController:SingleLengon<RuntimePanelController>, IRunTimePanelCtrl
{
    private Dictionary<string, TriggerPanel> itemDic = new Dictionary<string, TriggerPanel>();
    private Dictionary<string, GameObject> loadedGameObject = new Dictionary<string, GameObject>();

    public string Menu
    {
        get;
        private set;
    }

    public string Url
    {
        get;
        private set;
    }
    public void InitlizeCreater(string url,string menu)
    {
        this.Menu = menu;
        this.Url = url;
        AssetBundleManager.Instance.AddConnect(url, menu);
    }

    /// <summary>
    /// 注册时间创建面版
    /// </summary>
    /// <param name="panel"></param>
    public void RegisterRuntimePanel(TriggerPanel panel)
    {
        if (!itemDic.ContainsKey(panel.assetName))
        {
            //Debug.Log("注册" + panel.assetName);
            itemDic.Add(panel.assetName, panel);
        }
        else
        {
            itemDic[panel.assetName] = panel;
            Debug.Log("覆盖：" + panel.assetName);
        }
    }

    /// <summary>
    /// 移除注册，无法创建
    /// </summary>
    /// <param name="key"></param>
    public void RemoveRuntimePanel(string key)
    {
        if (itemDic.ContainsKey(key))
        {
            //Debug.Log("注销" + key);
            itemDic.Remove(key);
        }
        else
        {
            Debug.LogWarning("没有"+ key);
        }
    }

    /// <summary>
    /// 创建Panel
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="assetName"></param>
    /// <param name="onCreate"></param>
    public bool GetPanelFromBundle<T>(string assetName, UnityAction<T> onCreate)
    {
        TriggerPanel item;
        if (itemDic.TryGetValue(assetName, out item))
        {
            GameObject go;
            if (loadedGameObject.TryGetValue(assetName, out go) && go != null)
            {
                CreateInstance(item, go, onCreate);
                return true;
            }
            else
            {
                AssetBundleManager.Instance.SetActiveManu(Menu).LoadAssetFromUrlAsync<GameObject>(item.bundleName, assetName, (x) =>
                {
                    if (x != null)
                    {
                        loadedGameObject[assetName] = x;
                        CreateInstance(item, x, onCreate);
                    }
                    else
                    {
                        Debug.Log(item + "空");
                    }
                });

            }
        }
        return false;
    }


    /// <summary>
    /// 移除指定面版，并返回成功写否
    /// </summary>
    public bool RemovePanelByName(string assetName)
    {
        if (loadedGameObject.ContainsKey(assetName))
        {
            UnityEngine. Object.Destroy(loadedGameObject[assetName].gameObject);
            loadedGameObject.Remove(assetName);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 是否加载过
    /// </summary>
    /// <param name="assetName"></param>
    /// <returns></returns>
    bool IsPanelActive<T>(string assetName, out T component)
    {
        if (loadedGameObject.ContainsKey(assetName))
        {
            component = loadedGameObject[assetName].GetComponent<T>();
            return true;
        }
        component = default(T);
        return false;
    }

    /// <summary>
    /// 获取对象实例
    /// </summary>
    void CreateInstance<T>(TriggerPanel item, GameObject x, UnityAction<T> onCreate)
    {
        if(item == null || x == null)
        {
            Debug.Log("场景切换bundle异步示停止");
            return;
        }

        GameObject go = GameObject.Instantiate(x);
        go.SetActive(true);
        go.transform.SetParent(item.transform, item.isWorld);
        if (onCreate != null) onCreate(go.GetComponent<T>());

        switch (item.destroyType)
        {
            case TriggerPanel.DestroyType.DestroyGameObject:
                go.transform.SetParent(item.transform.parent);
                UnityEngine.Object.Destroy(item.gameObject);
                break;
            case TriggerPanel.DestroyType.DestroyThis:
                UnityEngine.Object.Destroy(item);
                break;
            case TriggerPanel.DestroyType.DontDestroy:
                break;
            case TriggerPanel.DestroyType.Hide:
                //go.transform.SetParent(item.transform.parent);
                item.enabled = false;
                break;
            default:
                break;
        }
    }
}
