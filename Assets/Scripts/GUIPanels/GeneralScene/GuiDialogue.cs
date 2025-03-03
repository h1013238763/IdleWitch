using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GuiDialogue : GuiBase
{
    protected override void OnButtonClick(string btn_name) {
        if( btn_name == "click_layer")
        {
            SetLine(MgrDialogue.Mgr().NextLine());
        }
    }

    public void SetLine(string line)
    {
        GetComponent<TextMeshProUGUI>("text").text = line;
        GetComponent<TextMeshProUGUI>("text").text += "\n\n Click to continue...";
    }

    public void SetPosition(Vector2 position)
    {
        GetComponent<RectTransform>().anchoredPosition = position;
    }
}