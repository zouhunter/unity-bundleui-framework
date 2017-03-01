using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using Rotorz.ReorderableList;
[CustomEditor(typeof(RunTimeObjectBehaiver))]
public class RuntimeObjectDrawer : Editor {
    SerializedProperty script;
    SerializedPropertyAdaptor adapt;
    RunTimeObjectBehaiver targetObj;
    bool swink;
    private void OnEnable()
    {
        script = serializedObject.FindProperty("m_Script");
       var ritems = serializedObject.FindProperty("bundles");
        adapt = new SerializedPropertyAdaptor(ritems);
        targetObj = (RunTimeObjectBehaiver)target;
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawScript();
        DrawToolButtons();
        DrawRuntimeItems();
        //base.OnInspectorGUI();
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
            if (GUILayout.Button("添加对象"))
            {
                AddNewItem();
            }
        }
       
    }

    private void DrawRuntimeItems()
    {
        Rotorz.ReorderableList.ReorderableListGUI.ListField(adapt);
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
                UnityEditor.EditorUtility.DisplayDialog("提示", "预制体没有assetBundle标记", "确认");
                return;
            }
        }
        UnityEditor.EditorUtility.SetDirty(this);

    }
    private void RemoveDouble()
    {
        List<RunTimeBundleInfo> tempList = new List<RunTimeBundleInfo>();
        for (int i = 0; i < targetObj.bundles.Count; i++)
        {
            if (tempList.Find(x => x.assetName == targetObj.bundles[i].assetName) == null)
            {
                tempList.Add(targetObj.bundles[i]);
            }
        }
        targetObj.bundles = new List<RunTimeBundleInfo>(tempList);
    }
    private void AddNewItem()
    {
        RunTimeBundleInfo item = new RunTimeBundleInfo();
        if (targetObj.bundles.Count > 0)
        {
            RunTimeBundleInfo lastItem = targetObj.bundles[targetObj.bundles.Count - 1];
            FieldInfo[] infos = typeof(RunTimeBundleInfo).GetFields();
            foreach (var info in infos)
            {
                info.SetValue(item, info.GetValue(lastItem));
            }
        }
        targetObj.bundles.Add(item);
    }
}
