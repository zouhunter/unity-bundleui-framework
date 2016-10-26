using System;
public abstract class Proxy<T> :Notifyer, IProxy<T>
{
    public Proxy() { m_proxyName = ToString(); }
    public Proxy(string name){m_proxyName = name;}
    public Proxy(string name, T data)
    {
        m_proxyName = name;
        Data = data;
    }

    protected T m_data;
    public abstract T Data { get; set; }

    protected string m_proxyName;
    public virtual string ProxyName
    {
        get { return (m_proxyName != null) ? m_proxyName : this.ToString(); }
    }
    public virtual void OnRegister()
    {
    }
    public virtual void OnRemove()
    {
    }
}