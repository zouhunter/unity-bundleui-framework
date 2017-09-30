using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using BundleUISystem.Internal;
namespace BundleUISystem
{
    public class UIPanelTemp : MonoBehaviour, IPanelButton, IPanelName, IPanelToggle
    {
        protected Button m_Btn;
        protected Toggle m_Tog;
        public event UnityAction onDelete;
        public event UnityAction<UIData> onCallBack;

        public virtual Button Btn
        {
            set
            {
                m_Btn = value;
            }
        }

        public virtual Toggle toggle
        {
            set
            {
                m_Tog = value;
                m_Tog.onValueChanged.AddListener((x) => { gameObject.SetActive(x); });
            }
        }
        public virtual void HandleData(UIData data)
        {
            gameObject.SetActive(true);
        }
        protected void CallBack(UIData statu)
        {
            if(onCallBack != null)
            {
                onCallBack.Invoke(statu);
            }
        }
        protected void CallBack<T>(T statu)
        {
            if (onCallBack != null)
            {
                onCallBack.Invoke(UIData.Allocate<T>(statu));
            }
        }
        protected virtual void OnDestroy()
        {
            if (onDelete != null) onDelete();
        }
    }
}

