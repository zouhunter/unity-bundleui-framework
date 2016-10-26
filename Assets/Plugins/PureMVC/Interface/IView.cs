using System;

public interface IView {
	void RegisterObserver(string observerName, IObserver observer);
	void NotifyObservers<T>(INotification<T> noti);
    void RemoveObserver(string observerName, object notifyContext);

	void RegisterMediator(IMediator mediator);
	T RetrieveMediator<T>(string mediatorName) where T :IMediator;
	IMediator RemoveMediator(string mediatorName);

	bool HasMediator(string mediatorName);
    bool HasObserver(string observerName);
}
