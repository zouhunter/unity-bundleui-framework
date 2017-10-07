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
    public enum DataType { NULL, STRING, INT, FLOAT, OBJECT, DIC, BOOL }

    public class UIData
    {
        private Dictionary<object, UIData> tableContent = new Dictionary<object, UIData>();
        public DataType type = DataType.NULL;
        protected object _data;

        public bool b { get { if (type == DataType.BOOL) return (bool)data; return false; } set { type = DataType.BOOL; data = value; } }
        public float f { get { if (type == DataType.FLOAT) return (float)data; return 0; } set { type = DataType.FLOAT; data = value; } }
        public int n { get { if (type == DataType.INT) return (int)data; return 0; } set { type = DataType.INT; data = value; } }
        public string str { get { if (type == DataType.STRING) return (string)data; return ""; } set { type = DataType.STRING; data = value; } }
        public object data { get { return _data; } set { _data = value; } }

        #region constractors
        private UIData() { type = DataType.OBJECT;  }
        public static UIData Allocate()
        {
            var uidata = poolObject.Allocate();
            uidata.type = DataType.NULL;
            if(poolObject.Length > 50){
                poolObject.Reset();
            }
            return uidata;
        }
        public static UIData Allocate(DataType t)
        {
            var uidata = Allocate();
            uidata.type = t;
            return uidata;
        }
        public static UIData Allocate<T>(T data)
        {
            if (data is string)
            {
                return Allocate(DataType.STRING, data);
            }
            else if (data is int)
            {
                return Allocate(DataType.INT, data);
            }
            else if (data is float)
            {
                return Allocate(DataType.FLOAT, data);
            }
            else if (data is bool)
            {
                return Allocate(DataType.BOOL, data);
            }
            else if (data is Dictionary<object, UIData>)
            {
                return Allocate(DataType.DIC, data);
            }
            else
            {
                return Allocate(DataType.OBJECT, data);
            }
        }

        public static UIData Allocate(DataType t, object data)
        {
            var uidata = Allocate();
            uidata.type = t;
            switch (t)
            {
                case DataType.STRING:
                    uidata.str = (string)data;
                    break;
                case DataType.INT:
                    uidata.n = (int)data;
                    break;
                case DataType.FLOAT:
                    uidata.f = (float)data;
                    break;
                case DataType.OBJECT:
                    uidata._data = data;
                    break;
                case DataType.BOOL:
                    uidata.b = (bool)data;
                    break;
                case DataType.DIC:
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
        private static UIData emptyData = UIData.Allocate(DataType.NULL);

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
                type = DataType.DIC;
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

        public bool IsEmpty { get { return type == DataType.NULL; } }

        #endregion

        #region simple operators
        public static implicit operator UIData(string s)
        {
            return UIData.Allocate(DataType.STRING, s);
        }
        public static implicit operator UIData(int i)
        {
            return UIData.Allocate(DataType.INT, i);
        }
        public static implicit operator UIData(float f)
        {
            return UIData.Allocate(DataType.FLOAT, f);
        }
        public static implicit operator UIData(bool b)
        {
            return UIData.Allocate(DataType.BOOL, b);
        }
        #endregion operators

        public override string ToString()
        {
            return str;
        }
    }

}