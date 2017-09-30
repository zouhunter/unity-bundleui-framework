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
namespace BundleUISystem {
    public partial class UIData {

        #region simple operators
        public static implicit operator Hashtable(UIData data)
        {
            return data.tableContent;
        }
        public static implicit operator UIData(Hashtable table)
        {
            return UIData.Allocate(Type.Table, table);
        }
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
        public static implicit operator string(UIData d)
        {
            return (d == null) ? null : d.str;
        }
        public static implicit operator UIData(string[] s)
        {
            return UIData.Allocate(Type.OBJECT,s);
        }

        #endregion operators

        #region Unity Struct Operate
        /*
         * Vector2
         */
        public static implicit operator Vector2(UIData obj)
        {
            return (Vector2)obj.data;
        }

        public static implicit operator UIData(Vector2 v)
        {
            return UIData.Allocate(Type.OBJECT, v); ;
        }

        /*
         * Vector3
         */
        public static implicit operator UIData(Vector3 v)
        {
            return UIData.Allocate(Type.OBJECT, v);
        }

        public static implicit operator Vector3(UIData obj)
        {
            return (Vector3)obj.data;
        }

        /*
         * Vector4
         */
        public static implicit operator UIData(Vector4 v)
        {
            return UIData.Allocate(Type.OBJECT, v);
        }

        public static implicit operator Vector4(UIData obj)
        {
            return (Vector4)obj.data;
        }

        /*
         * Matrix4x4
         */
        public static implicit operator UIData(Matrix4x4 m)
        {
            return UIData.Allocate(Type.OBJECT, m);
        }

        public static implicit operator Matrix4x4(UIData obj)
        {
            return (Matrix4x4)obj.data;
        }

        /*
         * Quaternion
         */
        public static implicit operator UIData(Quaternion q)
        {
            return UIData.Allocate(Type.OBJECT, q);
        }

        public static implicit operator Quaternion(UIData obj)
        {
            return (Quaternion)obj.data;
        }

        /*
         * Color
         */
        public static implicit operator UIData(Color c)
        {
            return UIData.Allocate(Type.OBJECT, c);
        }

        public static implicit operator Color(UIData obj)
        {
            return (Color)obj.data;
        }

        /*
         * Layer Mask
         */
        public static implicit operator UIData(LayerMask l)
        {
            return UIData.Allocate(Type.OBJECT, l);
        }

        public static implicit operator LayerMask(UIData obj)
        {
            return (LayerMask)obj.data;
        }

        public static implicit operator UIData(Rect r)
        {
            return UIData.Allocate(Type.OBJECT, r);
        }

        public static implicit operator Rect(UIData obj)
        {
            return (Rect)obj.data;
        }

        public static implicit operator UIData(RectOffset r)
        {
            return UIData.Allocate(Type.OBJECT, r);
        }

        public static implicit operator RectOffset(UIData obj)
        {
            return (RectOffset)obj.data;
        }

        public static implicit operator AnimationCurve(UIData obj)
        {
            return (AnimationCurve)obj.data;
        }

        public static implicit operator UIData(AnimationCurve a)
        {
            return UIData.Allocate(Type.OBJECT, a);
        }

        public static implicit operator Keyframe(UIData obj)
        {
            return (Keyframe)obj.data;
        }

        public static implicit operator UIData(Keyframe k)
        {
            return UIData.Allocate(Type.OBJECT, k);
        }
        #endregion
    }
}