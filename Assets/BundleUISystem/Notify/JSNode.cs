using System;
using System.Collections.Generic;

namespace BundleUISystem
{
 
    public abstract class JSNode
    {
        #region common interface

        public virtual string Value { get; set; }
        public virtual void Add(string aKey, JSNode aItem) { }
        public virtual void Add(JSNode aItem) { }
        public virtual IEnumerable<JSNode> Childs { get {yield break;} }

        #endregion common interface

        #region typecasting properties
        public virtual int AsInt
        {
            get
            {
                int v = 0;
                if (int.TryParse(Value, out v))
                    return v;
                return 0;
            }
            set
            {
                Value = value.ToString();
            }
        }
        public virtual float AsFloat
        {
            get
            {
                float v = 0.0f;
                if (float.TryParse(Value, out v))
                    return v;
                return 0.0f;
            }
            set
            {
                Value = value.ToString();
            }
        }
        public virtual double AsDouble
        {
            get
            {
                double v = 0.0;
                if (double.TryParse(Value, out v))
                    return v;
                return 0.0;
            }
            set
            {
                Value = value.ToString();
            }
        }
        public virtual bool AsBool
        {
            get
            {
                bool v = false;
                if (bool.TryParse(Value, out v))
                    return v;
                return !string.IsNullOrEmpty(Value);
            }
            set
            {
                Value = (value) ? "true" : "false";
            }
        }
        public virtual JSArray AsArray
        {
            get
            {
                return this as JSArray;
            }
        }
        public virtual JSObject AsObject
        {
            get
            {
                return this as JSObject;
            }
        }


        #endregion typecasting properties

        #region operators
        public static implicit operator JSNode(string s)
        {
            return new JSData(s);
        }
        public static implicit operator JSNode(int i)
        {
            return new JSData(i);
        }
        public static implicit operator JSNode(float i)
        {
            return new JSData(i);
        }
        public static implicit operator JSNode(bool i)
        {
            return new JSData(i);
        }
        public static implicit operator string(JSNode d)
        {
            return (d == null) ? null : d.Value;
        }
        public static bool operator ==(JSNode a, object b)
        {
            if (b == null && a is JSCreator)
                return true;
            return System.Object.ReferenceEquals(a, b);
        }

        public static bool operator !=(JSNode a, object b)
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

        internal static string Escape(string aText)
        {
            string result = "";
            foreach (char c in aText)
            {
                switch (c)
                {
                    case '\\': result += "\\\\"; break;
                    case '\"': result += "\\\""; break;
                    case '\n': result += "\\n"; break;
                    case '\r': result += "\\r"; break;
                    case '\t': result += "\\t"; break;
                    case '\b': result += "\\b"; break;
                    case '\f': result += "\\f"; break;
                    default: result += c; break;
                }
            }
            return result;
        }

    } // End of JSONNode

}