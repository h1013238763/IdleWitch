using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class SceneBase : MonoBehaviour
{   
    protected ScenePhase phase;
    protected List<bool> enter_states = new List<bool>();
    protected List<bool> exit_states = new List<bool>(); 

    protected virtual void Start()
    {
        phase = ScenePhase.WaitForLoadAnime;
    }

    protected virtual void FixedUpdate()
    {
        if(phase == ScenePhase.WaitForLoadAnime){ WaitForLoadAnime(); }
        else if(phase == ScenePhase.StartEnterAction){ StartEnterAction(); }
        else if(phase == ScenePhase.WaitForEnterAction){ WaitForEnterAction(); }
        else if(phase == ScenePhase.EnterActionDone){ EnterActionDone(); }
        
    }

    protected virtual void EnterAction()
    {

    }

    protected virtual void OnDisable()
    {
        // 注册退出场景事件 Register exit scene event
        MgrEvent.Mgr().AddEventListener("EnterLoadingDone", ExitAction);
        // 开始加载等待界面 Start loading waiting interface
        MgrGui.Mgr().ShowGui<GuiLoading>("GuiLoading", 4);
    }

    protected virtual void ExitAction()
    {
        
    }

    /// <summary>
    /// 查询加载动画状态 
    /// Check loading animation status
    /// </summary>    
    protected virtual void WaitForLoadAnime()
    {
        if(MgrGui.Mgr().GetGui<GuiLoading>("GuiLoading") == null){ return; }
        if(MgrGui.Mgr().GetGui<GuiLoading>("GuiLoading").ready_for_load)
            phase = ScenePhase.StartEnterAction;
    }

    /// <summary>
    /// 开始进入动作
    /// Start entering action 
    /// </summary>
    protected virtual void StartEnterAction()
    {
        EnterAction();
        phase = ScenePhase.WaitForEnterAction;
    }

    /// <summary>
    /// 等待进入动作完成 wait for the entry action to complete
    /// </summary>
    protected virtual void WaitForEnterAction()
    {
        if(enter_states.Count == 0 || enter_states.TrueForAll((state) => state == true))
            phase = ScenePhase.EnterActionDone;
    }

    /// <summary>
    /// 进入动作完成 Enter action completed
    /// </summary> 
    protected virtual void EnterActionDone()
    {
        MgrGui.Mgr().GetGui<GuiLoading>("GuiLoading").Exit();
        phase = ScenePhase.InScene;
    }
}

public enum ScenePhase
{
    WaitForLoadAnime,
    StartEnterAction,
    WaitForEnterAction,
    EnterActionDone,
    InScene,
    StartExitAction,
    WaitForExitAction,
    ExitActionDone
}