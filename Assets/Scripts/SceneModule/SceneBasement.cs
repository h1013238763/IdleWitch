using UnityEngine;

public class SceneBasement : MonoBehaviour {
    void Start()
    {
        Debug.Log("SceneBasement: start");
        MgrGui.Mgr().ShowGui<GuiKeyTip>("GuiKeyTip", 0);

        if(Player.Mgr().is_initial)
            Player.Mgr().SetPlayer();
    }
}

