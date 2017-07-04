using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using Rotorz.ReorderableList;
using BundleUISystem;
[CustomEditor(typeof(UIGroup)),CanEditMultipleObjects]
public class UIGroupDrawer : UIDrawerTemp
{

}
[CustomEditor(typeof(UIGroupObj))]
public class UIGroupObjDrawer : UIDrawerTemp
{

}

public class UIDrawerTemp : Editor {
    SerializedProperty script;
    SerializedProperty groupObjsProp;
    SerializedProperty bundlesProp;
    SerializedProperty defultProp;
    SerializedProperty assetUrlProp;
    SerializedProperty menuProp;

    DragAdapt bundlesAdapt;
    bool swink;
    int id;
    string[] option = new string[] {"默认","指定" };
    List<GameObject> created;
    private void OnEnable()
    {
        script = serializedObject.FindProperty("m_Script");
        bundlesProp = serializedObject.FindProperty("bundles");
        bundlesAdapt = new DragAdapt(bundlesProp);
        groupObjsProp = serializedObject.FindProperty("groupObjs");
        defultProp = serializedObject.FindProperty("defult");
        assetUrlProp = serializedObject.FindProperty("assetUrl");
        menuProp = serializedObject.FindProperty("menu");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawScript();
        DrawOption();
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
    private void DrawOption()
    {
        id = GUILayout.Toolbar(id, option);
        defultProp.boolValue = id == 0;
        if (!defultProp.boolValue)
        {
            EditorGUILayout.PropertyField(assetUrlProp);
            EditorGUILayout.PropertyField(menuProp);
        }
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
        ReorderableListGUI.Title("静态面板");
        ReorderableListGUI.ListField(groupObjsProp);
        ReorderableListGUI.Title("动态面板");
        Rotorz.ReorderableList.ReorderableListGUI.ListField(bundlesAdapt);
    }
    private void QuickUpdate()
    {
        for (int i = 0; i < bundlesProp.arraySize; i++)
        {
            var itemProp = bundlesProp.GetArrayElementAtIndex(i);
            var prefabProp = itemProp.FindPropertyRelative("prefab");
            var assetNameProp = itemProp.FindPropertyRelative("assetName");
            var bundleNameProp = itemProp.FindPropertyRelative("bundleName");

            if (prefabProp.objectReferenceValue == null)
            {
                UnityEditor.EditorUtility.DisplayDialog("空对象", assetNameProp.stringValue + "预制体为空", "确认");
                continue;
            }

            string assetPath = UnityEditor.AssetDatabase.GetAssetPath(prefabProp.objectReferenceValue);

            UnityEditor.AssetImporter importer = UnityEditor.AssetImporter.GetAtPath(assetPath);

            assetNameProp.stringValue = prefabProp.objectReferenceValue.name;
            bundleNameProp.stringValue = importer.assetBundleName;

            if (string.IsNullOrEmpty(bundleNameProp.stringValue))
            {
                UnityEditor.EditorUtility.DisplayDialog("提示", "预制体" + assetNameProp.stringValue + "没有assetBundle标记", "确认");
                return;
            }
        }
        UnityEditor.EditorUtility.SetDirty(this);

    }
    private void RemoveDouble()
    {
        compair: List<string> temp = new List<string>();

        for (int i = 0; i < bundlesProp.arraySize; i++)
        {
            var itemProp = bundlesProp.GetArrayElementAtIndex(i);
            var assetNameProp = itemProp.FindPropertyRelative("assetName");
            if(!temp.Contains(assetNameProp.stringValue)){
                temp.Add(assetNameProp.stringValue);
            }
            else
            {
                bundlesProp.DeleteArrayElementAtIndex(i);
                goto compair;
            }
        }
    }
    private void OpenAll()
    {
        if (created != null){
            return;
        }
        created = new List<GameObject>();
        for (int i = 0; i < bundlesProp.arraySize; i++)
        {
            var itemProp = bundlesProp.GetArrayElementAtIndex(i);
            var prefabProp = itemProp.FindPropertyRelative("prefab");
            var resetProp = itemProp.FindPropertyRelative("reset");
            GameObject instence = PrefabUtility.InstantiatePrefab(prefabProp.objectReferenceValue) as GameObject;
            if(target is UIGroup)
            {
                instence.transform.SetParent((target as UIGroup).transform, resetProp.boolValue);
            }
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
    private void SortAll()
    {
        for (int i = 0; i < bundlesProp.arraySize; i++)
        {
            for (int j = i; j < bundlesProp.arraySize - i - 1; j++)
            {
                var itemj = bundlesProp.GetArrayElementAtIndex(j).FindPropertyRelative("assetName");
                var itemj1 = bundlesProp.GetArrayElementAtIndex(j + 1).FindPropertyRelative("assetName");
                if (string.Compare(itemj.stringValue,itemj1.stringValue) > 0)
                {
                    bundlesProp.MoveArrayElement(j, j + 1);
                }
            }
        }
    }
    private void SaveAll()
    {
        var items = FindObjectsOfType<UIPanelTemp>();
        foreach (var item in items)
        {
            var prefab = PrefabUtility.GetPrefabParent(item.gameObject);
            if (prefab != null)
            {
                var root = PrefabUtility.FindPrefabRoot((GameObject)prefab);
                if (root != null)
                {
                    PrefabUtility.ReplacePrefab(item.gameObject, root, ReplacePrefabOptions.ConnectToPrefab);
                }
            }
        }
    }
}
