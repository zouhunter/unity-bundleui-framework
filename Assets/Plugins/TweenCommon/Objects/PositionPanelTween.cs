using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class PositionPanelTween : OpenCloseTween
{
    public PositionPanelTween(Transform target,Vector3 startPos, float duration, bool startActive, bool endActive) : base(target, duration, startActive, endActive)
    {
        tweenr = target.DOMove(startPos, duration).SetRelative(false).SetAutoKill(false).Pause();
    }
}
