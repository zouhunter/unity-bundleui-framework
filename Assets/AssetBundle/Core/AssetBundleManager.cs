using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using AssetBundles;

public class AssetBundleManager : SingleManager<AssetBundleManager>
{
#if UNITY_EDITOR
    //private static int m_SimulateAssetBundleInEditor;
    private static string kSimulateAssetBundles = "simulateinEditor";
    private ISimulationLoader simuationLoader = new SimulationLoader();
    // Flag to indicate if we want to simulate assetBundles in Editor without building them actually.
    public static bool SimulateAssetBundleInEditor
    {
        get
        {
            return UnityEditor.EditorPrefs.GetBool(kSimulateAssetBundles, true);
        }
        set
        {
            UnityEditor.EditorPrefs.SetBool(kSimulateAssetBundles, value);
        }
    }


#endif

    private ILocalAssetLoader localLoader = new LocalAssetLoader();
    public AssetBundleManager SetActiveManu(string menu)
    {
        bundleLoadCtrlDic.TryGetValue(menu, out activeLoader);
        return this;
    }
    private IUrlAssetBundleLoadCtrl activeLoader;
    private Dictionary<string, IUrlAssetBundleLoadCtrl> bundleLoadCtrlDic = new Dictionary<string, IUrlAssetBundleLoadCtrl>();
    /// <summary>
    /// 设置连接路径，menu要唯一
    /// </summary>
    /// <param name="url"></param>
    /// <param name="menu"></param>
    public void AddConnect(string url, string menu)
    {
        if (!bundleLoadCtrlDic.ContainsKey(menu))
        {
            UrlAssetBundleLoadCtrl loadCtrl = new UrlAssetBundleLoadCtrl(url, menu);
            bundleLoadCtrlDic.Add(menu, loadCtrl);
        }
    }

    void Update()
    {
        if (activeLoader != null)
        {
            activeLoader.UpdateDownLand();
        }
        localLoader.UpdateDownLand();
    }
    /// <summary>
    /// 加载依赖关系
    /// </summary>
    /// <param name="onMenuLoad"></param>
    private void LoadMenu(UnityAction onMenuLoad)
    {
        AssetBundleLoadOperation initopera = activeLoader.Initialize();
        StartCoroutine(WaitInalize(initopera, onMenuLoad));
    }
    /// <summary>
    /// 从url异步加载一个资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="assetBundleName"></param>
    /// <param name="assetName"></param>
    /// <param name="onAssetLoad"></param>
    public void LoadAssetFromUrlAsync<T>(string assetBundleName, string assetName, UnityAction<T> onAssetLoad) where T : UnityEngine.Object
    {
#if UNITY_EDITOR
        if (SimulateAssetBundleInEditor)
        {
            T asset = simuationLoader.LoadAsset<T>(assetBundleName, assetName);
            onAssetLoad(asset);
            return;
        }
#endif
        if (activeLoader != null)
        {
            LoadMenu(() =>
            {
                onAssetLoad += (x) => { activeLoader.UnloadAssetBundle(assetBundleName); };
                AssetBundleLoadAssetOperation operation = activeLoader.LoadAssetAsync(assetBundleName, assetName, typeof(T));
                StartCoroutine(WaitLoadObject(operation, onAssetLoad));
            });
        }
        else
        {
            Debug.Log("Please Set Menu");
        }
    }
    /// <summary>
    /// 从url异步从bundle中加载一组资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="assetBundleName"></param>
    /// <param name="assetNames"></param>
    /// <param name="allAssetLoad"></param>
    public void LoadAssetsFromUrlAsync<T>(string assetBundleName, string[] assetNames, UnityAction<T[]> allAssetLoad) where T : UnityEngine.Object
    {
#if UNITY_EDITOR
        if (SimulateAssetBundleInEditor)
        {
            T[] asset = simuationLoader.LoadAssets<T>(assetBundleName, assetNames);
            allAssetLoad(asset);
            return;
        }
#endif
        if (activeLoader != null)
        {
            LoadMenu(() =>
            {
                T[] objectPool = new T[assetNames.Length];
                int j = 0;


                for (int i = 0; i < assetNames.Length; i++)
                {
                    int index = i;
                    UnityAction<T> loadOnce = (x) =>
                    {
                        objectPool[index] = x;
                        j++;
                        if (j == assetNames.Length)
                        {
                            allAssetLoad(objectPool);
                            activeLoader.UnloadAssetBundle(assetBundleName);
                        }
                    };
                    AssetBundleLoadAssetOperation operation = activeLoader.LoadAssetAsync(assetBundleName, assetNames[index], typeof(T));
                    StartCoroutine(WaitLoadObject(operation, loadOnce));
                }
            });
        }
        else
        {
            Debug.Log("Please Set Menu");
        }
    }
    /// <summary>
    /// 从url加载出场景
    /// </summary>
    /// <param name="assetBundleName"></param>
    /// <param name="assetName"></param>
    /// <param name="isAddictive"></param>
    /// <param name="onLevelLoad"></param>
    public void LoadLevelFromUrlAsync(string assetBundleName, string assetName, bool isAddictive, UnityAction<float> onProgressChange)
    {
#if UNITY_EDITOR
        if (SimulateAssetBundleInEditor)
        {
            simuationLoader.LoadSceneAsync(assetBundleName, assetName, isAddictive, onProgressChange);
            return;
        }
#endif
        if (activeLoader != null)
        {
            LoadMenu(() =>
            {
                AssetBundleLoadLevelOperation operation = activeLoader.LoadLevelAsync(assetBundleName, assetName, isAddictive);
                StartCoroutine(WaitLoadLevel(operation, onProgressChange));
            });
        }
        else
        {
            Debug.Log("Please Set Menu");
        }
    }

    /// <summary>
    /// 本地Bundle包中加载独立资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filePath"></param>
    /// <param name="assetName"></param>
    /// <returns></returns>
    public T LoadAssetFromFile<T>(string filePath, string assetName) where T : UnityEngine.Object
    {
        return localLoader.LoadAsset<T>(filePath, assetName);
    }

    /// <summary>
    /// 本地加载场景
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filePath"></param>
    /// <param name="levelName"></param>
    /// <param name="onProgressChanged"></param>
    public void LoadLevelFromFile(string filePath, string levelName, UnityEngine.SceneManagement.LoadSceneMode loadMode, UnityAction<float> onProgressChanged)
    {
        if (System.IO.File.Exists(filePath))
        {
            AsyncOperation opera = localLoader.LoadLevelAsync(filePath, levelName, loadMode);
            StartCoroutine(WaitLoadLevel(opera,onProgressChanged));
        }
        else
        {
            Debug.LogWarning(filePath + ":notExist");
        }
    }
    /// <summary>
    /// 本地异步加载
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filePath"></param>
    /// <param name="assetName"></param>
    /// <param name="onAssetBundleLoad"></param>
    public void LoadAssetAsyncFromFile<T>(string filePath, string assetName, UnityAction<T> onAssetBundleLoad) where T : UnityEngine.Object
    {
        if (System.IO.File.Exists(filePath))
        {
            localLoader.LoadAssetAsync<T>(filePath, assetName, onAssetBundleLoad);
            localLoader.UnLoadAssetBundle(filePath);
        }
        else
        {
            Debug.LogWarning(filePath + ":notExist");
        }
    }
    /// <summary>
    /// 本地异步加载一组资源 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="assetBundleName"></param>
    /// <param name="assetNames"></param>
    /// <param name="allAssetLoad"></param>
    public void LoadAssetsAsyncFromFile<T>(string filePath, UnityAction<T[]> allAssetLoad, params string[] assetNames) where T : UnityEngine.Object
    {
        T[] objectPool = new T[assetNames.Length];
        int j = 0;
        for (int i = 0; i < assetNames.Length; i++)
        {
            int index = i;

            localLoader.LoadAssetAsync<T>(filePath, assetNames[index], (x) =>
            {
                objectPool[index] = x;
                j++;
                if (j == assetNames.Length)
                {
                    allAssetLoad(objectPool);
                    localLoader.UnLoadAssetBundle(filePath);
                }
            });
        }
    }

    IEnumerator WaitInalize(AssetBundleLoadOperation operation, UnityAction onActive)
    {
        yield return operation;
        if (onActive != null) onActive.Invoke();
    }
    IEnumerator WaitLoadObject<T>(AssetBundleLoadAssetOperation operation, UnityAction<T> onLoad) where T : UnityEngine.Object
    {
        yield return operation;
        if (onLoad != null)
        {
            T asset = operation.GetAsset<T>();
            onLoad.Invoke(asset);
        }
    }
    IEnumerator WaitLoadLevel(AssetBundleLoadLevelOperation operation, UnityAction<float> onProgressChanged)
    {
        while (!operation.IsDone())
        {
            if (operation.m_Request != null)
            {
                operation.m_Request.allowSceneActivation = false;
                if (onProgressChanged != null) onProgressChanged(operation.m_Request.progress);
                if (operation.m_Request.progress >= 0.9f)
                {
                    operation.m_Request.allowSceneActivation = true;
                }
            }
            yield return null;
        }
    }
    IEnumerator WaitLoadLevel(AsyncOperation operation, UnityAction<float> onProgressChanged)
    {
        while (!operation.isDone)
        {
            operation.allowSceneActivation = false;
            if(onProgressChanged!=null) onProgressChanged(operation.progress);
            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
