using UnityEngine;
using System;
using UnityEngine.Events;
public interface IFacade:INotifier
{
    void RegisterProxy(IProxy prox);
    IProxy RetrieveProxy(string name);
    T RetrieveProxy<T>(string name) where T : IProxy;
    IProxy RemoveProxy(string name);

    void RegisterMediator(IMediator mediator);
    IMediator RetrieveMediator(string name);
    T RetrieveMediator<T>(string name) where T : IMediator;
    void RemoveMediator(string name);

	void RegisterCommand<T>(string noti)where T:ICommand,new ();
	void RemoveCommand(string noti);

    void RegisterEvent(string noti, UnityAction even);
    void RegisterEvent<T>(string noti, UnityAction<T> even);

    void RemoveEvent(string noti, UnityAction even);
    void RemoveEvent<T>(string noti, UnityAction<T> even);
    void RemoveEvents(string noti);
}
