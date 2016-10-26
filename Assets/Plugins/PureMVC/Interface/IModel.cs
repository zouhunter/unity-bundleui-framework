using System;

public interface IModel {
	void RegisterProxy(IProxy type);
	T RetrieveProxy<T>(string proxyName);
    IProxy RemoveProxy(string proxyName);
	bool HasProxy(string proxyName);
}
