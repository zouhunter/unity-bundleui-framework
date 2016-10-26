using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;


public class TestPanel : MonoBehaviour, IRunTimeButton
{
    public Toggle.ToggleEvent openClose;
    private Button openBtn;
    public Button Btn
    {
        set
        {
            openBtn = value;
        }
    }
    public Button closeBtn;

    public event UnityAction OnDelete;

    void Start()
    {
        openBtn.onClick.AddListener(OnOpenClicked);
        closeBtn.onClick.AddListener(ClosePanel);

        openClose.Invoke(true);
    }

    void OnOpenClicked()
    {
        openClose.Invoke(true);
    }

    void ClosePanel()
    {
        //方法一：保留在内存中
        //openClose.Invoke(false);
        //方法二：直接销毁
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (OnDelete != null) OnDelete();
    }
}
