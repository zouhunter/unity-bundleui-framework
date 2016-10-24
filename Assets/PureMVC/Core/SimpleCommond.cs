
public abstract class Command : Notifyer, ICommand
{
	public abstract void Execute(INotification notification);
}
public abstract class Command<T> : Notifyer, ICommand<T>
{
	public abstract void Execute(INotification<T> notification);
}