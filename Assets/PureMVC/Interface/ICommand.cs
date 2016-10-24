public interface ICommand<T>:ICommand
{
    void Execute(INotification<T> notify);
}
public interface ICommand{}
