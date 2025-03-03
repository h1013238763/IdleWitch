using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 资源加载管理器
/// Resource Load Manager
/// </summary>
public class MgrResource: MgrBase<MgrResource>
{
    /// <summary>
    /// 加载资源
    /// load resource
    /// </summary>
    /// <param name="name">资源路径 resource path</param>
    /// <typeparam name="T">资源类型 resource type</typeparam>
    /// <returns>资源 resource</returns>
    public T Load<T>(string name) where T : Object
    {
        T res = Resources.Load<T>(name);
        return res;
    }

    /// <summary>
    /// 异步加载资源
    /// load resource async
    /// </summary>
    /// <param name="name">资源路径 resource path</param>
    /// <param name="callback">回调函数 callback function</param>
    /// <typeparam name="T">资源类型 resource type</typeparam>
    public void LoadAsync<T>(string name, UnityAction<T> callback) where T : Object
    {
        MgrMono.Mgr().StartCoroutine(ILoadAsync<T>(name, callback));
    }

    // 异步加载资源具体执行
    // load resource async execution
    private IEnumerator ILoadAsync<T>(string name, UnityAction<T> callback) where T : Object
    {
        ResourceRequest res = Resources.LoadAsync<T>(name);
        yield return res;

        callback( res.asset as T );
    }

    /// <summary>
    /// 清理内存
    /// clear memory
    /// </summary>
    public void ClearMemory(){
        MgrMono.Mgr().StartCoroutine(IClearMemory());
    }

    // 清理内存具体执行
    // clear memory execution
    private IEnumerator IClearMemory(){
        yield return Resources.UnloadUnusedAssets();
        System.GC.Collect();
    }
}