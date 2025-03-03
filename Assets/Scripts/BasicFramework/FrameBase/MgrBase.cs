using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单例模式基类模块
/// Singleton Base Module 
/// </summary>
/// <typeparam name="T"> 类名称 Class Name </typeparam>
public class MgrBase<T> where T: new(){

    /// <summary>
    /// 单例实例 Singleton Instance
    /// </summary>
    private static T mgr;

    /// <summary>
    /// 获取类单例对象
    /// Get Class Singleton Object
    /// </summary>
    /// <returns>类单例对象 Class Singleton Object</returns>  
    public static T Mgr(){ 
        if(mgr == null)
            mgr = new T();
        return mgr;
    }
}