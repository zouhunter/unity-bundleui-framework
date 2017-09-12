using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
namespace BundleUISystem.Internal
{
    public interface IEventHold
    {
        void Record(string key, UnityAction<JSONObject> handle);
        bool Remove(string key, UnityAction<JSONObject> handle);
        void Remove(string key);
        bool NotifyObserver(string key);
        bool NotifyObserver(string key, JSONObject value);
    }
}