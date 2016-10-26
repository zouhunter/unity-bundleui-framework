using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class RotPanelTween : OpenCloseTween
{
    public RotPanelTween(Transform target,Vector3 endRot, float duration, bool startActive, bool endActive) : base(target, duration, startActive, endActive)
    {
        tweenr = target.DORotate(endRot, duration).SetAutoKill(false).Pause();
    }
}
