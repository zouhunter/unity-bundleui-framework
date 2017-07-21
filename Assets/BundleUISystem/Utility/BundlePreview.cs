using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using BundleUISystem;
using AssetBundles;

public class BundlePreview : MonoBehaviour {
    [System.Serializable]
    public class Data
    {
        public string assetUrl;
        public string menu;
        public List<BundleInfo> rbundles = new List<BundleInfo>();
    }
    AssetBundleLoader loader;
    public Data data;
    public void Start()
    {
        loader = AssetBundleLoader.GetInstance(data.assetUrl, data.menu);
        var canvas = FindObjectOfType<Canvas>();
        for (int i = 0; i < data.rbundles.Count; i++)
        {
            loader.LoadAssetFromUrlAsync<GameObject>(data.rbundles[i].bundleName, data.rbundles[i].assetName, (x) =>
            {
               var y = Instantiate<GameObject>(x);
                if (y.GetComponent<RectTransform>())
                {
                    y.transform.SetParent(canvas.transform, false);
                }
            });
        }
    }
}
