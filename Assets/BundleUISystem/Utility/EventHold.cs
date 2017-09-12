using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using BundleUISystem.Internal;

namespace BundleUISystem
{

    public class EventHold: IEventHold
    {
        private Dictionary<string, UnityAction<JSONObject>> m_needHandle = new Dictionary<string, UnityAction<JSONObject>>();

        #region 注册注销事件
        public void Record(string key, UnityAction<JSONObject> handle)
        {
            // First check if we know about the message type
            if (!m_needHandle.ContainsKey(key))
            {
                m_needHandle.Add(key, handle);
            }
            else
            {
                m_needHandle[key] += handle;
            }
        }

        public bool Remove(string key, UnityAction<JSONObject> handle)
        {
            if (m_needHandle.ContainsKey(key))
            {
                m_needHandle[key] -= handle;
                if (m_needHandle[key] == null)
                {
                    m_needHandle.Remove(key);
                    return false;
                }
            }
            return true;
        }

        public void Remove(string key)
        {
            if (m_needHandle.ContainsKey(key))
            {
                m_needHandle.Remove(key);
            }
        }
        #endregion

        #region 触发事件
        public bool NotifyObserver(string key)
        {
            bool lReportMissingRecipient = true;

            if (m_needHandle.ContainsKey(key))
            {
                m_needHandle[key].Invoke(null);

                lReportMissingRecipient = false;
            }

            return !lReportMissingRecipient;
        }
        public bool NotifyObserver(string key, JSONObject value)
        {
            bool lReportMissingRecipient = true;

            if (m_needHandle.ContainsKey(key))
            {
                m_needHandle[key].Invoke(value);

                lReportMissingRecipient = false;
            }

            return !lReportMissingRecipient;
        }
        #endregion

        #region 
        public bool HaveRecord(string key)
        {
            return m_needHandle.ContainsKey(key);
        }
        #endregion
    }
}
