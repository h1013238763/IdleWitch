using UnityEngine;

public class TownCore : MonoBehaviour, IInteractable
{
    public GameObject ball = null;

    void Start() {
        ball = transform.Find("Circle").gameObject;
        ball.SetActive(Player.Mgr().has_placed);
    }

    public void Interact()
    {
        if(Player.Mgr().has_core == true) {
            Player.Mgr().has_placed = true;
            Player.Mgr().has_core = false;
            ball.SetActive(true);
        }
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