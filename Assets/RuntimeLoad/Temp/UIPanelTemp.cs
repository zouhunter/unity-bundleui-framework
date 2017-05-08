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
        [SerializeField]
        protected UnityEvent m_OnOpen;
        protected Button m_Btn;
        [SerializeField]
        protected Toggle.ToggleEvent m_OpenClose;
        protected Toggle m_Tog;
        public event Action OnDelete;
        protected virtual void OnEnable(){
            m_OnOpen.Invoke();
            m_OpenClose.Invoke(true);
        }
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

        }

        protected virtual void OnDestroy()
        {
            m_OpenClose.Invoke(false);
            if (OnDelete != null) OnDelete();
        }
    }
}

