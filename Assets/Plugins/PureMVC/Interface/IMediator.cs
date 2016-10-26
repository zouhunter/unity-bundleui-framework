using UnityEngine;
using System.Collections.Generic;
public interface IMediator
{
    string MediatorName { get; }
    IList<string> ListNotificationInterests();
    void OnRegister();
    void OnRemove();
}
public interface ISimpleMediator: IMediator
{
    void HandleNotification(INotification notify);
}
public interface IMediator<T>:IMediator
{
    void HandleNotification(INotification<T> notify);
}
