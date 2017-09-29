using UnityEngine;

namespace BundleUISystem
{
    public partial class JSONObject
    {
        #region simple operators
        public static implicit operator JSONObject(string s)
        {
            return CreateStringObject(s);
        }
        public static implicit operator JSONObject(int i)
        {
            return Create(i);
        }
        public static implicit operator JSONObject(float f)
        {
            return Create(f);
        }
        public static implicit operator JSONObject(bool b)
        {
            return Create(b);
        }
        public static implicit operator string(JSONObject d)
        {
            return (d == null) ? null : d.str;
        }
        public static implicit operator JSONObject(string[] s)
        {
            if (s == null) return null;
            var obj = JSONObject.Create(Type.ARRAY);
            for (int i = 0; i < s.Length; i++)
            {
                obj.Add(s[i]);
            }
            return obj;
        }
        public static bool operator ==(JSONObject a, object b)
        {
            if (b == null && a is JSONObject)
                return true;
            return System.Object.ReferenceEquals(a, b);
        }

        public static bool operator !=(JSONObject a, object b)
        {
            return !(a == b);
        }
        public override bool Equals(object obj)
        {
            return System.Object.ReferenceEquals(this, obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion operators

        #region Unity Struct Operate
        /*
         * Vector2
         */
        public static implicit operator Vector2(JSONObject obj)
        {
            float x = obj["x"] ? obj["x"].f : 0;
            float y = obj["y"] ? obj["y"].f : 0;
            return new Vector2(x, y);
        }

        public static implicit operator JSONObject(Vector2 v)
        {
            JSONObject vdata = JSONObject.obj;
            if (v.x != 0) vdata.AddField("x", v.x);
            if (v.y != 0) vdata.AddField("y", v.y);
            return vdata;
        }

        /*
         * Vector3
         */
        public static implicit operator JSONObject(Vector3 v)
        {
            JSONObject vdata = JSONObject.obj;
            if (v.x != 0) vdata.AddField("x", v.x);
            if (v.y != 0) vdata.AddField("y", v.y);
            if (v.z != 0) vdata.AddField("z", v.z);
            return vdata;
        }

        public static implicit operator Vector3(JSONObject obj)
        {
            float x = obj["x"] ? obj["x"].f : 0;
            float y = obj["y"] ? obj["y"].f : 0;
            float z = obj["z"] ? obj["z"].f : 0;
            return new Vector3(x, y, z);
        }

        /*
         * Vector4
         */
        public static implicit operator JSONObject(Vector4 v)
        {
            JSONObject vdata = JSONObject.obj;
            if (v.x != 0) vdata.AddField("x", v.x);
            if (v.y != 0) vdata.AddField("y", v.y);
            if (v.z != 0) vdata.AddField("z", v.z);
            if (v.w != 0) vdata.AddField("w", v.w);
            return vdata;
        }

        public static implicit operator Vector4(JSONObject obj)
        {
            float x = obj["x"] ? obj["x"].f : 0;
            float y = obj["y"] ? obj["y"].f : 0;
            float z = obj["z"] ? obj["z"].f : 0;
            float w = obj["w"] ? obj["w"].f : 0;
            return new Vector4(x, y, z, w);
        }

        /*
         * Matrix4x4
         */
        public static implicit operator JSONObject(Matrix4x4 m)
        {
            JSONObject mdata = JSONObject.obj;
            if (m.m00 != 0) mdata.AddField("m00", m.m00);
            if (m.m01 != 0) mdata.AddField("m01", m.m01);
            if (m.m02 != 0) mdata.AddField("m02", m.m02);
            if (m.m03 != 0) mdata.AddField("m03", m.m03);
            if (m.m10 != 0) mdata.AddField("m10", m.m10);
            if (m.m11 != 0) mdata.AddField("m11", m.m11);
            if (m.m12 != 0) mdata.AddField("m12", m.m12);
            if (m.m13 != 0) mdata.AddField("m13", m.m13);
            if (m.m20 != 0) mdata.AddField("m20", m.m20);
            if (m.m21 != 0) mdata.AddField("m21", m.m21);
            if (m.m22 != 0) mdata.AddField("m22", m.m22);
            if (m.m23 != 0) mdata.AddField("m23", m.m23);
            if (m.m30 != 0) mdata.AddField("m30", m.m30);
            if (m.m31 != 0) mdata.AddField("m31", m.m31);
            if (m.m32 != 0) mdata.AddField("m32", m.m32);
            if (m.m33 != 0) mdata.AddField("m33", m.m33);
            return mdata;
        }

        public static implicit operator Matrix4x4(JSONObject obj)
        {
            Matrix4x4 result = new Matrix4x4();
            if (obj["m00"]) result.m00 = obj["m00"].f;
            if (obj["m01"]) result.m01 = obj["m01"].f;
            if (obj["m02"]) result.m02 = obj["m02"].f;
            if (obj["m03"]) result.m03 = obj["m03"].f;
            if (obj["m10"]) result.m10 = obj["m10"].f;
            if (obj["m11"]) result.m11 = obj["m11"].f;
            if (obj["m12"]) result.m12 = obj["m12"].f;
            if (obj["m13"]) result.m13 = obj["m13"].f;
            if (obj["m20"]) result.m20 = obj["m20"].f;
            if (obj["m21"]) result.m21 = obj["m21"].f;
            if (obj["m22"]) result.m22 = obj["m22"].f;
            if (obj["m23"]) result.m23 = obj["m23"].f;
            if (obj["m30"]) result.m30 = obj["m30"].f;
            if (obj["m31"]) result.m31 = obj["m31"].f;
            if (obj["m32"]) result.m32 = obj["m32"].f;
            if (obj["m33"]) result.m33 = obj["m33"].f;
            return result;
        }

        /*
         * Quaternion
         */
        public static implicit operator JSONObject(Quaternion q)
        {
            JSONObject qdata = JSONObject.obj;
            if (q.w != 0) qdata.AddField("w", q.w);
            if (q.x != 0) qdata.AddField("x", q.x);
            if (q.y != 0) qdata.AddField("y", q.y);
            if (q.z != 0) qdata.AddField("z", q.z);
            return qdata;
        }

        public static implicit operator Quaternion(JSONObject obj)
        {
            float x = obj["x"] ? obj["x"].f : 0;
            float y = obj["y"] ? obj["y"].f : 0;
            float z = obj["z"] ? obj["z"].f : 0;
            float w = obj["w"] ? obj["w"].f : 0;
            return new Quaternion(x, y, z, w);
        }

        /*
         * Color
         */
        public static implicit operator JSONObject(Color c)
        {
            JSONObject cdata = JSONObject.obj;
            if (c.r != 0) cdata.AddField("r", c.r);
            if (c.g != 0) cdata.AddField("g", c.g);
            if (c.b != 0) cdata.AddField("b", c.b);
            if (c.a != 0) cdata.AddField("a", c.a);
            return cdata;
        }

        public static implicit operator Color(JSONObject obj)
        {
            Color c = new Color();
            for (int i = 0; i < obj.Count; i++)
            {
                switch (obj.keys[i])
                {
                    case "r":
                        c.r = obj[i].f;
                        break;
                    case "g":
                        c.g = obj[i].f;
                        break;
                    case "b":
                        c.b = obj[i].f;
                        break;
                    case "a":
                        c.a = obj[i].f;
                        break;
                }
            }
            return c;
        }

        /*
         * Layer Mask
         */
        public static implicit operator JSONObject(LayerMask l)
        {
            JSONObject result = JSONObject.obj;
            result.AddField("value", l.value);
            return result;
        }

        public static implicit operator LayerMask(JSONObject obj)
        {
            LayerMask l = new LayerMask {value = (int) obj["value"].n};
            return l;
        }

        public static implicit operator JSONObject(Rect r)
        {
            JSONObject result = JSONObject.obj;
            if (r.x != 0) result.AddField("x", r.x);
            if (r.y != 0) result.AddField("y", r.y);
            if (r.height != 0) result.AddField("height", r.height);
            if (r.width != 0) result.AddField("width", r.width);
            return result;
        }

        public static implicit operator Rect(JSONObject obj)
        {
            Rect r = new Rect();
            for (int i = 0; i < obj.Count; i++)
            {
                switch (obj.keys[i])
                {
                    case "x":
                        r.x = obj[i].f;
                        break;
                    case "y":
                        r.y = obj[i].f;
                        break;
                    case "height":
                        r.height = obj[i].f;
                        break;
                    case "width":
                        r.width = obj[i].f;
                        break;
                }
            }
            return r;
        }

        public static implicit operator JSONObject(RectOffset r)
        {
            JSONObject result = JSONObject.obj;
            if (r.bottom != 0) result.AddField("bottom", r.bottom);
            if (r.left != 0) result.AddField("left", r.left);
            if (r.right != 0) result.AddField("right", r.right);
            if (r.top != 0) result.AddField("top", r.top);
            return result;
        }

        public static implicit operator RectOffset(JSONObject obj)
        {
            RectOffset r = new RectOffset();
            for (int i = 0; i < obj.Count; i++)
            {
                switch (obj.keys[i])
                {
                    case "bottom":
                        r.bottom = (int) obj[i].n;
                        break;
                    case "left":
                        r.left = (int) obj[i].n;
                        break;
                    case "right":
                        r.right = (int) obj[i].n;
                        break;
                    case "top":
                        r.top = (int) obj[i].n;
                        break;
                }
            }
            return r;
        }

        public static implicit operator AnimationCurve(JSONObject obj)
        {
            AnimationCurve a = new AnimationCurve();
            if (obj.HasField("keys"))
            {
                JSONObject keys = obj.GetField("keys");
                for (int i = 0; i < keys.list.Count; i++)
                {
                    a.AddKey(keys[i]);
                }
            }
            if (obj.HasField("preWrapMode"))
                a.preWrapMode = (WrapMode) ((int) obj.GetField("preWrapMode").n);
            if (obj.HasField("postWrapMode"))
                a.postWrapMode = (WrapMode) ((int) obj.GetField("postWrapMode").n);
            return a;
        }

        public static implicit operator JSONObject(AnimationCurve a)
        {
            JSONObject result = JSONObject.obj;
            result.AddField("preWrapMode", a.preWrapMode.ToString());
            result.AddField("postWrapMode", a.postWrapMode.ToString());
            if (a.keys.Length > 0)
            {
                JSONObject keysJSON = JSONObject.Create();
                for (int i = 0; i < a.keys.Length; i++)
                {
                    keysJSON.Add(a.keys[i]);
                }
                result.AddField("keys", keysJSON);
            }
            return result;
        }

        public static implicit operator Keyframe(JSONObject obj)
        {
            Keyframe k = new Keyframe(obj.HasField("time") ? obj.GetField("time").n : 0,
                obj.HasField("value") ? obj.GetField("value").n : 0);
            if (obj.HasField("inTangent")) k.inTangent = obj.GetField("inTangent").n;
            if (obj.HasField("outTangent")) k.outTangent = obj.GetField("outTangent").n;
            if (obj.HasField("tangentMode")) k.tangentMode = (int) obj.GetField("tangentMode").n;

            return k;
        }

        public static implicit operator JSONObject(Keyframe k)
        {
            JSONObject result = JSONObject.obj;
            if (k.inTangent != 0) result.AddField("inTangent", k.inTangent);
            if (k.outTangent != 0) result.AddField("outTangent", k.outTangent);
            if (k.tangentMode != 0) result.AddField("tangentMode", k.tangentMode);
            if (k.time != 0) result.AddField("time", k.time);
            if (k.value != 0) result.AddField("value", k.value);
            return result;
        }
        #endregion
    }
}