﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using BundleUISystem;

namespace MyNamespace
{
    public class Panel1 : UIPanelTemp
    {
        public override void HandleData(object data)
        {
            Debug.Log(data);
        }
    }
}
