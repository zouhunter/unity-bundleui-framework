using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using BundleUISystem.Internal;
using BundleUISystem;
using System;
using Object = UnityEngine.Object;

[CustomPropertyDrawer(typeof(PrefabInfo))]
public class PrefabInfoDrawer :ItemInfoBaseDrawer
{
    SerializedProperty prefabProp;//prefab
    protected const int ht = 4;

    protected override void InitPropertys(SerializedProperty property)
    {
        base.InitPropertys(property);
        prefabProp = property.FindPropertyRelative("prefab");
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
        var rect = new Rect(opendRect.x, opendRect.y, opendRect.width, singleHeight);

        EditorGUI.PropertyField(rect, typeProp, new GUIContent("type"));

        switch ((ItemInfoBase.Type)typeProp.enumValueIndex)
        {
            case ItemInfoBase.Type.Name:
                break;
            case ItemInfoBase.Type.Button:
                rect.y += singleHeight;
                EditorGUI.PropertyField(rect, buttonProp, new GUIContent("Button"));
                break;
            case ItemInfoBase.Type.Toggle:
                rect.y += singleHeight;
                EditorGUI.PropertyField(rect, toggleProp, new GUIContent("Toggle"));
                break;
            case ItemInfoBase.Type.Enable:
                break;
            default:
                break;
        }

        rect.y += singleHeight;
        EditorGUI.PropertyField(rect, parentLayerProp, new GUIContent("parentLayer"));

        rect.y += singleHeight;
        EditorGUI.PropertyField(rect, resetProp, new GUIContent("reset"));
    }

    protected override void DrawObjectField(Rect rect)
    {
        if (prefabProp.objectReferenceValue != null)
        {
            if (GUI.Button(rect, "", EditorStyles.objectFieldMiniThumb))
            {
                EditorGUIUtility.PingObject(prefabProp.objectReferenceValue);
            }
        }
        else
        {
            prefabProp.objectReferenceValue = EditorGUI.ObjectField(rect, null, typeof(GameObject), false);
        }
    }


    protected override void OnDragPerformGameObject(GameObject go)
    {
        prefabProp.objectReferenceValue = go;
        assetNameProp.stringValue = go.name;
    }

    protected override void ResetBuildInfoOnOpen()
    {
        base.ResetBuildInfoOnOpen();

        if (prefabProp.objectReferenceValue != null)
        {
            assetNameProp.stringValue = prefabProp.objectReferenceValue.name;
        }
    }

    protected override GameObject GetPrefabItem()
    {
        GameObject gopfb = prefabProp.objectReferenceValue as GameObject;
        return gopfb;
    }

    protected override void HideItemIfInstenced()
    {
        var obj = EditorUtility.InstanceIDToObject(instanceIDProp.intValue);

        if (obj != null){
            Object.DestroyImmediate(obj);
        }

        instanceIDProp.intValue = 0;
    }
}
