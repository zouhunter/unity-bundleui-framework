using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
public class Controller : IController
{
    protected IView m_view;
    protected IDictionary<string, Type> m_commandMap;
    protected static volatile IController m_instance;
    protected readonly object m_syncRoot = new object();
    protected static readonly object m_staticSyncRoot = new object();
    protected Controller()
    {
        m_commandMap = new Dictionary<string, Type>();
        InitializeController();
    }
    public static IController Instance
    {
        get
        {
            if (m_instance == null)
            {
                lock (m_staticSyncRoot)
                {
                    if (m_instance == null) m_instance = new Controller();
                }
            }

            return m_instance;
        }
    }
    static Controller()
    {

    }
    protected virtual void InitializeController()
    {
        m_view = View.Instance;
    }

    public virtual void RegisterCommand(string noti, Type commandType)
    {
        lock (m_syncRoot)
        {
            if (!m_commandMap.ContainsKey(noti))
            {
                m_view.RegisterObserver(noti, new Observer("ExecuteCommand", this));
                m_commandMap.Add(noti, commandType);
            }
        }
    }
    /// <summary>
    /// 执行Command
    /// </summary>
    /// <param name="note"></param>
    public virtual void ExecuteCommand(INotification note)
    {
        Type commandType = null;

        lock (m_syncRoot)
        {
            if (!m_commandMap.ContainsKey(note.ObserverName)) return;
            commandType = m_commandMap[note.ObserverName];
        }

        object commandInstance = Activator.CreateInstance(commandType);

        Type t = commandInstance.GetType();
        BindingFlags f = BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase;
        MethodInfo mi = t.GetMethod("Execute", f);
        try
        {
            mi.Invoke(commandInstance, new object[] { note });
        }
        catch
        {
            Debug.LogWarningFormat("使用{0}的Mediator格式不对或者观察者名字重复", note.ObserverName);
        }
    }
    public virtual bool HasCommand(string notificationName)
    {
        lock (m_syncRoot)
        {
            return m_commandMap.ContainsKey(notificationName);
        }
    }
    public virtual void RemoveCommand(string notificationName)
    {
        lock (m_syncRoot)
        {
            if (m_commandMap.ContainsKey(notificationName))
            {
                m_view.RemoveObserver(notificationName, this);
                m_commandMap.Remove(notificationName);
            }
        }
    }
}