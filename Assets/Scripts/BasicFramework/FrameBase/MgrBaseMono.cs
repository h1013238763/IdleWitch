using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单例模式Mono基类模块
/// Singleton Base Module Mono
/// </summary>
/// <typeparam name="T"> 类名称 Class Name </typeparam>
public class MgrBaseMono<T> : MonoBehaviour where T : MonoBehaviour
{
    /// <summary>
    /// 单例实例 Singleton Instance
    /// </summary>
    private static T mgr;

    /// <summary>
    /// 获取类单例对象
    /// Get Class Singleton Object
    /// </summary>
    /// <returns>类单例对象 Class Singleton Object</returns>
    public static T Mgr()
    {
        if(mgr == null) { 
                GameObject obj = new GameObject(typeof(T).ToString());
                mgr = obj.AddComponent<T>();
                GameObject.DontDestroyOnLoad(obj);
        }
        return mgr;
    }
}