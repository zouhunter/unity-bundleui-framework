using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BundleUISystem;

public class Demo : MonoBehaviour
{
    string panel1 = "Panel1";
    string poppanel = "PopPanel";
    string poppanel1 = "PopPanel1";
    private void Start()
    {
        panel1 = PanelName.Panel1;
        poppanel = PanelName.PopPanel;
        poppanel1 = PanelName.PopPanel1;
    }
    void OnGUI()
    {
        if (GUILayout.Button("打开panel1"))
        {
            var table = new Hashtable();
            table["tableItem1"] = "你好";
            UIGroup.Open(panel1, (x) => { Debug.Log("callBack panel1" + x);  }, table);
        }
        if (GUILayout.Button("打开panel1 1000 次"))
        {
            for (int i = 0; i < 1000; i++)
            {
                var table = new Hashtable();
                table["tableItem1"] = "你好";
                BundleUISystem.UIGroup.Open(panel1, (x) => { Debug.Log("onClose panel1" + x); }, table);
            }
        }
        if (GUILayout.Button("隐藏panel1"))
        {
            BundleUISystem.UIGroup.Hide(panel1);
        }
        if (GUILayout.Button("关闭panel1"))
        {
            BundleUISystem.UIGroup.Close(panel1);
        }
        if (GUILayout.Button("打开Poppanel【层级为10】"))
        {
            BundleUISystem.UIGroup.Open(poppanel);
        }
        if (GUILayout.Button("关闭Poppanel"))
        {
            BundleUISystem.UIGroup.Close(poppanel);
        }
        if (GUILayout.Button("打开Poppanel1【层级为6】"))
        {
            BundleUISystem.UIGroup.Open(poppanel1);
        }
        if (GUILayout.Button("关闭Poppanel1"))
        {
            BundleUISystem.UIGroup.Close(poppanel1);
        }
    }
}

class User
{
    public string Name { get; set; }
}