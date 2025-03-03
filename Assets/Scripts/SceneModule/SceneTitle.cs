using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTitle : SceneBase 
{
    protected override void EnterAction()
    {
        enter_states.Add(false);
        MgrGui.Mgr().ShowGui<GuiTitle>("GuiTitle", 0, (gui) => {
            enter_states[0] = true;
        });
    }
}