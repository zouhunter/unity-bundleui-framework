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
        public Queue<object> dataQueue = new Queue<object>();//多次打开使用
        public UnityAction<GameObject> OnCreate;

        public enum Type
        {
            Name = 0,
            Button = 1,
            Toggle = 2,
            Enable = 3,
        }

        public enum Layer : int
        {
            Heap = 1<<0,//堆叠面版
            Mask = 1<<1,//遮罩面板
            Pop = 1<<2,//弹窗提示
            Tip = 1<<3,//最高显示,没有射线识别不会影响其他ui
        }
    }
}