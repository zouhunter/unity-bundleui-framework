﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
namespace BundleUISystem.Internal
{
    public interface IPanelName
    {
        event UnityAction<JSONNode> OnDelete;
        void HandleData(JSONNode data);
    }

}