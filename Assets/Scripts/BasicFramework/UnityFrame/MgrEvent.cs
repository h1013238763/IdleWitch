using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 事件中心模块管理器
/// Event Center Module Manager
/// </summary>
public class MgrEvent : MgrBase<MgrEvent>
{
    // 创建监听列表 Create Listener List
    private Dictionary<string, IEventInfo> event_dict = new Dictionary<string, IEventInfo>();

    /// <summary>
    /// 添加事件监听
    /// Add Event Listener
    /// </summary>
    /// <param name="name"> 事件名称 Event Name </param>
    /// <param name="action"> 触发方法 Trigger Method </param>
    public void AddEventListener<T>(string name, UnityAction<T> action)
    {
        // 查找对应监听 Find corresponding listener
        if(event_dict.ContainsKey(name))
            (event_dict[name] as EventInfo<T>).actions += action;
        else
            event_dict.Add(name, new EventInfo<T>( action ));
    }

    /// <summary>
    /// 添加无参事件监听
    /// Add No Parameter Event Listener
    /// </summary>
    /// <param name="name"> 事件名称 Event Name </param>
    /// <param name="action"> 触发方法 Trigger Method </param>
    public void AddEventListener(string name, UnityAction action)
    {
        // 查找对应监听 Find corresponding listener
        if(event_dict.ContainsKey(name))
            (event_dict[name] as EventInfo).actions += action;
        else
            event_dict.Add(name, new EventInfo( action ));
    }

    /// </summary>
    /// 触发带参数事件
    /// Trigger Event with Parameters
    /// </summary>
    /// <param name="name">事件名称 Event Name</param>
    public void EventTrigger<T>(string name, T info)
    {
        // 查找对应监听 Find corresponding listener
        if(event_dict.ContainsKey(name))
        {
            if((event_dict[name] as EventInfo<T>).actions != null)
                (event_dict[name] as EventInfo<T>).actions.Invoke(info);
        }
    }

    /// <summary>
    /// 触发无参事件
    /// Trigger No Parameter Event
    /// </summary>
    /// <param name="name">事件名称 Event Name</param>
    public void EventTrigger(string name)
    {
        // 查找对应监听 Find corresponding listener
        if(event_dict.ContainsKey(name))
        {
            if((event_dict[name] as EventInfo).actions != null)
                (event_dict[name] as EventInfo).actions.Invoke();
        }
    }

    /// <summary>
    /// 移除事件监听
    /// Remove Event Listener
    /// </summary>
    /// <param name="name"> 事件名称 Event Name </param>
    /// <param name="action"> 触发方法 Trigger Method </param>
    public void RemoveEventListener<T>(string name, UnityAction<T> action)
    {
        if(event_dict.ContainsKey(name))
            (event_dict[name] as EventInfo<T>).actions-= action;
    }

    /// <summary>
    /// 移除无参事件监听
    /// Remove No Parameter Event Listener
    /// </summary>
    /// <param name="name"> 事件名称 Event Name </param>
    /// <param name="action"> 触发方法 Trigger Method </param>
    public void RemoveEventListener(string name, UnityAction action)
    {
        if(event_dict.ContainsKey(name))
            (event_dict[name] as EventInfo).actions-= action;
    }

    /// <summary>
    /// 清空特定事件监听
    /// Clear Specific Event Listener
    /// </summary>
    /// <param name="name"> 事件名称 Event Name </param>
    public void Clear(string name)
    {
        if(event_dict.ContainsKey(name))
            event_dict.Remove(name);
    }

    /// <summary>
    /// 清空全部事件监听
    /// Clear All Event Listener
    /// </summary>
    public void Clear()
    {
        event_dict.Clear();
    }

    // 包装用接口和类
    // Packaged Interface and Class
    internal interface IEventInfo{}

    // 有参事件信息类 Event Information Class with Parameters
    internal class EventInfo<T> : IEventInfo
    {
        public UnityAction<T> actions;
        public EventInfo( UnityAction<T> action )
        {
            actions += action;
        }
    }

    // 无参事件信息类 Event Information Class without Parameters
    internal class EventInfo : IEventInfo
    {
        public UnityAction actions;
        public EventInfo( UnityAction action )
        {
            actions += action;
        }
    }
}



