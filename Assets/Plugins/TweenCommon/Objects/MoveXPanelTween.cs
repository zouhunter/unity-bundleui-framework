using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class MoveXPanelTween : OpenCloseTween
{
    public MoveXPanelTween(Transform target,float x, float duration, bool startActive, bool endActive) : base(target, duration, startActive, endActive)
    {
        tweenr = target.DOMoveX(x, duration).SetAutoKill(false).Pause();
    }
}
