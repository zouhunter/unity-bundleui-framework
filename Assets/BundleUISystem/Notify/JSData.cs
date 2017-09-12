using System;
using System.Collections.Generic;
using System.Collections;

namespace BundleUISystem
{

    public class JSData : JSNode
    {
        private string m_Data;
        public override string Value
        {
            get { return m_Data; }
            set { m_Data = value; }
        }
        public JSData(string aData)
        {
            m_Data = aData;
        }
        public JSData(float aData)
        {
            AsFloat = aData;
        }
        public JSData(double aData)
        {
            AsDouble = aData;
        }
        public JSData(bool aData)
        {
            AsBool = aData;
        }
        public JSData(int aData)
        {
            AsInt = aData;
        }

        public override string ToString()
        {
            return "\"" + Escape(m_Data) + "\"";
        }
    } // End of JSONData

}