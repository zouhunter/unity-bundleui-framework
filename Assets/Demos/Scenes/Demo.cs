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
        if (GUILayout.Button("关闭panel1"))
        {
            BundleUISystem.UIGroup.Close<Panel1>();
        }
    }
}
