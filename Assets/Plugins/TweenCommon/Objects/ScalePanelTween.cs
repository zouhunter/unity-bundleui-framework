using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class ScalePanelTween:OpenCloseTween{
    public ScalePanelTween(Transform target,Vector3 smallest,float duration,int loop = 1,Ease type = Ease.Linear,bool startActive = false,bool endActive = true):base(target, duration, startActive,endActive)
    {
        tweenr = target.DOScale(smallest, duration).SetEase(type).SetLoops(loop).SetAutoKill(false).Pause();
    }
}
