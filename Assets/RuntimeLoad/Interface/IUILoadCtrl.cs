using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace BundleUISystem.Internal
{
    public interface IUILoadCtrl
    {
        string Menu { get; }
        void GetGameObjectFromBundle(UIBundleInfo trigger);
    }
}