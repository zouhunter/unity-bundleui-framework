﻿using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
namespace BundleUISystem
{
    public abstract class ItemInfoBase
    {
        //#if UNITY_EDITOR
        public int instanceID;
        //#endif

        public string assetName;
        public abstract string IDName { get; }
        public Button button;
        public Toggle toggle;
        public GameObject instence;
        public bool reset;
        public int parentLayer;
        public Type type = Type.Name;
        public Queue<JSONNode> dataQueue = new Queue<JSONNode>();//多次打开使用
        public UnityAction<GameObject> OnCreate;
        public enum Type
        {
            Name = 0,
            Button = 1,
            Toggle = 2,
            Enable = 3,
        }
    }
}