using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;


public class RunTimeLoadController : IRunTimeLoadCtrl
{
    private Dictionary<string, GameObject> loadedPrefabs = new Dictionary<string, GameObject>();
    private AssetBundleManager assetLoader { get { return AssetBundleManager.GetInstance()[Menu]; } }
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
        GameObject go;
        if (loadedPrefabs.TryGetValue(trigger.IDName, out go) && go != null)
        {
            CreateInstance(go, trigger);
        }
        else
        {
            assetLoader.LoadAssetFromUrlAsync<GameObject>(trigger.bundleName, trigger.assetName, (x) =>
            {
                if (x != null)
                {
                    loadedPrefabs[trigger.assetName] = x;
                    CreateInstance(x, trigger);
                }
                else
                {
                    Debug.Log(x + "::空");
                }
            });
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

    /// <summary>
    /// 清除缓存
    /// </summary>
    public void ClearLoaded()
    {
        loadedPrefabs.Clear();
    }
}
