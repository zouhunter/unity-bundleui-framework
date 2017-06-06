using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using BundleUISystem;
#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(UIPanelOpener)),CanEditMultipleObjects]
public class UIPanelOpenerDrawer:Editor
{
    public override void OnInspectorGUI()
    {
        var opener = (UIPanelOpener)target;
        opener.panelTemp = EditorGUILayout.ObjectField(opener.panelTemp, typeof(UIPanelTemp), true) as UIPanelTemp;
        if(opener.panelTemp) opener.panelName = opener.panelTemp.name;
        EditorGUI.BeginDisabledGroup(true);
        opener.panelName = EditorGUILayout.TextField(opener.panelName);
        EditorGUI.EndDisabledGroup();
        opener.destroyOnOpen = EditorGUILayout.Toggle("destroyOnOpen", opener.destroyOnOpen, EditorStyles.toggle);
    }
}
#endif
public class UIPanelOpener : MonoBehaviour {
#if UNITY_EDITOR
public UIPanelTemp panelTemp;
#endif
    public string panelName;
    public bool destroyOnOpen;
    public void OpenSend(object data)
    {
        UIGroup.Open(panelName,data);
        if(destroyOnOpen)
        {
            Destroy(gameObject);
        }
    }
    public void Open()
    {
        UIGroup.Open(panelName);
        if (destroyOnOpen)
        {
            Destroy(gameObject);
        }
    }
}
