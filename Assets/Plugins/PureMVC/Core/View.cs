using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class View :IView
{
    protected IDictionary<string, IMediator> m_mediatorMap;
	protected IDictionary<string, List<IObserver>> m_observerMap;

	public static volatile IView Instance = new View();
    protected readonly object m_syncRoot = new object();
    protected View()
    {
        m_mediatorMap = new Dictionary<string, IMediator>();
		m_observerMap = new Dictionary<string, List<IObserver>>();
        InitializeView();
    }
    protected virtual void InitializeView()
    {

    }
    /// <summary>
    /// 注册成为观察者
    /// </summary>
    /// <param name="obName"></param>
    /// <param name="observer"></param>
	public void RegisterObserver(string eventName, IObserver observer)
    {
        lock (m_syncRoot)
        {
            if (m_observerMap.ContainsKey(eventName))
            {
                if (!m_observerMap[eventName].Contains(observer))
                {
                    m_observerMap[eventName].Add(observer);
                }
            }
            else
            {
                m_observerMap.Add(eventName, new List<IObserver>() { observer });
            }
        }
    }
    /// <summary>
    /// 通知所有观察者
    /// </summary>
    /// <param name="notify"></param>
    public void NotifyObservers<T>(INotification<T> noti)
    {
        IList<IObserver> observers = null;

        lock (m_syncRoot)
        {
            if (m_observerMap.ContainsKey(noti.ObserverName))
            {
                IList<IObserver> observers_ref = m_observerMap[noti.ObserverName];
                observers = new List<IObserver>(observers_ref);
            }
        }

        if (observers != null)
        {
            for (int i = 0; i < observers.Count; i++)
            {
                IObserver observer = observers[i];
                observer.NotifyObserver<T>(noti);
            }
        }
    }

    /// <summary>
    /// 将指定的观察者移除
    /// </summary>
    /// <param name="name"></param>
	public void RemoveObserver(string eventName, object notifyContext)
    {
        lock (m_syncRoot)
        {
            // the observer list for the notification under inspection
            if (m_observerMap.ContainsKey(eventName))
            {
                IList<IObserver> observers = m_observerMap[eventName];

                for (int i = 0; i < observers.Count; i++)
                {
                    if (observers[i].CompareNotifyContext(notifyContext))
                    {
                        observers.RemoveAt(i);
                        break;
                    }
                }

                if (observers.Count == 0)
                {
                    m_observerMap.Remove(eventName);
                }
            }
        }
    }
    /// <summary>
    /// 将所有的观察者移除
    /// </summary>
    /// <param name="name"></param>
	public void RemoveObservers(string eventName)
    {
        lock (m_syncRoot)
        {
            if (m_observerMap.ContainsKey(eventName))
            {
                m_observerMap.Remove(eventName);
            }
        }
    }
    /// <summary>
    /// 注册mediator
    /// </summary>
    /// <param name="notify"></param>
    public void RegisterMediator(IMediator mediator)
    {
        lock (m_syncRoot)
        {
            if (m_mediatorMap.ContainsKey(mediator.MediatorName)) return;

            // Register the Mediator for retrieval by name
            m_mediatorMap[mediator.MediatorName] = mediator;

            // Get Notification interests, if any.
			IList<string> interests = mediator.ListNotificationInterests();
            // Register Mediator as an observer for each of its notification interests
            if (interests.Count > 0)
            {
                // Create Observer
                IObserver observer = new Observer("HandleNotification", mediator);

                // Register Mediator as Observer for its list of Notification interests
                for (int i = 0; i < interests.Count; i++)
                {
                    RegisterObserver(interests[i], observer);
                }
            }
        }
        // alert the mediator that it has been registered
        mediator.OnRegister();
    }
    /// <summary>
    /// 获取Mediator
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Name"></param>
    /// <returns></returns>
    public T RetrieveMediator<T>(string Name) where T : IMediator
    {
        lock (m_syncRoot)
        {
            if (!m_mediatorMap.ContainsKey(Name)) return default(T);
            IMediator manager = null;
            m_mediatorMap.TryGetValue(Name, out manager);
            return (T)manager;
        }
    }
    /// <summary>
    /// 移除mediator
    /// </summary>
    /// <param name="notify"></param>
    public IMediator RemoveMediator(string mediatorName)
    {
        IMediator mediator = null;

        lock (m_syncRoot)
        {
            if (!m_mediatorMap.ContainsKey(mediatorName)) return null;
            mediator = (IMediator)m_mediatorMap[mediatorName];

			IList<string> interests = mediator.ListNotificationInterests();

            for (int i = 0; i < interests.Count; i++)
            {
                RemoveObserver(interests[i], mediator);
            }

            m_mediatorMap.Remove(mediatorName);
        }

        if (mediator != null) mediator.OnRemove();
        return mediator;
    }
    /// <summary>
    /// 是否有mdeiator
    /// </summary>
    /// <param name="mediatorName"></param>
    /// <returns></returns>
    public bool HasMediator(string mediatorName)
    {
        lock (m_syncRoot)
        {
            return m_mediatorMap.ContainsKey(mediatorName);
        }
    }
    /// <summary>
    /// 是否含有观察者
    /// </summary>
    /// <param name="observerName"></param>
    /// <returns></returns>
    public bool HasObserver(string observerName)
    {
        lock(m_syncRoot)
        {
            return m_observerMap.ContainsKey(observerName);
        }
    }

}