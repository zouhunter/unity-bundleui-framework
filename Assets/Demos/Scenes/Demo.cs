using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyNamespace;
public class Demo : MonoBehaviour {
    void OnGUI()
    {
        if (GUILayout.Button("打开panel1"))
        {
            BundleUISystem.UIGroup.Open<Panel1>("hellow world");
        }
    }
}
