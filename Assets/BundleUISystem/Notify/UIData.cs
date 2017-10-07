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
using System;

namespace BundleUISystem
{
    public class UIData
    {
        protected object _data;
        private Dictionary<object, UIData> tableContent = new Dictionary<object, UIData>();
        public enum Type { NULL, STRING, INT, FLOAT, OBJECT, DIC, BOOL }
        public Type type = Type.NULL;

        public bool b { get { if (type == Type.BOOL) return (bool)data; return false; } set { type = Type.BOOL; data = value; } }
        public float f { get { if (type == Type.FLOAT) return (float)data; return 0; } set { type = Type.FLOAT; data = value; } }
        public int n { get { if (type == Type.INT) return (int)data; return 0; } set { type = Type.INT; data = value; } }
        public string str { get { if (type == Type.STRING) return (string)data; return ""; } set { type = Type.STRING; data = value; } }
        public object data { get { return _data; } set { _data = value; } }

        #region constractors
        private UIData() { type = Type.OBJECT;  }
        public static UIData Allocate()
        {
            var uidata = poolObject.Allocate();
            uidata.type = Type.NULL;
            if(poolObject.Length > 50){
                poolObject.Reset();
            }
            return uidata;
        }
        public static UIData Allocate(Type t)
        {
            var uidata = Allocate();
            uidata.type = t;
            return uidata;
        }
        public static UIData Allocate<T>(T data)
        {
            if (data is string)
            {
                return Allocate(Type.STRING, data);
            }
            else if (data is int)
            {
                return Allocate(Type.INT, data);
            }
            else if (data is float)
            {
                return Allocate(Type.FLOAT, data);
            }
            else if (data is bool)
            {
                return Allocate(Type.BOOL, data);
            }
            else if (data is Dictionary<object, UIData>)
            {
                return Allocate(Type.DIC, data);
            }
            else
            {
                return Allocate(Type.OBJECT, data);
            }
        }

        public static UIData Allocate(Type t, object data)
        {
            var uidata = Allocate();
            uidata.type = t;
            switch (t)
            {
                case Type.STRING:
                    uidata.str = (string)data;
                    break;
                case Type.INT:
                    uidata.n = (int)data;
                    break;
                case Type.FLOAT:
                    uidata.f = (float)data;
                    break;
                case Type.OBJECT:
                    uidata._data = data;
                    break;
                case Type.BOOL:
                    uidata.b = (bool)data;
                    break;
                case Type.DIC:
                    uidata._data = uidata.tableContent = (Dictionary<object, UIData>)data;
                    break;
                default:
                    break;
            }
            return uidata;
        }

        static ObjectPool<UIData> poolObject = new ObjectPool<UIData>(1, () => { return new UIData(); });

        public void Release()
        {
            foreach (var item in tableContent)
            {
                var childData = item.Value;
                childData.Release();
            }
            tableContent.Clear();
            this._data = default(object);
            poolObject.Release(this);
        }

        #endregion

        #region Switch
        private static UIData emptyData = UIData.Allocate(Type.NULL);

        public UIData this[object index]
        {
            get
            {
                if (!tableContent.ContainsKey(index))
                {
                    return emptyData;
                }
                else
                {
                    return tableContent[index] as UIData;
                }
            }
            set
            {
                type = Type.DIC;
                tableContent[index] = value;
            }
        }

        public T OfType<T>()
        {
            if (data is T)
            {
                return (T)data;
            }
            return default(T);
        }

        public bool IsEmpty { get { return type == Type.NULL; } }

        #endregion

        #region simple operators
        public static implicit operator UIData(string s)
        {
            return UIData.Allocate(Type.STRING, s);
        }
        public static implicit operator UIData(int i)
        {
            return UIData.Allocate(Type.INT, i);
        }
        public static implicit operator UIData(float f)
        {
            return UIData.Allocate(Type.FLOAT, f);
        }
        public static implicit operator UIData(bool b)
        {
            return UIData.Allocate(Type.BOOL, b);
        }
        #endregion operators

        public override string ToString()
        {
            return str;
        }
    }

}