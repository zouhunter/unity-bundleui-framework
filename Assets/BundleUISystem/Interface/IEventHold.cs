using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
namespace BundleUISystem.Internal
{
    public interface IEventHold
    {
        void Record(string key, UnityAction<JSNode> handle);
        bool Remove(string key, UnityAction<JSNode> handle);
        void Remove(string key);
        bool NotifyObserver(string key);
        bool NotifyObserver(string key, JSNode value);
    }
}