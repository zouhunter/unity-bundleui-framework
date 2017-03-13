using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(RunTimeBundleInfo))]
public class RuntimeABInfoDrawer : PropertyDrawer
{
    const float widthBt = 20;
    const int ht = 7;
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!property.isExpanded) return EditorGUIUtility.singleLineHeight;
        var typeProp = property.FindPropertyRelative("type");
        var buttonListProp = property.FindPropertyRelative("button");
        var toggleListProp = property.FindPropertyRelative("toggle");
        var messageListProp = property.FindPropertyRelative("message");
        switch (typeProp.enumValueIndex)
        {
            case 0:
                return ht * EditorGUIUtility.singleLineHeight + EditorGUI.GetPropertyHeight(buttonListProp);
            case 1:
                return ht * EditorGUIUtility.singleLineHeight + EditorGUI.GetPropertyHeight(toggleListProp);
            case 2:
            case 3:
                return ht * EditorGUIUtility.singleLineHeight + EditorGUI.GetPropertyHeight(messageListProp);
            default:
                return ht * EditorGUIUtility.singleLineHeight;
        }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var prefab = property.FindPropertyRelative("prefab");
        var assetName = property.FindPropertyRelative("assetName");
        var bundleName = property.FindPropertyRelative("bundleName");
        var typeProp = property.FindPropertyRelative("type"); ;
        var parentProp = property.FindPropertyRelative("parent");
        var boolProp = property.FindPropertyRelative("reset");
        var buttonProp = property.FindPropertyRelative("button");
        var toggleProp = property.FindPropertyRelative("toggle");
        var messageProp = property.FindPropertyRelative("message");
        var instenceProp = property.FindPropertyRelative("instence");
        float height = EditorGUIUtility.singleLineHeight;

        Rect rect = new Rect(position.xMin, position.yMin, position.width, height);

        rect.width -= widthBt * 8;
        rect.width /= 1.5f;
        if (GUI.Button(rect, assetName.stringValue))
        {
            property.isExpanded = !property.isExpanded;
            if (instenceProp.objectReferenceValue != null)
            {
                Object.DestroyImmediate(instenceProp.objectReferenceValue);
            }
            if (property.isExpanded)
            {
                string[] paths = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(bundleName.stringValue, assetName.stringValue);
                if (paths != null && paths.Length > 0)
                {
                    GameObject gopfb = AssetDatabase.LoadAssetAtPath<GameObject>(paths[0]);
                    prefab.objectReferenceValue = gopfb;
                    GameObject go = PrefabUtility.InstantiatePrefab(gopfb) as GameObject;
                    bool isworld = parentProp.objectReferenceValue == null ? true : !((Transform)parentProp.objectReferenceValue).GetComponent<RectTransform>();
                    go.transform.SetParent((Transform)parentProp.objectReferenceValue, isworld);
                    if (boolProp.boolValue)
                    {
                        go.transform.position = Vector3.zero;
                        go.transform.localRotation = Quaternion.identity;
                    }
                    instenceProp.objectReferenceValue = go;
                }
            }
        }
        rect.width = widthBt * 7;
        rect.x = position.xMax - widthBt * 8;

        prefab.objectReferenceValue = EditorGUI.ObjectField(rect, new GUIContent(""), prefab.objectReferenceValue, typeof(GameObject), false);

        rect = new Rect(position.xMin, position.yMin, position.width, height);
        if (!property.isExpanded)
        {
            return;
        }
        EditorGUI.BeginDisabledGroup(true);
        rect = new Rect(position.xMin, position.yMin + height, position.width, height);
        EditorGUI.PropertyField(rect, assetName, new GUIContent("name"));

        rect = new Rect(position.xMin, position.yMin + 2 * height, position.width, height);
        EditorGUI.PropertyField(rect, bundleName, new GUIContent("bundle"));
        EditorGUI.EndDisabledGroup();

        rect = new Rect(position.xMin, position.yMin + 3 * height, position.width, height);
        EditorGUI.PropertyField(rect, typeProp, new GUIContent("type"));


        rect = new Rect(position.xMin, position.yMin + 4 * height, position.width, height);
        EditorGUI.PropertyField(rect, parentProp, new GUIContent("parent"));

        rect = new Rect(position.xMin, position.yMin + 5 * height, position.width, height);
        EditorGUI.PropertyField(rect, boolProp, new GUIContent("reset"));

        rect = new Rect(position.xMin, position.yMin + 6 * height, position.width, height);
        switch (typeProp.enumValueIndex)
        {
            case 0:
                EditorGUI.PropertyField(rect, buttonProp, new GUIContent("Button"));
                break;
            case 1:
                EditorGUI.PropertyField(rect, toggleProp, new GUIContent("Toggle"));
                break;
            case 2:
            case 3:
                EditorGUI.PropertyField(rect, messageProp, new GUIContent("Key"));
                break;
            case 4:
                break;
            default:
                break;
        }
    }
}
