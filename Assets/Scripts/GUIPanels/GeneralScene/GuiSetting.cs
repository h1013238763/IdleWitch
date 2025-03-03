using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GuiSetting : GuiBase
{
    private string setting_tab; // 设置标签 Setting tab page
    private bool listen_event;  // 是否监听事件 Whether to listen to events

    /// <summary>
    /// 显示 Show
    /// </summary>
    public override void Show()
    {
        listen_event = false;
        LocalizeTMPText();
        SetSliderValue();
        SetResolutionDropdown();
        SetFullScreenMode();
        ChangeSettingTab("btn_audio");
        listen_event = true;
    }

    /// <summary>
    /// 按钮点击事件 Button click event
    /// </summary>
    /// <param name="btn_name"> 按钮名称 Button name </param>
    protected override void OnButtonClick(string btn_name)
    {
        if(!listen_event) return;
        // 整体设置 General setting
        if(btn_name == "btn_confirm"){ ConfirmSetting(); }
        else if(btn_name == "btn_default"){ DefaultSetting(); }
        else if(btn_name == "btn_cancel"){ CancelSetting(); }

        // 设置标签 Setting tab
        else if(btn_name == "btn_audio"){ ChangeSettingTab("btn_audio"); }
        else if(btn_name == "btn_video"){ ChangeSettingTab("btn_video"); }
        else if(btn_name == "btn_game"){ ChangeSettingTab("btn_game"); }
        else if(btn_name == "btn_language"){ ChangeSettingTab("btn_language"); }

        // 语言设置 Language setting
        else if(btn_name == "language_en"){ MgrLocale.Mgr().SetLocale((int)LocaleID.en); }
        else if(btn_name == "language_zh"){ MgrLocale.Mgr().SetLocale((int)LocaleID.zh); }
    }

    /// <summary>
    /// 滑动条数值改变事件 Slider value change event
    /// </summary>
    /// <param name="slider_name"> 滑动条名称 Slider name </param>
    /// <param name="value"> 数值 Value </param>
    protected override void OnSliderValueChanged(string slider_name, float value)
    {
        if(!listen_event) return;
        // 音量设置 Volume setting
        if(slider_name.Contains("_vol")){ ChangeVolume(slider_name, value); }
    }

    /// <summary>
    /// 下拉框数值改变事件 Dropdown value change event
    /// </summary>
    /// <param name="dropdown_name"> 下拉框名称 Dropdown name </param>
    /// <param name="value"> 数值 Value </param>
    protected override void OnDropdownValueChanged(string dropdown_name, int value)
    {
        if(!listen_event) return;
        // 分辨率设置 Resolution setting
        if(dropdown_name == "drop_resolution"){  ChangeResolution(value); }
    }

    /// <summary>
    /// 开关状态改变事件 Toggle state change event
    /// </summary>
    /// <param name="toggle_name"> 开关名称 Toggle name </param>
    /// <param name="value"> 状态 State </param>
    protected override void OnToggleValueChanged(string toggle_name, bool value)
    {
        if(!listen_event) return;
        // 全屏模式 Full screen mode
        if(toggle_name.Contains("_screen")){ ChangeScreenMode(toggle_name, value); }
    }

    #region 整体设置 General setting
    // 点击确认设置 Click confirm setting
    private void ConfirmSetting()
    {
        // 保存设置 Save setting
        MgrEvent.Mgr().EventTrigger("SaveConfig");
        // 关闭设置 Close setting
        MgrGui.Mgr().HideGui("GuiSetting");
    }

    // 点击默认按钮 Click default button
    private void DefaultSetting()
    {
        // 恢复默认设置 Restore default settings
        MgrEvent.Mgr().EventTrigger("DefaultConfig");
        listen_event = false;
        SetSliderValue();
        SetResolutionDropdown();
        SetFullScreenMode();
        listen_event = true;
    }

    // 点击取消按钮 Click cancel button
    private void CancelSetting()
    {
        // 取消设置 Cancel setting
        MgrEvent.Mgr().EventTrigger("CancelConfig");
        // 关闭设置 Close setting
        MgrGui.Mgr().HideGui("GuiSetting");
    }
    
    // 更改设置标签 Change setting tab
    private void ChangeSettingTab(string tab)
    {
        setting_tab = tab;
        GetComponent<Image>("panel_audio").gameObject.SetActive(tab == "btn_audio");
        GetComponent<Image>("panel_video").gameObject.SetActive(tab == "btn_video");
        GetComponent<Image>("panel_game").gameObject.SetActive(tab == "btn_game");
        GetComponent<Image>("panel_language").gameObject.SetActive(tab == "btn_language");
    }

    #endregion


    #region 音乐设置
    // 更改音量 Change volume
    private void ChangeVolume(string slider_name, float value) 
    {
        string volume_text = ((int)(value * 100)).ToString();
        // 音量设置 Volume setting
        if(slider_name == "slider_master_vol")
        {
            MgrAudio.Mgr().ChangeMasterVolume(value);
            GetComponent<TextMeshProUGUI>("volume_value_master").text = volume_text;
        }
        else if(slider_name == "slider_bgm_vol")
        {
            MgrAudio.Mgr().ChangeVolume(AudioClass.Music, value);
            GetComponent<TextMeshProUGUI>("volume_value_bgm").text = volume_text;
        }
        else if(slider_name == "slider_sound_vol")
        {
            MgrAudio.Mgr().ChangeVolume(AudioClass.Sound, value);
            GetComponent<TextMeshProUGUI>("volume_value_sound").text = volume_text;
        }    
        else if(slider_name == "slider_envir_vol")
        {
            MgrAudio.Mgr().ChangeVolume(AudioClass.Environment, value);
            GetComponent<TextMeshProUGUI>("volume_value_envir").text = volume_text;
        }   
    }
    
    #endregion

    #region 画面设置
    // 更改分辨率 Change resolution
    private void ChangeResolution(int index)
    {
        MgrScreen.Mgr().SetResolution(index);
    }

    // 全屏模式 Full screen mode
    private void ChangeScreenMode(string toggle_name, bool value)
    {
        listen_event = false;

        if(toggle_name == "toggle_screen_full" && value)
            MgrScreen.Mgr().SetFullScreenMode(ScreenMode.FullScreen);
        else if(toggle_name == "toggle_screen_borderless" && value)
            MgrScreen.Mgr().SetFullScreenMode(ScreenMode.Borderless);
        else if(toggle_name == "toggle_screen_window" && value)
            MgrScreen.Mgr().SetFullScreenMode(ScreenMode.Windowed);

        SetFullScreenMode();

        listen_event = true;
    }

    #endregion

    // 游戏设置

    // 语言设置


    // 本地化设置文本 Localize setting text
    private void LocalizeTMPText(){
        // 基础按钮文本 Basic button text
        MgrLocale.Mgr().LocalizeTMPText(
            GetComponent<TextMeshProUGUI>("btn_text_confirm"), 
            "GuiSettingTable", 
            "btn_text_confirm");
        MgrLocale.Mgr().LocalizeTMPText(
            GetComponent<TextMeshProUGUI>("btn_text_default"), 
            "GuiSettingTable", 
            "btn_text_default");
        MgrLocale.Mgr().LocalizeTMPText(
            GetComponent<TextMeshProUGUI>("btn_text_cancel"), 
            "GuiSettingTable", 
            "btn_text_cancel");
        // 音量设置文本 Volume setting text
        MgrLocale.Mgr().LocalizeTMPText(
            GetComponent<TextMeshProUGUI>("volume_title_master"), 
            "GuiSettingTable", 
            "volume_title_master");
        MgrLocale.Mgr().LocalizeTMPText(
            GetComponent<TextMeshProUGUI>("volume_title_bgm"), 
            "GuiSettingTable", 
            "volume_title_bgm");
        MgrLocale.Mgr().LocalizeTMPText(
            GetComponent<TextMeshProUGUI>("volume_title_sound"), 
            "GuiSettingTable", 
            "volume_title_sound");
        MgrLocale.Mgr().LocalizeTMPText(
            GetComponent<TextMeshProUGUI>("volume_title_envir"), 
            "GuiSettingTable", 
            "volume_title_envir");
        // 画面设置文本 Video setting text
        MgrLocale.Mgr().LocalizeTMPText(
            GetComponent<TextMeshProUGUI>("drop_title_resolution"), 
            "GuiSettingTable", 
            "drop_title_resolution");
        MgrLocale.Mgr().LocalizeTMPText(
            GetComponent<TextMeshProUGUI>("text_screen_full"), 
            "GuiSettingTable", 
            "text_screen_full");
        MgrLocale.Mgr().LocalizeTMPText(
            GetComponent<TextMeshProUGUI>("text_screen_borderless"), 
            "GuiSettingTable", 
            "text_screen_borderless");
        MgrLocale.Mgr().LocalizeTMPText(
            GetComponent<TextMeshProUGUI>("text_screen_window"), 
            "GuiSettingTable", 
            "text_screen_window");
    }

    // 设置滑动条数值 Set slider value
    private void SetSliderValue(){
        // 音量设置 Volume setting
        GetComponent<Slider>("slider_master_vol").value = MgrAudio.Mgr().GetMasterVolume();
        GetComponent<Slider>("slider_bgm_vol").value = MgrAudio.Mgr().GetVolume(AudioClass.Music);
        GetComponent<Slider>("slider_sound_vol").value = MgrAudio.Mgr().GetVolume(AudioClass.Sound);
        GetComponent<Slider>("slider_envir_vol").value = MgrAudio.Mgr().GetVolume(AudioClass.Environment);
    }

    // 设置分辨率下拉框 Set resolution drop-down box
    private void SetResolutionDropdown(){
        // 分辨率下拉框 Resolution drop-down box
        TMP_Dropdown drop_resolution = GetComponent<TMP_Dropdown>("drop_resolution");
        drop_resolution.ClearOptions();
        List<string> resolution_list = MgrScreen.Mgr().GetResolutions();
        drop_resolution.AddOptions(resolution_list);
        drop_resolution.value = MgrScreen.Mgr().GetResolutionIndex();
    }

    // 设置全屏模式 Set full screen mode
    private void SetFullScreenMode(){
        // 全屏模式 Full screen mode
        ScreenMode mode = MgrScreen.Mgr().GetScreenMode();
        GetComponent<Toggle>("toggle_screen_full").isOn = mode == ScreenMode.FullScreen;
        GetComponent<Toggle>("toggle_screen_borderless").isOn = mode == ScreenMode.Borderless;
        GetComponent<Toggle>("toggle_screen_window").isOn = mode == ScreenMode.Windowed;
    }
}