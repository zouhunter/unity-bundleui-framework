using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class RotYMoveXTween : MutiOpenCloseTween
{
    public RotYMoveXTween(Transform target,float x,Vector3 rotEnd, float duration, bool startActive, bool endActive) : base(target, duration, startActive, endActive)
    {
        Tweener RotY = target.DORotate(rotEnd, duration / 3f).SetLoops(3);
        Tweener moveX = target.DOMoveX(x, duration);
        tweenrs.Append(RotY);
        tweenrs.Join(moveX);
        tweenrs.SetAutoKill(false).Pause();
    }
}
