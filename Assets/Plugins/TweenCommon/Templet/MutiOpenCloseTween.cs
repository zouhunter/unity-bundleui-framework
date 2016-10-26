using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public abstract class MutiOpenCloseTween : IUITween
{
    protected Sequence tweenrs;
    protected bool endActive;
    protected bool startActive;
    protected float duration;
    protected Transform tranform;
    protected bool playforward;
    public MutiOpenCloseTween(Transform target, float duration, bool startActive, bool endActive)
    {
        this.tranform = target;
        this.startActive = startActive;
        this.endActive = endActive;
        this.duration = duration;
        tweenrs = DOTween.Sequence();
    }

    public void Kill()
    {
        tweenrs.Kill();
    }

    public void TogglePause()
    {
        tweenrs.TogglePause();
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
            if (tweenrs.IsPlaying())
            {
                tweenrs.PlayForward();
            }
            else
            {
                tweenrs.OnComplete(() => {
                    tranform.gameObject.SetActive(endActive);
                }).Restart();
            }
        }
        else
        {
            tweenrs.OnComplete(() => {
                tranform.gameObject.SetActive(startActive);
            }).PlayBackwards();
        }
    }

    public void Rewind()
    {
        tweenrs.Rewind();
        tranform.gameObject.SetActive(startActive);
    }
    public void TogglePlay()
    {
        Play(!playforward);
    }
}
