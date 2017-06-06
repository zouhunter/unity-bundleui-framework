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
            if (GUILayout.Button("排序"))
            {
                SortAll();
            }
            if (GUILayout.Button("批量保存"))
            {
                SaveAll();
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
    private void SortAll()
    {
        System.Comparison<UIBundleInfo> comp = (x, y) => { return string.Compare(x.assetName, y.assetName); };
        targetObj.bundles.Sort(comp);
    }
    private void SaveAll()
    {
        var items = FindObjectsOfType<UIPanelTemp>();
        foreach (var item in items)
        {
            var prefab = PrefabUtility.GetPrefabParent(item.gameObject);
            if(prefab != null)
            {
                var root = PrefabUtility.FindPrefabRoot((GameObject)prefab);
                if(root != null)
                {
                    PrefabUtility.ReplacePrefab(item.gameObject, root, ReplacePrefabOptions.ConnectToPrefab);
                }
            }
        }
    }
}
