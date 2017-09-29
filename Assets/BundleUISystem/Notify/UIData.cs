using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.Sprites;
using UnityEngine.Scripting;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.Assertions.Must;
using UnityEngine.Assertions.Comparers;
using System.Collections;
using System.Collections.Generic;

public partial class UIData
{
    public static Queue<UIData> releaseQueue = new Queue<UIData>();
    private Hashtable tableContent = new Hashtable();
    public object data;

    public enum Type { NULL, STRING,INT,FLOAT, OBJECT, ARRAY, BOOL, BAKED }
    public bool isContainer { get { return (type == Type.ARRAY || type == Type.OBJECT); } }
    public Type type = Type.NULL;
    public int Count
    {
        get
        {
            if (tableContent == null)
                return -1;
            return tableContent.Count;
        }
    }
    public int n;
    public float f;
    public bool b;
    public string str;
    #region constractors
    public UIData() { }
    public UIData(object data)
    {
        this.data = data;
    }
    public UIData(Type t)
    {
        type = t;
        switch (t)
        {
            case Type.NULL:
                break;
            case Type.STRING:
                break;
            case Type.INT:
                break;
            case Type.FLOAT:
                break;
            case Type.OBJECT:
                break;
            case Type.ARRAY:
                break;
            case Type.BOOL:
                break;
            case Type.BAKED:
                break;
            default:
                break;
        }
    }
    #endregion
    public UIData this[int index]
    {
        get
        {
            return null;
        }
        set
        {
            
        }
    }
    public UIData this[string index]
    {
        get
        {
           
            return null;
        }
        set
        {
            
        }
    }
}
