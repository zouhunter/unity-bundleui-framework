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

namespace BundleUISystem {
    public partial class UIData
    {
        protected object data;
        private Hashtable tableContent = new Hashtable();
        public enum Type { STRING, INT, FLOAT, OBJECT, ARRAY,Table, BOOL }
        public Type type = Type.OBJECT;
        public bool isContainer { get { return (type == Type.OBJECT)||(type==Type.ARRAY)||type==Type.Table; } }
        public int Count
        {
            get
            {
                if (tableContent == null)
                    return -1;
                return tableContent.Count;
            }
        }

        private int n;
        private float f;
        private bool b;
        private string str;

        public bool B { get { return b; } set { type = Type.BOOL; Data = b = value; } }
        public float F { get { return f; } set { type = Type.FLOAT; Data = f = value; } }
        public int Num { get { return n; } set { type = Type.INT; Data = n = value; } }
        public string Str { get { return str; }set { type = Type.STRING;Data = str = value; } }
        public object Data { get { return data; }set { data = value; } }
        public Hashtable Table { get { return tableContent; }set { type = Type.Table;data = tableContent = value; } }
        #region constractors
        public static UIData Allocate(Type t)
        {
            var uidata = poolObject.Allocate();
            uidata.type = t;
            return uidata;
        }

        public static UIData Allocate(Type t,object data) {
            var uidata = poolObject.Allocate();
            uidata.type = t;
            switch (t)
            {
                case Type.STRING:
                    uidata.Str = (string)data;
                    break;
                case Type.INT:
                    uidata.Num = (int)data;
                    break;
                case Type.FLOAT:
                    uidata.F = (float)data;
                    break;
                case Type.OBJECT:
                    uidata.data = data;
                    break;
                case Type.BOOL:
                    uidata.B = (bool)data;
                    break;
                case Type.ARRAY:
                    var objarray = (object[])data;
                    for (int i = 0; i < objarray.Length; i++){
                        uidata.tableContent[i] = objarray[i];
                    }
                    uidata.data = data;
                    break;
                case Type.Table:
                    uidata.data = uidata.tableContent = (Hashtable)data;
                    break;
                default:
                    break;
            }
            return uidata;
        }

        public void Clear()
        {
            this.data = default(object);
            n = default(int);
            f = default(float);
            b = default(bool);
            str = default(string);
            tableContent.Clear();
        }

        static ObjectPool<UIData> poolObject = new ObjectPool<UIData>(1);

        public void Release()
        {
            Clear();
            poolObject.Release(this);
        }

        #endregion

        public object this[object index]
        {
            get
            {
                return tableContent[index];
            }
            set
            {
                tableContent[index] = value;
            }
        }
    }
}