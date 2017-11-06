using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using BundleUISystem.Internal;
using BundleUISystem;

[CustomPropertyDrawer(typeof(UIBundleInfo))]
public class UIBundleInfoDrawer : ItemInfoBaseDrawer
{
    protected SerializedProperty goodProp;//uibundle = property.FindPropertyRelative("good");
    protected SerializedProperty guidProp;//uibundle = property.FindPropertyRelative("guid");
    protected SerializedProperty bundleNameProp;//bundle

    protected const int ht = 5;

    protected override void InitPropertys(SerializedProperty property)
    {
        base.InitPropertys(property);
        goodProp = property.FindPropertyRelative("good");
        guidProp = property.FindPropertyRelative("guid");
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

    protected override void DragAndDrapAction(Rect acceptRect)
    {
        base.DragAndDrapAction(acceptRect);
        if (Event.current.type == EventType.Repaint)
        {
            var path0 = AssetDatabase.GUIDToAssetPath(guidProp.stringValue);
            var obj0 = AssetDatabase.LoadAssetAtPath<GameObject>(path0);
            goodProp.boolValue = obj0 != null;
        }
    }

    protected override void DrawExpanded(Rect opendRect)
    {
        var rect = new Rect(opendRect.x, opendRect.y, opendRect.width, singleHeight);

        EditorGUI.BeginDisabledGroup(true);
        EditorGUI.PropertyField(rect, assetNameProp, new GUIContent("[name]"));
        rect.y += singleHeight;
        EditorGUI.PropertyField(rect, bundleNameProp, new GUIContent("[bundle]"));
        EditorGUI.EndDisabledGroup();

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
        if (goodProp.boolValue)
        {
            if (GUI.Button(acceptRect, "", EditorStyles.objectFieldMiniThumb))
            {
                var path = AssetDatabase.GUIDToAssetPath(guidProp.stringValue);
                var obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                EditorGUIUtility.PingObject(obj);
            }
        }
        else
        {
            var obj = EditorGUI.ObjectField(acceptRect, null, typeof(GameObject), false);
            if (obj != null)
            {
                var path = AssetDatabase.GetAssetPath(obj);
                guidProp.stringValue = AssetDatabase.AssetPathToGUID(path);
            }
        }

    }

    protected override void WorningIfNotRight(Rect rect)
    {
        base.WorningIfNotRight(rect);
        if (!goodProp.boolValue)
        {
            Worning(rect, assetNameProp.stringValue + " Changed！!!");
        }
    }

    protected override void OnDragPerformGameObject(GameObject go)
    {
        var path = AssetDatabase.GetAssetPath(go);
        if (!string.IsNullOrEmpty(path))
        {
            guidProp.stringValue = AssetDatabase.AssetPathToGUID(path);
            AssetImporter importer = AssetImporter.GetAtPath(path);
            assetNameProp.stringValue = go.name;
            bundleNameProp.stringValue = importer.assetBundleName;
        }
    }
    protected override void InstantiatePrefab(GameObject gopfb)
    {
        base.InstantiatePrefab(gopfb);
        var path = AssetDatabase.GetAssetPath(gopfb);
        guidProp.stringValue = AssetDatabase.AssetPathToGUID(path);
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
