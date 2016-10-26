using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;


public class SwitchAblePanel : MonoBehaviour
{
    public Transform panel;
    public TweenCommon.TweenType tweenType;
    IUITween tween;
    UnityAction<bool> delayAction;
    bool delyValue;
    void Start()
    {
        ///坐标需要在Start中才能获取到
        tween = TweenCommon.Instance.GetNormalTween(tweenType, panel);
        tween.Rewind();
        if (delayAction != null)
        {
            delayAction(delyValue);
            delayAction = null;
        }
    }

    public void ToggleOpenTween(bool isOpen)
    {
        if (tween == null)
        {
            delyValue = isOpen;
            delayAction = (x) => { tween.Play(isOpen); };
        }
        else
        {
            tween.Play(isOpen);
        }
    }
    public void PlayForward()
    {
        ToggleOpenTween(true);
    }

    public void PlayBackWard()
    {
        ToggleOpenTween(false);
    }

}
