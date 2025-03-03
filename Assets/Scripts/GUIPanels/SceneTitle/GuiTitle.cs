using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuiTitle : GuiBase
{
    public override void Show()
    {
        LocalizeTMPText();

        GetComponent<TextMeshProUGUI>("text_version").text = "Version: " + Application.version;
    }

    protected override void OnButtonClick(string btn_name)
    {
        MgrAudio.Mgr().PlayAudio(AudioClass.Sound, "ClickBtn", false);
        if (btn_name == "btn_continue") { ContinueGame(); }
        else if (btn_name == "btn_start") { StartGame(); }
        else if (btn_name == "btn_load") { LoadGame(); }
        else if (btn_name == "btn_setting") { Setting(); }
        else if (btn_name == "btn_exit") { ExitGame(); }        
    }

    private void ContinueGame()
    {
        MgrGame.Mgr().LoadGame(0);
    }

    private void StartGame()
    {
        MgrGame.Mgr().LoadGame(-1);
    }

    private void LoadGame()
    {
        MgrGui.Mgr().ShowGui<GuiLoadGame>("GuiLoadGame", 1);
    }

    /// <summary>
    /// 按下设置按钮 开启设置gui Press the setting button to open the setting gui 
    /// </summary>
    private void Setting()
    {
        MgrGui.Mgr().ShowGui<GuiSetting>("GuiSetting", 1);
    }

    /// <summary>
    /// 按下退出按钮 退出游戏 Press the exit button to exit the game
    /// </summary>
    private void ExitGame()
    {
        Application.Quit();
    }

    // 本地化文本 Localize text
    private void LocalizeTMPText()
    {
        MgrLocale.Mgr().LocalizeTMPText(
            GetComponent<TextMeshProUGUI>("btn_text_continue"), 
            "GuiTitleTable", 
            "btn_text_continue");
        MgrLocale.Mgr().LocalizeTMPText(
            GetComponent<TextMeshProUGUI>("btn_text_start"), 
            "GuiTitleTable", 
            "btn_text_start");
        MgrLocale.Mgr().LocalizeTMPText(
            GetComponent<TextMeshProUGUI>("btn_text_load"), 
            "GuiTitleTable", 
            "btn_text_load");
        MgrLocale.Mgr().LocalizeTMPText(
            GetComponent<TextMeshProUGUI>("btn_text_setting"), 
            "GuiTitleTable", 
            "btn_text_setting");
        MgrLocale.Mgr().LocalizeTMPText(
            GetComponent<TextMeshProUGUI>("btn_text_exit"), 
            "GuiTitleTable", 
            "btn_text_exit");
    }
}