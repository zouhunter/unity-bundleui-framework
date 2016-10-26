using System;

public interface INotification
{
    string ObserverName { get; set; }
    Type Type { get; set; }
    string ToString { get; }
    bool Destroy { get; set; }
    bool IsUsing { get; set; }
}

public interface INotification<T>:INotification{
    T Body { get; set; }
}