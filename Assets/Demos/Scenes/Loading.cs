using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class Loading : MonoBehaviour {
    public string targetSceneName = "Demo";
    public string targetSceneBundle = "scene/demo";
    public Slider progressBar;
    private void Awake()
    {
        AssetBundleLoader.Instence.LoadLevelFromUrlAsync(targetSceneBundle, targetSceneName, false, UpdateProgress);
    }
    private void UpdateProgress(float progress)
    {
        progressBar.value = progress;
    }
}
