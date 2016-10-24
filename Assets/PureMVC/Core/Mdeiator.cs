using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
public abstract class Mediator : IMediator
{
    private string mediatorName;
    public virtual string MediatorName
    {
        get {
            if (mediatorName == null){
                mediatorName = System.Guid.NewGuid().ToString();
            }
            return mediatorName;
        }
    }
    public abstract IList<string> ListNotificationInterests();
    public abstract void HandleNotification(INotification notification);

    public virtual void OnRegister() { }
    public virtual void OnRemove() { }
}

public abstract class Mediator<T> : IMediator<T>
{
    private string mediatorName;
    public virtual string MediatorName
    {
        get
        {
            if (mediatorName == null)
            {
                mediatorName = System.Guid.NewGuid().ToString();
            }
            return mediatorName;
        }
    }
    public abstract IList<string> ListNotificationInterests ();
	public abstract void HandleNotification (INotification<T> notification);

    public virtual void OnRegister() { }
    public virtual void OnRemove() { }
}