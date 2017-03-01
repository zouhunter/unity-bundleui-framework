using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;


public class RunTimeLoadController : IRunTimeLoadCtrl
{
    private AssetBundleManager assetLoader { get { return AssetBundleManager.GetInstance(); } }
    private List<string> _loadingKeys = new List<string>();
    public string Menu
    {
        get;
        private set;
    }

    public RunTimeLoadController(string menu)
    {
        this.Menu = menu;
    }

    /// <summary>
    /// 创建对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="assetName"></param>
    /// <param name="onCreate"></param>
    public void GetGameObjectFromBundle(RunTimeBundleInfo trigger)
    {
        if (!_loadingKeys.Contains(trigger.IDName))
        {
            _loadingKeys.Add(trigger.IDName);
            assetLoader.LoadAssetFromUrlAsync<GameObject>(trigger.bundleName, trigger.assetName, (x) =>
            {
                if (x != null)
                {
                    CreateInstance(x, trigger);
                    _loadingKeys.Remove(trigger.IDName);
                }
                else
                {
                    Debug.Log(x + "::空");
                }
            });
        }
        else
        {
            Debug.Log("asset:" + trigger.IDName + "isLoading");
        }
    }

    /// <summary>
    /// 获取对象实例
    /// </summary>
    void CreateInstance(GameObject prefab, RunTimeBundleInfo trigger)
    {
        if (prefab == null || trigger == null)
        {
            return;
        }
        GameObject go = GameObject.Instantiate(prefab);

        go.SetActive(true);
        go.transform.SetParent(trigger.parent, trigger.isWorld);
        if (trigger.OnCreate != null) trigger.OnCreate(go);
    }

    public void ClearLoaded()
    {
        //throw new NotImplementedException();
    }
}
