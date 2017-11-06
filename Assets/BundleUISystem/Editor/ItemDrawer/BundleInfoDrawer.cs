using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using BundleUISystem.Internal;
using BundleUISystem;

[CustomPropertyDrawer(typeof(BundleInfo))]
public class BundleInfoDrawer : ItemInfoBaseDrawer
{
    const int ht = 5;
    protected SerializedProperty bundleNameProp;//bundle

    protected override void InitPropertys(SerializedProperty property)
    {
        base.InitPropertys(property);
        bundleNameProp = property.FindPropertyRelative("bundleName");
    }
    protected override float GetInfoItemHeight()
    {
        switch ((ItemInfoBase.Type)typeProp.enumValueIndex)
        {
            case ItemInfoBase.Type.Button:
                return ht * EditorGUIUtility.singleLineHeight + EditorGUI.GetPropertyHeight(buttonProp);
            case ItemInfoBase.Type.Toggle:
                return ht * EditorGUIUtility.singleLineHeight + EditorGUI.GetPropertyHeight(toggleProp);
            default:
                return ht * EditorGUIUtility.singleLineHeight;
        }
    }
    protected override void DrawExpanded(Rect opendRect)
    {
        var rect = new Rect(opendRect.xMin, opendRect.yMin, opendRect.width, singleHeight);
        EditorGUI.PropertyField(rect, assetNameProp, new GUIContent("[name]"));

        rect.y += singleHeight;
        EditorGUI.PropertyField(rect, bundleNameProp, new GUIContent("[bundle]"));

        rect.y += singleHeight;
        EditorGUI.PropertyField(rect, typeProp, new GUIContent("[type]"));

        switch ((ItemInfoBase.Type)typeProp.enumValueIndex)
        {
            case ItemInfoBase.Type.Name:
                break;
            case ItemInfoBase.Type.Button:
                rect.y += singleHeight;
                EditorGUI.PropertyField(rect, buttonProp, new GUIContent("[Btn]"));
                break;
            case ItemInfoBase.Type.Toggle:
                rect.y += singleHeight;
                EditorGUI.PropertyField(rect, toggleProp, new GUIContent("[Tog]"));
                break;
            case ItemInfoBase.Type.Enable:
                break;
            default:
                break;
        }

        rect.y += singleHeight;
        EditorGUI.PropertyField(rect, parentLayerProp, new GUIContent("[layer]"));

        //rect.y += singleHeight;
        //EditorGUI.PropertyField(rect, rematrixProp, new GUIContent("[rematrix]"));
    }

    protected override void DrawObjectField(Rect acceptRect)
    {
        if (!string.IsNullOrEmpty(assetNameProp.stringValue) && !string.IsNullOrEmpty(assetNameProp.stringValue))
        {
            if (GUI.Button(acceptRect, "", EditorStyles.objectFieldMiniThumb))
            {
                var select = EditorUtility.DisplayDialog("提示", "是否打开新场景并加载资源", "是", "否");
                if (!select)
                {
                    return;
                }
                BundlePreview.Data data = new BundlePreview.Data();
                var assetUrl = serializedObject.FindProperty("assetUrl");
                var menu = serializedObject.FindProperty("menu");
                if (string.IsNullOrEmpty(assetUrl.stringValue) || string.IsNullOrEmpty(menu.stringValue))
                {
                    return;
                }
                data.assetUrl = assetUrl.stringValue;
                data.menu = menu.stringValue;
                var bdinfo = new BundleInfo();
                bdinfo.assetName = assetNameProp.stringValue;
                bdinfo.bundleName = bundleNameProp.stringValue;
                //bdinfo.rematrix = rematrixProp.boolValue;
                data.rbundles.Add(bdinfo);
                var path = AssetDatabase.GUIDToAssetPath("018159907ea26db409399b839477ad27");
                UnityEditor.SceneManagement.EditorSceneManager.OpenScene(path);
                GameObject holder = new GameObject("holder");
                BundlePreview preview = holder.AddComponent<BundlePreview>();
                preview.data = data;
                EditorApplication.ExecuteMenuItem("Edit/Play");
            }
        }
        else
        {
            var obj = EditorGUI.ObjectField(acceptRect, null, typeof(GameObject), false);
            if(obj != null)
            {
                assetNameProp.stringValue = obj.name;
                var path = AssetDatabase.GetAssetPath(obj);
                AssetImporter import = AssetImporter.GetAtPath(path);
                if(!string.IsNullOrEmpty(import.assetBundleName)){
                    bundleNameProp.stringValue = import.assetBundleName;
                }
            }
        }
    }


    protected override void OnDragPerformGameObject(GameObject go)
    {
        var path = AssetDatabase.GetAssetPath(go);
        if (!string.IsNullOrEmpty(path))
        {
            AssetImporter importer = AssetImporter.GetAtPath(path);
            assetNameProp.stringValue = go.name;
            bundleNameProp.stringValue = importer.assetBundleName;
        }
    }
    protected override GameObject GetPrefabItem()
    {
        string[] paths = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(bundleNameProp.stringValue, assetNameProp.stringValue);
        if (paths != null && paths.Length > 0)
        {
            GameObject gopfb = AssetDatabase.LoadAssetAtPath<GameObject>(paths[0]);
            return gopfb;
        }
        return null;
    }
}
