using BundleUISystem;
using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class NewEditorTest {

    [Test]
    public void EditorTest()
    {
        JSONObject obj = JSONObject.Create(JSONObject.Type.OBJECT);
        obj.AddField("哈哈",JSONObject.Create(7));
        Debug.Log(obj);
        Debug.Log(obj["哈哈"]);
    }

}
