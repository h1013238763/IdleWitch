using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GuiConfirm : GuiBase
{
    public override void Show()
    {
        MgrLocale.Mgr().LocalizeTMPText(
            GetComponent<TextMeshProUGUI>("btn_text_confirm"), 
            "GuiSettingTable", 
            "btn_text_confirm");
        MgrLocale.Mgr().LocalizeTMPText(
            GetComponent<TextMeshProUGUI>("btn_text_cancel"), 
            "GuiSettingTable", 
            "btn_text_cancel");
    }

    protected override void OnButtonClick(string btn_name)
    {
        MgrAudio.Mgr().PlayAudio(AudioClass.Sound, "ClickBtn", false);
        if (btn_name == "btn_confirm") { Confirm(); }
        else if (btn_name == "btn_cancel") { Cancel(); }
    }

    public void SetConfirmInfo(string info)
    {
        GetComponent<TextMeshProUGUI>("text_info").text = info;
    }

    private void Confirm()
    {
        MgrEvent.Mgr().EventTrigger("GuiConfirm: Confirm");
        MgrGui.Mgr().HideGui("GuiConfirm");
    }

    private void Cancel()
    {
        MgrGui.Mgr().HideGui("GuiConfirm");
    }
}