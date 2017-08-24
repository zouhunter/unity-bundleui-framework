using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Demo : MonoBehaviour {
    void OnGUI()
    {
        if (GUILayout.Button("打开panel1"))
        {
            BundleUISystem.UIGroup.Open<Panel1>("hellow world");
        }
        if (GUILayout.Button("打开panel1 1000 次"))
        {
            for (int i = 0; i < 1000; i++)
            {
                BundleUISystem.UIGroup.Open<Panel1>("hellow world:" + i);
            }
        }
        if (GUILayout.Button("关闭panel1"))
        {
            BundleUISystem.UIGroup.Close<Panel1>();
        }
        if (GUILayout.Button("打开Poppanel【层级为10】"))
        {
            BundleUISystem.UIGroup.Open("PopPanel");
        }
        if (GUILayout.Button("关闭Poppanel"))
        {
            BundleUISystem.UIGroup.Close("PopPanel");
        }
        if (GUILayout.Button("打开Poppanel1【层级为6】"))
        {
            BundleUISystem.UIGroup.Open("PopPanel 1");
        }
        if (GUILayout.Button("关闭Poppanel1"))
        {
            BundleUISystem.UIGroup.Close("PopPanel 1");
        }
    }
}
