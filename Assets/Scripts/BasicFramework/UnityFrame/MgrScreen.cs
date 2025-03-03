using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 画面配置管理器 Screen configuration manager
/// </summary>
public class MgrScreen : MgrBase<MgrScreen>, ISaveData
{
    private ScreenConfig saved_config = new ScreenConfig();  // 保存的画面配置 Saved screen configuration
    private ScreenConfig curr_config = new ScreenConfig();   // 当前的画面配置 Current screen configuration

    /// <summary>
    /// 获取全屏模式 Get full screen mode
    /// </summary>
    /// <returns> 全屏模式 Screen mode </returns>
    public ScreenMode GetScreenMode() { 
        return curr_config.screen_mode; 
    }

    /// <summary>
    /// 设置全屏模式 Set full screen mode
    /// </summary>
    /// <param name="mode"> 屏幕模式 Screen mode </param>
    public void SetFullScreenMode(ScreenMode mode)
    {
        curr_config.screen_mode = mode;
        switch (mode)
        {
            case ScreenMode.FullScreen:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case ScreenMode.Windowed:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case ScreenMode.Borderless:
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                break;
        }
        SetResolution(curr_config.resolution_index);
    }

    /// <summary>
    /// 获取所有可用分辨率 Get all available resolutions
    /// </summary>
    /// <returns> 分辨率列表 Resolution list </returns>
    public List<string> GetResolutions()
    {
        List<string> res = new List<string>();
        foreach (Resolution r in Screen.resolutions)
        {
            res.Add(r.width + " x " + r.height);
        }
        return res;
    }

    /// <summary>
    /// 获取分辨率索引 Get current resolution index
    /// </summary>
    /// <returns> 分辨率索引 Resolution index </returns>
    public int GetResolutionIndex()
    {
        return curr_config.resolution_index;
    }

    /// <summary>
    /// 设置分辨率 Set resolution
    /// </summary>
    /// <param name="index"> 分辨率索引 Resolution index </param>
    public void SetResolution(int index)
    {
        curr_config.resolution_index = index;
        Screen.SetResolution(
            Screen.resolutions[index].width, 
            Screen.resolutions[index].height, 
            Screen.fullScreen);
    }

    /// <summary>
    /// 画面配置数据 Screen configuration data
    /// </summary>
    private class ScreenConfig
    {
        public ScreenMode screen_mode;       // 屏幕模式 Screen mode
        public int resolution_index;        // 分辨率索引 Resolution index

        public ScreenConfig()
        {
            screen_mode = ScreenMode.Borderless;
            for(int i = 0; i < Screen.resolutions.Length; i++)
            {
                if (Screen.resolutions[i].width == Screen.currentResolution.width &&
                    Screen.resolutions[i].height == Screen.currentResolution.height)
                {
                    resolution_index = i;
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 新建画面数据 New screen data
    /// </summary>
    public void NewData()
    {
        saved_config = new ScreenConfig();
        curr_config = new ScreenConfig();
    }

    /// <summary>
    /// 保存画面数据 Save screen data
    /// </summary>
    /// <returns> 画面数据Json screen data Json </returns>
    public string SaveData()
    {
        saved_config.screen_mode = curr_config.screen_mode;
        saved_config.resolution_index = curr_config.resolution_index;
        return MgrJson.Mgr().ObjectToJson<ScreenConfig>(saved_config);
    }

    /// <summary>
    /// 读取画面数据 Read screen data
    /// </summary>
    /// <param name="json_data"> 画面数据Json screen data Json </param>
    public void LoadData(string json_data)
    {
        saved_config = MgrJson.Mgr().JsonToObject<ScreenConfig>(json_data);
        curr_config.screen_mode = saved_config.screen_mode;
        curr_config.resolution_index = saved_config.resolution_index;
        SetFullScreenMode(saved_config.screen_mode);
    }

    /// <summary>
    /// 应用画面数据 Apply screen data
    /// </summary>
    public void ApplyData()
    {
        SetFullScreenMode(curr_config.screen_mode);
    }
}

/// <summary>
/// 屏幕模式 Screen mode
/// </summary>
public enum ScreenMode
{
    FullScreen,
    Windowed,
    Borderless
}