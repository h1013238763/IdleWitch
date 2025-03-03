using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景管理器 Scene Manager
/// </summary>
public class MgrScene: MgrBase<MgrScene>
{
    /// <summary>
    /// 加载场景
    /// load scene
    /// </summary>
    /// <param name="scene_name">场景名称 scene name</param>
    /// <param name="callback">回调函数 callback function</param>
    public void LoadScene(string scene_name, UnityAction callback = null)
    {
        SceneManager.LoadScene(scene_name);

        if(callback != null)
            callback();
    }

    /// <summary>
    /// 异步加载场景
    /// load scene async
    /// </summary>
    /// <param name="scene_name">场景名称 scene name</param>
    /// <param name="callback">回调函数 callback function</param>
    public void LoadSceneAsync(string scene_name, UnityAction callback = null)
    {
        MgrMono.Mgr().StartCoroutine(ILoadSceneAsync(scene_name, callback));
    }
    
    // 异步加载场景具体执行
    // load scene async execution
    // <param name="scene_name">场景名称 scene name</param>
    // <param name="callback">回调函数 callback function</param>
    private IEnumerator ILoadSceneAsync(string scene_name, UnityAction callback = null)
    {
        AsyncOperation async_operation = SceneManager.LoadSceneAsync(scene_name);

        // 等待加载完成 Wait for loading to complete
        while (!async_operation.isDone)     // 向外分发进度条 Loading progress bar
        {
            MgrEvent.Mgr().EventTrigger("SceneLoadingUpdate", async_operation.progress);
            yield return async_operation.progress;
        }

        if(callback != null)
            callback();
    }
}