using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BundleUISystem
{
    internal class JSCreator : JSNode
    {
        private JSNode m_Node = null;
        private string m_Key = null;

        public JSCreator(JSNode aNode)
        {
            m_Node = aNode;
            m_Key = null;
        }
        public JSCreator(JSNode aNode, string aKey)
        {
            m_Node = aNode;
            m_Key = aKey;
        }

        private void Set(JSNode aVal)
        {
            if (m_Key == null)
            {
                m_Node.Add(aVal);
            }
            else
            {
                m_Node.Add(m_Key, aVal);
            }
            m_Node = null; // Be GC friendly.
        }

        public virtual JSNode this[int aIndex]
        {
            get
            {
                return new JSCreator(this);
            }
            set
            {
                var tmp = new JSArray();
                tmp.Add(value);
                Set(tmp);
            }
        }

        public virtual JSNode this[string aKey]
        {
            get
            {
                return new JSCreator(this, aKey);
            }
            set
            {
                var tmp = new JSObject();
                tmp.Add(aKey, value);
                Set(tmp);
            }
        }
        public override void Add(JSNode aItem)
        {
            var tmp = new JSArray();
            tmp.Add(aItem);
            Set(tmp);
        }
        public override void Add(string aKey, JSNode aItem)
        {
            var tmp = new JSObject();
            tmp.Add(aKey, aItem);
            Set(tmp);
        }
        public static bool operator ==(JSCreator a, object b)
        {
            if (b == null)
                return true;
            return System.Object.ReferenceEquals(a, b);
        }

        public static bool operator !=(JSCreator a, object b)
        {
            return !(a == b);
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return true;
            return System.Object.ReferenceEquals(this, obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return "";
        }

        public override int AsInt
        {
            get
            {
                JSData tmp = new JSData(0);
                Set(tmp);
                return 0;
            }
            set
            {
                JSData tmp = new JSData(value);
                Set(tmp);
            }
        }
        public override float AsFloat
        {
            get
            {
                JSData tmp = new JSData(0.0f);
                Set(tmp);
                return 0.0f;
            }
            set
            {
                JSData tmp = new JSData(value);
                Set(tmp);
            }
        }
        public override double AsDouble
        {
            get
            {
                JSData tmp = new JSData(0.0);
                Set(tmp);
                return 0.0;
            }
            set
            {
                JSData tmp = new JSData(value);
                Set(tmp);
            }
        }
        public override bool AsBool
        {
            get
            {
                JSData tmp = new JSData(false);
                Set(tmp);
                return false;
            }
            set
            {
                JSData tmp = new JSData(value);
                Set(tmp);
            }
        }
        public override JSArray AsArray
        {
            get
            {
                JSArray tmp = new JSArray();
                Set(tmp);
                return tmp;
            }
        }
        public override JSObject AsObject
        {
            get
            {
                JSObject tmp = new JSObject();
                Set(tmp);
                return tmp;
            }
        }
    } // End of JSONLazyCreator
}