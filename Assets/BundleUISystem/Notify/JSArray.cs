using System;
using System.Collections.Generic;
using System.Collections;

namespace BundleUISystem
{
    public class JSArray : JSNode, IEnumerable
    {
        private List<JSNode> m_List = new List<JSNode>();
        public virtual JSNode this[int aIndex]
        {
            get
            {
                if (aIndex < 0 || aIndex >= m_List.Count)
                    return new JSCreator(this);
                return m_List[aIndex];
            }
            set
            {
                if (aIndex < 0 || aIndex >= m_List.Count)
                    m_List.Add(value);
                else
                    m_List[aIndex] = value;
            }
        }
        public virtual JSNode this[string aKey]
        {
            get { return new JSCreator(this); }
            set { m_List.Add(value); }
        }
        public virtual int Count
        {
            get { return m_List.Count; }
        }
        public override void Add(string aKey, JSNode aItem)
        {
            m_List.Add(aItem);
        }
        public virtual JSNode Remove(int aIndex)
        {
            if (aIndex < 0 || aIndex >= m_List.Count)
                return null;
            JSNode tmp = m_List[aIndex];
            m_List.RemoveAt(aIndex);
            return tmp;
        }
        public virtual JSNode Remove(JSNode aNode)
        {
            m_List.Remove(aNode);
            return aNode;
        }
        public override IEnumerable<JSNode> Childs
        {
            get
            {
                foreach (JSNode N in m_List)
                    yield return N;
            }
        }
        public IEnumerator GetEnumerator()
        {
            foreach (JSNode N in m_List)
                yield return N;
        }
        public override string ToString()
        {
            string result = "[ ";
            foreach (JSNode N in m_List)
            {
                if (result.Length > 2)
                    result += ", ";
                result += N.ToString();
            }
            result += " ]";
            return result;
        }

    } // End of JSONArray

}