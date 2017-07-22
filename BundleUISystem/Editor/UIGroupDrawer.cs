using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using Rotorz.ReorderableList;
using BundleUISystem;
using System;
using AssetBundle = UnityEngine.AssetBundle;
[CustomEditor(typeof(UIGroup)), CanEditMultipleObjects]
public class UIGroupDrawer : UIDrawerTemp
{
    protected override void DrawRuntimeItems()
    {
        ReorderableListGUI.Title("共用资源列表");
        ReorderableListGUI.ListField(groupObjsProp);
        base.DrawRuntimeItems();
    }
}
[CustomEditor(typeof(UIGroupObj))]
public class UIGroupObjDrawer : UIDrawerTemp
{

}

public abstract class UIDrawerTemp : Editor
{
    protected SerializedProperty script;
    protected SerializedProperty groupObjsProp;
    protected SerializedProperty bundlesProp;
    protected SerializedProperty prefabsProp;
    protected SerializedProperty rbundlesProp;
    protected SerializedProperty assetUrlProp;
    protected SerializedProperty menuProp;
    protected SerializedProperty defultTypeProp;
    protected DragAdapt bundlesAdapt;
    protected DragAdapt prefabsAdapt;
    protected DragAdapt rbundlesAdapt;
    protected bool swink;
#if AssetBundleTools
    protected string[] option = new string[] { "预制", "本地", "路径" };
#else
    protected string[] option = new string[] { "预制"};
#endif
    protected static List<GameObject> created = new List<GameObject>();
    private void OnEnable()
    {
        script = serializedObject.FindProperty("m_Script");
        bundlesProp = serializedObject.FindProperty("bundles");
        bundlesAdapt = new DragAdapt(bundlesProp, "bundles");
        prefabsProp = serializedObject.FindProperty("prefabs");
        prefabsAdapt = new DragAdapt(prefabsProp, "prefabs");
        rbundlesProp = serializedObject.FindProperty("rbundles");
        rbundlesAdapt = new DragAdapt(rbundlesProp, "rbundles");
        groupObjsProp = serializedObject.FindProperty("groupObjs");
        defultTypeProp = serializedObject.FindProperty("defultType");
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
        EditorGUI.BeginChangeCheck();
        defultTypeProp.enumValueIndex = GUILayout.Toolbar(defultTypeProp.enumValueIndex, option,EditorStyles.toolbarButton);
    }
    protected virtual void DrawRuntimeItems()
    {
        switch ((UILoadType)defultTypeProp.enumValueIndex)
        {
            case UILoadType.LocalPrefab:
                ReorderableListGUI.Title("预制体动态加载资源信息列表");
                Rotorz.ReorderableList.ReorderableListGUI.ListField(prefabsAdapt);
                break;
            case UILoadType.LocalBundle:
                ReorderableListGUI.Title("本地动态加载资源信息列表");
                Rotorz.ReorderableList.ReorderableListGUI.ListField(bundlesAdapt);
                break;
            case UILoadType.RemoteBundle:
                ReorderableListGUI.Title("远端动态加载资源信息列表");
                Rotorz.ReorderableList.ReorderableListGUI.ListField(rbundlesAdapt);
                break;
            default:
                break;
        }

    }

    private void DrawToolButtons()
    {
        var btnStyle = EditorStyles.miniButton;
        switch ((UILoadType)defultTypeProp.enumValueIndex)
        {
            case UILoadType.LocalPrefab:
                using (var hor = new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button(new GUIContent("%", "移除重复"), btnStyle))
                    {
                        RemoveBundlesDouble(prefabsProp);
                    }
                    if (GUILayout.Button(new GUIContent("！", "排序"), btnStyle))
                    {
                        SortAllBundles(prefabsProp);
                    }
                    if (GUILayout.Button(new GUIContent("o", "批量加载"), btnStyle))
                    {
                        GroupLoadPrefabs(prefabsProp);
                    }
                    if (GUILayout.Button(new GUIContent("c", "批量关闭"), btnStyle))
                    {
                        CloseAllCreated();
                    }
                }
                break;
            case UILoadType.LocalBundle:
                using (var hor = new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button(new GUIContent("%", "移除重复"), btnStyle))
                    {
                        RemoveBundlesDouble(bundlesProp);
                    }
                    if (GUILayout.Button(new GUIContent("*", "快速更新"), btnStyle))
                    {
                        QuickUpdateBundles();
                    }
                    if (GUILayout.Button(new GUIContent("!", "排序"), btnStyle))
                    {
                        SortAllBundles(bundlesProp);
                    }
                    if (GUILayout.Button(new GUIContent("o", "批量加载"), btnStyle))
                    {
                        GroupLoadPrefabs(bundlesProp);
                    }
                    if (GUILayout.Button(new GUIContent("c", "批量关闭"), btnStyle))
                    {
                        CloseAllCreated();
                    }
                }
                break;
            case UILoadType.RemoteBundle:
                using (var hor = new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button(new GUIContent("%", "移除重复"), btnStyle))
                    {
                        RemoveBundlesDouble(rbundlesProp);
                    }
                    if (GUILayout.Button(new GUIContent("!", "排序"), btnStyle))
                    {
                        SortAllBundles(rbundlesProp);
                    }
                    if (GUILayout.Button(new GUIContent("o", "批量加载"), btnStyle))
                    {
                        GroupPreviewFromBundles();
                    }
                }
                break;
            default:
                break;
        }

        switch ((UILoadType)defultTypeProp.enumValueIndex)
        {
            case UILoadType.RemoteBundle:
                EditorGUILayout.PropertyField(assetUrlProp);
                EditorGUILayout.PropertyField(menuProp);
                break;
            default:
                break;
        }

    }

    private void GroupPreviewFromBundles()
    {
        BundlePreview.Data data = new BundlePreview.Data();
        var assetUrl = rbundlesProp.serializedObject.FindProperty("assetUrl");
        var menu = rbundlesProp.serializedObject.FindProperty("menu");
        Debug.Log(assetUrl);
        data.assetUrl = assetUrl.stringValue;
        data.menu = menu.stringValue;
        for (int i = 0; i < rbundlesProp.arraySize; i++)
        {
            var itemProp = rbundlesProp.GetArrayElementAtIndex(i);
            var assetProp = itemProp.FindPropertyRelative("assetName");
            var bundleProp = itemProp.FindPropertyRelative("bundleName");
            var resetProp = itemProp.FindPropertyRelative("reset");

            var bdinfo = new BundleInfo();
            bdinfo.assetName = assetProp.stringValue;
            bdinfo.bundleName = bundleProp.stringValue;
            bdinfo.reset = resetProp.boolValue;
            data.rbundles.Add(bdinfo);
        }
        var path = AssetDatabase.GUIDToAssetPath("018159907ea26db409399b839477ad27");
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene(path);
        GameObject holder = new GameObject("holder");
        BundlePreview preview = holder.AddComponent<BundlePreview>();
        preview.data = data;
        EditorApplication.ExecuteMenuItem("Edit/Play");
    }

    private void GroupLoadPrefabs(SerializedProperty proprety)
    {
        for (int i = 0; i < proprety.arraySize; i++)
        {
            var itemProp = proprety.GetArrayElementAtIndex(i);
            var prefabProp = itemProp.FindPropertyRelative("prefab");
            var resetProp = itemProp.FindPropertyRelative("reset");
            GameObject instence = PrefabUtility.InstantiatePrefab(prefabProp.objectReferenceValue) as GameObject;
            if (target is UIGroup)
            {
                instence.transform.SetParent((target as UIGroup).transform, resetProp.boolValue);
            }
            if (created == null) created = new List<GameObject>();
            created.Add(instence);
        }
    }

    private void QuickUpdateBundles()
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
    private void RemoveBundlesDouble(SerializedProperty property)
    {
        compair: List<string> temp = new List<string>();

        for (int i = 0; i < property.arraySize; i++)
        {
            var itemProp = property.GetArrayElementAtIndex(i);
            var assetNameProp = itemProp.FindPropertyRelative("assetName");
            if (!temp.Contains(assetNameProp.stringValue))
            {
                temp.Add(assetNameProp.stringValue);
            }
            else
            {
                property.DeleteArrayElementAtIndex(i);
                goto compair;
            }
        }
    }

    private void CloseAllCreated()
    {
        if (created == null)
        {
            return;
        }
        TrySaveAllPrefabs();
        for (int i = 0; i < created.Count; i++)
        {
            if (created[i] != null)
            {
                DestroyImmediate(created[i]);
            }
        }
        created.Clear();
        created = null;
    }
    private void SortAllBundles(SerializedProperty property)
    {
        for (int i = 0; i < property.arraySize; i++)
        {
            for (int j = i; j < property.arraySize - i - 1; j++)
            {
                var itemj = property.GetArrayElementAtIndex(j).FindPropertyRelative("assetName");
                var itemj1 = property.GetArrayElementAtIndex(j + 1).FindPropertyRelative("assetName");
                if (string.Compare(itemj.stringValue, itemj1.stringValue) > 0)
                {
                    property.MoveArrayElement(j, j + 1);
                }
            }
        }
    }

    private void TrySaveAllPrefabs()
    {
        if (created == null)
        {
            return;
        }

        foreach (var item in created)
        {
            var prefab = PrefabUtility.GetPrefabParent(item);
            if (prefab != null)
            {
                var root = PrefabUtility.FindPrefabRoot((GameObject)prefab);
                if (root != null)
                {
                    PrefabUtility.ReplacePrefab(item, root, ReplacePrefabOptions.ConnectToPrefab);
                }
            }
        }
    }
}
