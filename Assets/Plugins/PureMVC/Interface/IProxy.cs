using UnityEngine;
using System.Collections.Generic;
public interface IProxy
{
    string ProxyName { get; }
    void OnRegister();
    void OnRemove();
}
public interface IProxy<T>:IProxy {
	T Data { get; set; }
}
