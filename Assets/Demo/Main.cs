using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class Main : MonoBehaviour {
    void Awake()
    {
        AssetBundleManager.GetInstance().AddConnect("file:///" + Application.streamingAssetsPath, "AssetBundle");
    }
}
