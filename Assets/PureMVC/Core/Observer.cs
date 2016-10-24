using UnityEngine;
using System;
using System.Reflection;

public class Observer : IObserver
{
    protected readonly object m_syncRoot = new object();
    public Observer(string notifyMethod, object notifyContext)
    {
        m_notifyMethod = notifyMethod;
        m_notifyContext = notifyContext;
    }
    public virtual void NotifyObserver<T>(INotification<T> notification)
    {
        object context;
        string method;

        // Retrieve the current state of the object, then notify outside of our thread safe block
        lock (m_syncRoot)
        {
            context = NotifyContext;
            method = NotifyMethod;
        }

        Type t = context.GetType();
        BindingFlags f = BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase;
        MethodInfo mi = t.GetMethod(method, f);
        try {
            mi.Invoke(context, new object[] { notification });
        }
        catch
        {
            Debug.LogWarningFormat("使用{0}的Mediator格式不对或者观察者名字重复", notification.ObserverName);
        }
    }
    public virtual bool CompareNotifyContext(object obj)
    {
        lock (m_syncRoot)
        {
            return NotifyContext.Equals(obj);
        }
    }

    public virtual string NotifyMethod
    {
        private get
        {
            return m_notifyMethod;
        }
        set
        {
            m_notifyMethod = value;
        }
    }
    public virtual object NotifyContext
    {
        private get
        {
            return m_notifyContext;
        }
        set
        {
            m_notifyContext = value;
        }
    }

    private string m_notifyMethod;

    private object m_notifyContext;

}