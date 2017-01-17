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
        var typeProp = property.FindPropertyRelative("type");    ;
        var parentProp = property.FindPropertyRelative("parent");
        var boolProp = property.FindPropertyRelative("isWorld");
        var buttonProp = property.FindPropertyRelative("button");
        var toggleProp = property.FindPropertyRelative("toggle");
        var messageProp = property.FindPropertyRelative("message");

        float height = EditorGUIUtility.singleLineHeight;

        Rect rect = new Rect(position.xMin, position.yMin, position.width, height);
        addArrayTools(rect, property);

        rect.width -= widthBt * 8;
        rect.width  /= 1.5f;
        if (GUI.Button(rect,assetName.stringValue))
        {
            property.isExpanded = !property.isExpanded;
        }
        rect.width =  widthBt * 4;
        rect.x = position.xMax - widthBt * 8;

        prefab.objectReferenceValue = EditorGUI.ObjectField(rect, new GUIContent(""),prefab.objectReferenceValue,typeof(GameObject),false);

        rect = new Rect(position.xMin, position.yMin, position.width, height);
        if (!property.isExpanded)
        {
            return;
        }

        rect = new Rect(position.xMin, position.yMin + height, position.width, height);
        EditorGUI.PropertyField(rect, assetName, new GUIContent("name"));

        rect = new Rect(position.xMin, position.yMin + 2 * height, position.width, height);
        EditorGUI.PropertyField(rect, bundleName, new GUIContent("bundle"));

        rect = new Rect(position.xMin, position.yMin + 3 * height, position.width, height);
        EditorGUI.PropertyField(rect, typeProp, new GUIContent("type"));


        rect = new Rect(position.xMin, position.yMin + 4 * height, position.width, height);
        EditorGUI.PropertyField(rect, parentProp, new GUIContent("parent"));

        rect = new Rect(position.xMin, position.yMin + 5 * height, position.width, height);
        EditorGUI.PropertyField(rect, boolProp, new GUIContent("isWorld"));

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
            default:
                break;
        }
       
    }

    void addArrayTools(Rect position, SerializedProperty property)
    {
        string path = property.propertyPath;

        int arrayInd = path.LastIndexOf(".Array");
        bool bIsArray = arrayInd >= 0;

        if (bIsArray)
        {
            SerializedObject so = property.serializedObject;
            string arrayPath = path.Substring(0, arrayInd);
            SerializedProperty arrayProp = so.FindProperty(arrayPath);

            //Next we need to grab the index from the path string
            int indStart = path.IndexOf("[") + 1;
            int indEnd = path.IndexOf("]");

            string indString = path.Substring(indStart, indEnd - indStart);

            int myIndex = int.Parse(indString);
            Rect rcButton = position;
            rcButton.height = EditorGUIUtility.singleLineHeight;
            rcButton.x = position.xMax - widthBt * 4;
            rcButton.width = widthBt;

            if (GUI.Button(rcButton, "o"))
            {
                var prefab = property.FindPropertyRelative("prefab");
                var item =  arrayProp.GetArrayElementAtIndex(myIndex);
                var asset = item.FindPropertyRelative("assetName");
                var bundle = item.FindPropertyRelative("bundleName");
                var parent = item.FindPropertyRelative("parent");
                var isworld = item.FindPropertyRelative("isWorld");

                string[] paths = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(bundle.stringValue,asset.stringValue);
                GameObject gopfb = AssetDatabase.LoadAssetAtPath<GameObject>(paths[0]);
                prefab.objectReferenceValue = gopfb;
                GameObject go = PrefabUtility.InstantiatePrefab(gopfb) as GameObject;
                go.transform.SetParent((Transform)parent.objectReferenceValue, isworld.boolValue);
            }

            rcButton.x += 2 * widthBt;
            if (GUI.Button(rcButton, "-"))
            {
                arrayProp.DeleteArrayElementAtIndex(myIndex);
                so.ApplyModifiedProperties();
            }

            rcButton.x += widthBt;
            if (GUI.Button(rcButton, "+"))
            {
                arrayProp.InsertArrayElementAtIndex(myIndex);
                so.ApplyModifiedProperties();
            }
        }
    }

}
