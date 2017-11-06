using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
namespace BundleUISystem
{
    public abstract class ItemInfoBase
    {
        public int instanceID;
        public string assetName;
        public abstract string IDName { get; }
        public Button button;
        public Toggle toggle;
        public GameObject instence;
        public Layer parentLayer;
        public Type type = Type.Name;
        public Queue<UIData> dataQueue = new Queue<UIData>();//多次打开使用
        public UnityAction<GameObject> OnCreate;

        public enum Type
        {
            Name = 0,
            Button = 1,
            Toggle = 2,
            Enable = 3,
        }

        public enum Layer:int
        {
            Background,
            PopWithMask,
            PopNoMask,
            Tip,
        }
    }
}