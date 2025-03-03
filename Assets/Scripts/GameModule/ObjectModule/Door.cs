using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour, IInteractable
{
    public string curr_scene;
    void Start() {
        curr_scene = SceneManager.GetActiveScene().name;
    }

    public void Interact()
    {
        Player.Mgr().interactable = false;
        MgrEvent.Mgr().AddEventListener("EnterLoadingDone", ChangeScene);
        MgrGui.Mgr().ShowGui<GuiLoading>("GuiLoading", 0);
    }

    public void ChangeScene() {
        if (curr_scene == "SceneBasement") {
            MgrScene.Mgr().LoadSceneAsync("SceneTown", () => {
                Player.Mgr().SetPlayer();
                Player.Mgr().interactable = true;
                MgrGui.Mgr().GetGui<GuiLoading>("GuiLoading").Exit();
            });
        } else if (curr_scene == "SceneTown") {
            MgrScene.Mgr().LoadSceneAsync("SceneBasement", () => {
                Player.Mgr().SetPlayer(4.31f);
                Player.Mgr().interactable = true;
                MgrGui.Mgr().GetGui<GuiLoading>("GuiLoading").Exit();
            });
        }
        MgrEvent.Mgr().RemoveEventListener("EnterLoadingDone", ChangeScene);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            Player.Mgr().interactable_object = this;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            Player.Mgr().interactable_object = null;
        }
    }

    void OnMouseDown() {
        if( !Player.Mgr().interactable) return;
        Player.Mgr().Interact();
    }
}