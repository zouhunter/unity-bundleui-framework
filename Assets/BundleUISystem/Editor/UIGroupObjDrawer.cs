using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using Rotorz.ReorderableList;
using BundleUISystem;
[CustomEditor(typeof(UIGroupObj))]
public class UIGroupObjDrawer : Editor {
    SerializedProperty script;
    DragAdapt bundlesAdapt;
    UIGroupObj targetObj;
    bool swink;
    List<GameObject> created;
    private void OnEnable()
    {
        script = serializedObject.FindProperty("m_Script");
        bundlesAdapt = new DragAdapt(serializedObject.FindProperty("bundles"));
        targetObj = (UIGroupObj)target;
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawScript();
        DrawToolButtons();
        DrawRuntimeItems();
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawScript()
    {
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(script);
        EditorGUI.EndDisabledGroup();
    }

    private void DrawToolButtons()
    {
        using (var hor = new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("移除重复"))
            {
                RemoveDouble();
            }
            if (GUILayout.Button("快速更新"))
            {
                QuickUpdate();
            }
            if (GUILayout.Button("批量编辑"))
            {
                OpenAll();
            }
            if (GUILayout.Button("退出编辑"))
            {
                CloseAll();
            }
        }
       
    }

    private void DrawRuntimeItems()
    {
        Rotorz.ReorderableList.ReorderableListGUI.ListField(bundlesAdapt);
    }
    private void QuickUpdate()
    {
        foreach (var item in targetObj.bundles)
        {
            if (item.prefab == null)
            {
                UnityEditor.EditorUtility.DisplayDialog("空对象", item.assetName + "预制体为空", "确认");
                continue;
            }

            string assetPath = UnityEditor.AssetDatabase.GetAssetPath(item.prefab);

            UnityEditor.AssetImporter importer = UnityEditor.AssetImporter.GetAtPath(assetPath);

            item.assetName = item.prefab.name;
            item.bundleName = importer.assetBundleName;

            if (string.IsNullOrEmpty(item.bundleName))
            {
                UnityEditor.EditorUtility.DisplayDialog("提示", "预制体" + item.assetName +"没有assetBundle标记", "确认");
                return;
            }
        }
        UnityEditor.EditorUtility.SetDirty(this);

    }
    private void RemoveDouble()
    {
        List<UIBundleInfo> tempList = new List<UIBundleInfo>();
        for (int i = 0; i < targetObj.bundles.Count; i++)
        {
            if (tempList.Find(x => x.assetName == targetObj.bundles[i].assetName) == null)
            {
                tempList.Add(targetObj.bundles[i]);
            }
        }
        targetObj.bundles = new List<UIBundleInfo>(tempList);
    }
    private void OpenAll()
    {
        UIBundleInfo item;
        if (created != null){
            return;
        }
        created = new List<GameObject>();
        var uigroup = GameObject.FindObjectOfType<UIGroup>();
        for (int i = 0; i < targetObj.bundles.Count; i++)
        {
            item = targetObj.bundles[i];
            GameObject instence = PrefabUtility.InstantiatePrefab(item.prefab) as GameObject;
            instence.transform.SetParent(uigroup.transform,!(uigroup.transform is RectTransform));
            created.Add(instence);
        }
    }
    private void CloseAll()
    {
        if (created == null)
        {
            return;
        }
        for (int i = 0; i < created.Count; i++)
        {
            if(created[i] != null) DestroyImmediate(created[i]);
        }
        created.Clear();
        created = null;
    }
}
