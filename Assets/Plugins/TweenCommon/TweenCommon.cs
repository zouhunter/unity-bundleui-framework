using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;


public class TweenCommon : SingleManager<TweenCommon>
{
    public enum TweenType
    {
        ScalePanel,
        ScaleWoringPanel,
        UpPostionPanel,
        RotatePanel,
    }

    private int s_duration = 1;

    public void InitTweenCommon(int singleDuration)
    {
        this.s_duration = singleDuration;
    }

    /// <summary>
    /// 获取一个通用的Tween
    /// </summary>
    /// <param name="type"></param>
    /// <param name="transf"></param>
    /// <returns></returns>
    public IUITween GetNormalTween(TweenType type,Transform transf)
    {
        IUITween tween = null;
        switch (type)
        {
            case TweenType.ScalePanel:
                transf.localScale = Vector3.one * 0.8f;
                tween = new ScalePanelTween(transf, Vector3.one, s_duration, startActive: false);
                break;
            case TweenType.ScaleWoringPanel:
                transf.localScale = Vector3.one * 0.8f;
                tween = new ScalePanelTween(transf, Vector3.one, s_duration/3f,3,DG.Tweening.Ease.InSine, startActive: false);
                break;
            case TweenType.UpPostionPanel:
                Vector3 startPos = transf.position;
                transf.position += Vector3.up * 10f;
                tween = new PositionPanelTween(transf, startPos, s_duration, false, true);
                break;
            case TweenType.RotatePanel:
                transf.localEulerAngles = Vector3.up * 45;
                tween = new RotPanelTween(transf, Vector3.zero, s_duration, false, true);
                break;
            default:
                break;
        }
        return tween;
    }
}
