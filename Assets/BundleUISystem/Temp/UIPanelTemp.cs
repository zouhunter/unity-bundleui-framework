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
        public event UnityAction<object> onCallBack;
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
        public virtual void HandleData(object data)
        {
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
        }

        protected void CallBack(object callBackData)
        {
            if(onCallBack != null){
                onCallBack.Invoke(callBackData);
            }
        }

        protected virtual void OnDestroy()
        {
            if (onDelete != null) onDelete();
        }
    }
}

