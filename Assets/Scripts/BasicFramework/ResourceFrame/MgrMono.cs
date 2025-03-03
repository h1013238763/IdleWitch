using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Mono协同管理器
/// Mono Coroutine Manager
/// </summary>
public class MgrMono : MgrBaseMono<MgrMono> 
{
    private event UnityAction update_event;         // 动画帧函数 update frame events
    private event UnityAction fixed_update_event;   // 固定时间帧函数 fixed time frame events
    private event UnityAction late_update_event;    // 延后帧函数 late update frame events

    /// <summary>
    /// 添加Update函数
    /// Add Update Function
    /// </summary>
    /// <param name="action"> Update函数 update function </param>
    public void AddUpdateEvent(UnityAction action)
    {
        update_event += action;
    }

    /// <summary>
    /// 移除Update函数
    /// Remove Update Function
    /// </summary>
    /// <param name="action"> Update函数 update function </param>
    public void DelUpdateEvent(UnityAction action)
    {
        update_event -= action;
    }

    /// <summary>
    /// 添加FixedUpdate函数
    /// Add FixedUpdate Function
    /// </summary>
    /// <param name="action"> Update函数 update function </param>
    public void AddFixedUpdateEvent(UnityAction action)
    {
        fixed_update_event += action;
    }

    /// <summary>
    /// 移除FixedUpdate函数
    /// Remove FixedUpdate Function
    /// </summary>
    /// <param name="action"> Update函数 update function </param>
    public void DelFixedUpdateEvent(UnityAction action)
    {
        fixed_update_event -= action;
    }

    /// <summary>
    /// 添加LateUpdate函数
    /// Add LateUpdate Function
    /// </summary>
    /// <param name="action"> Update函数 update function </param>
    public void AddLateUpdateEvent(UnityAction action)
    {
        late_update_event += action;
    }

    /// <summary>
    /// 移除LateUpdate函数
    /// Remove LateUpdate Function
    /// </summary>
    /// <param name="action"> Update函数 update function </param>
    public void DelLateUpdateEvent(UnityAction action)
    {
        late_update_event -= action;
    }

    // 每游戏动画帧触发
    // Triggered every game update frame ( animation frame )
    private void Update()
    {
        if( update_event != null)
            update_event();
    }

    // 每固定时间帧触发
    // Triggered every fixed time frame ( time frame )
    private void FixedUpdate()
    {
        if( fixed_update_event != null )
            fixed_update_event();
    }

    // 每动画帧之后触发
    // Triggered after each animation frame
    private void LateUpdate()
    {
        if( late_update_event != null )
            late_update_event();
    }

    /// <summary>
    /// 开启协程
    /// Start Coroutine
    /// </summary>
    /// <param name="routine"> 协程函数 Coroutine Function </param> 
    public void StartCoroutine(IEnumerator routine){
        base.StartCoroutine(routine);
    }
}