using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;

public abstract class OpenCloseTween : IUITween {
    protected Tweener tweenr;
    protected bool endActive;
    protected bool startActive;
    protected float duration;
    protected Transform tranform;
    protected bool playforward;
    public OpenCloseTween(Transform target,float duration, bool startActive, bool endActive)
    {
        this.tranform = target;
        this.startActive = startActive;
        this.endActive = endActive;
        this.duration = duration;
    }

    public void Kill()
    {
        tweenr.Kill();
    }

    public void TogglePause()
    {
        tweenr.TogglePause();
    }

    /// <summary>
    /// 向目标方向播放
    /// </summary>
    /// <param name="forward"></param>
    public void Play(bool forward = false)
    {
        playforward = forward;
        tranform.gameObject.SetActive(true);
        if (forward)
        {
            if (tweenr.IsPlaying())
            {
                tweenr.PlayForward();
            }
            else
            {
                tweenr.OnComplete(() => {
                    tranform.gameObject.SetActive(endActive);
                }).Restart();
            }
        }
        else
        {
            tweenr.OnRewind(() => {
                if (tweenr.IsBackwards()){
                    tranform.gameObject.SetActive(startActive);
                }
            }).PlayBackwards();
        }
    }

    /// <summary>
    /// 重置
    /// </summary>
    public void Rewind()
    {
        tweenr.Rewind();
        tranform.gameObject.SetActive(startActive);
    }
}
