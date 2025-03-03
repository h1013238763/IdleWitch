using UnityEngine;

public class Servant : MonoBehaviour, IInteractable
{
    public Dialogue dialogue_start_quest;
    public Dialogue dialogue_complete_quest;

    public void Start() {
        dialogue_start_quest = new Dialogue();
        dialogue_start_quest.lines.Add("My lord, I am your faithful servant.");
        dialogue_start_quest.lines.Add("You have finally awakened from your slumber.");
        dialogue_start_quest.lines.Add("Our floating island has stopped working for a long time.");
        dialogue_start_quest.lines.Add("It needs its core in order to go elsewhere.");
        dialogue_start_quest.lines.Add("This is a core I have kept for a long time.");
        dialogue_start_quest.lines.Add("Please take it to the lower levels to energize the Floating Island.");
        dialogue_start_quest.lines.Add("Only you have such an honorable right to control the hollow island.");

        dialogue_complete_quest = new Dialogue();
        dialogue_complete_quest.lines.Add("The floating island has been activated");
        dialogue_complete_quest.lines.Add("I am ready to serve you again on this journey.");
        dialogue_complete_quest.lines.Add("Please give the order to depart.");
    }

    public void Interact()
    {
        if(!Player.Mgr().has_core && !Player.Mgr().has_placed){
            MgrDialogue.Mgr().StartDialogue(dialogue_start_quest);
            Player.Mgr().has_core = true;
        }
        if(Player.Mgr().has_placed){
            dialogue_complete_quest.current_line = 0;
            MgrDialogue.Mgr().StartDialogue(dialogue_complete_quest);
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