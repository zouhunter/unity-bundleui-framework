using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RTObjUtility
{
    public static System.Action<string> MessageNotHandled;
    public static Dictionary<string, Delegate> m_needHandle = new Dictionary<string, Delegate>();
    public static void NoMessageHandle(string rMessage)
    {
        if (MessageNotHandled == null)
        {
            Debug.Log("action not registed!!!" + rMessage);
        }
        else
        {
            MessageNotHandled(rMessage);
        }
    }

    #region 注册注销事件

    public static void Record(string key, Delegate handle)
    {
        if (!m_needHandle.ContainsKey(key))
        {
            m_needHandle.Add(key, handle);
        }
        else
        {
            m_needHandle[key] = Delegate.Combine(m_needHandle[key], handle);
        }
    }
    public static bool Remove(string key, Delegate handle)
    {
        if (m_needHandle.ContainsKey(key))
        {
            m_needHandle[key] = Delegate.Remove(m_needHandle[key], handle);
            if (m_needHandle[key] == null)
            {
                m_needHandle.Remove(key);
                return false;
            }
        }
        return true;
    }
    public static void RemoveEvents(string key)
    {
        if (m_needHandle.ContainsKey(key))
        {
            m_needHandle.Remove(key);
        }
    }
    public static bool HaveEvent(string key)
    {
        return m_needHandle.ContainsKey(key);
    }
    #endregion

    #region 触发事件
    public static void Open(string assetName, object data = null)
    {
        bool lReportMissingRecipient = true;
        if (m_needHandle.ContainsKey(assetName))
        {
            m_needHandle[assetName].DynamicInvoke(data);
            lReportMissingRecipient = false;
        }

        if (lReportMissingRecipient)
        {
            NoMessageHandle(assetName);
        }
    }
    public static void Open<T>(object data = null) where T:RTObjTemp
    {
        string assetName = typeof(T).ToString();
        Open(assetName, data);
    }
    #endregion
}
