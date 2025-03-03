using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using TMPro;

public class GuiLoading : GuiBase
{
    private Transform mask;                 // 遮罩层 Mask
    private const float ANIME_TIME = 0.8f;    // 动画时间 Animation time
    public bool ready_for_load { get; private set; } // 动画完成 Animation completed

    /// <summary>
    /// 加载界面显示事件 
    /// Loading interface display event
    /// 完成进入动画，触发后续事件
    /// Complete the entry animation and trigger subsequent events
    /// </summary>
    public override void Show()
    {
        ready_for_load = false;
        mask = GetComponent<Image>("MaskImage").transform;     // 初始化 initial
        mask.localScale = new Vector3(1f, 1f, 1f); // 初始化 initial
        mask.DOScale(new Vector3(0f, 0f, 1f), ANIME_TIME)
            .SetEase(Ease.Linear)
            .OnComplete(() => {
                ready_for_load = true;
                MgrEvent.Mgr().EventTrigger("EnterLoadingDone");
            });
    }

    /// <summary>
    /// 加载界面隐藏事件
    /// Loading interface hidden event
    /// 完成退出动画，触发后续事件
    /// Complete the exit animation and trigger subsequent events
    /// </summary>
    public void Exit(){
        ready_for_load = false;
        mask.localScale = new Vector3(0f, 0f, 1f); // 初始化 initial
        mask.DOScale(new Vector3(1f, 1f, 1f), ANIME_TIME)
            .SetEase(Ease.Linear)
            .OnComplete(() => {
                MgrEvent.Mgr().EventTrigger("ExitLoadingDone");
                MgrGui.Mgr().HideGui("GuiLoading");
            });
    }
}