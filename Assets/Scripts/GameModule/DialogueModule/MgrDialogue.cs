using UnityEngine;

public class MgrDialogue : MgrBase<MgrDialogue>
{
    public Dialogue current_dialogue;

    public void StartDialogue(Dialogue dialogue)
    {
        Player.Mgr().interactable = false;
        current_dialogue = dialogue;
        if (current_dialogue == null)
            return;
        MgrGui.Mgr().ShowGui<GuiDialogue>("GuiDialogue", 0, (gui) => {
            gui.SetLine(current_dialogue.GetLine());
        });
    }

    public string NextLine()
    {
        string line = current_dialogue.GetNextLine();
        Debug.Log(line);
        if (line == null)
            EndDialogue();
        return line;
    }

    public void EndDialogue()
    {
        Player.Mgr().interactable = true;
        current_dialogue = null;
        MgrGui.Mgr().HideGui("GuiDialogue");
    }
}