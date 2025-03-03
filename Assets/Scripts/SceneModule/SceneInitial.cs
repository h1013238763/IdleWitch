using UnityEngine;

public class SceneInitial : MonoBehaviour {
    
    private void Awake()
    {
        MgrConfig.Mgr().GameAwake();
    }
}