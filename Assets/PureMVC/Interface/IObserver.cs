public interface IObserver {

    string NotifyMethod { set; }
    object NotifyContext { set; }
    void NotifyObserver<T>(INotification<T> notification);
    bool CompareNotifyContext(object obj);
}
