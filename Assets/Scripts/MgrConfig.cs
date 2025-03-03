using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

/// <summary>
/// 游戏配置管理器 Game configuration manager
/// </summary>
public class MgrConfig : MgrBase<MgrConfig>
{
    private Dictionary<string, string> config_dict; // 配置字典 Config Dictionary
    private const string CONFIG_FILE_REF = "GameConfig.config"; // 配置文件路径 Config file path
    private const string FIRST_SCENE = "SceneTitle"; // 第一个场景 First scene
    private bool is_initialized = false; // 是否已初始化 Is initialized

    // 需要被配置的管理器列表 Manager list that needs to be configured
    private List<ISaveData> mgr_to_config = new List<ISaveData>(){
        MgrAudio.Mgr(),
        MgrLocale.Mgr(),
        MgrScreen.Mgr()
    };

    /// <summary>
    /// 游戏启动时初始化事件 Game startup initialization event
    /// </summary>
    public void GameAwake()
    {
        if(is_initialized) return;
        // 开始加载等待界面 Start loading waiting interface
        MgrEvent.Mgr().AddEventListener("EnterLoadingDone", () => {
            MgrMono.Mgr().StartCoroutine(GameInitial());
        });
        MgrGui.Mgr().ShowGui<GuiLoading>("GuiLoading", 4);        
    }

    // 游戏初始化 Game initialization
    private IEnumerator GameInitial()
    {
        yield return CodeFrameInitialized();    // 初始化代码框架 Initialize code framework
        yield return LoadConfigAsync();         // 读取配置文件 Read configuration file
        yield return RegisterGlobleEvent();     // 注册全局事件 Register global events
        yield return GameDataInitialize();      // 初始化游戏数据 Initialize game data

        is_initialized = true;
        MgrScene.Mgr().LoadScene(FIRST_SCENE);
        MgrEvent.Mgr().Clear("EnterLoadingDone");
    }


    // 初始化代码框架 Initialize code framework
    private IEnumerator CodeFrameInitialized()
    {
        DOTween.Init(); yield return null;
        // 初始化资源框架 Initialize resource framework
        MgrMono.Mgr(); yield return null;
        MgrJson.Mgr(); yield return null;
        MgrPool.Mgr(); yield return null;
        MgrResource.Mgr(); yield return null;
        // 初始化Unity功能框架 Initialize Unity functional framework
        MgrAudio.Mgr(); yield return null;
        MgrEvent.Mgr(); yield return null;
        MgrGui.Mgr(); yield return null;
        MgrLocale.Mgr(); yield return null;
        MgrScene.Mgr(); yield return null;
    }

    // 注册全局事件 Register global events
    private IEnumerator RegisterGlobleEvent()
    {
        // 注册更新配置文件事件 Register update configuration file event
        MgrEvent.Mgr().AddEventListener("SaveConfig", SaveConfig);
        MgrEvent.Mgr().AddEventListener("DefaultConfig", NewConfig);
        MgrEvent.Mgr().AddEventListener("CancelConfig", LoadConfig);
        yield return null;
    }

    // 初始化游戏数据 Initialize game data
    private IEnumerator GameDataInitialize()
    {
        // 读取游戏存档表 Read game saves
        yield return MgrGame.Mgr().ReadSavesAsync();
    }

    /// <summary>
    /// 创建新的配置 Create new configuration
    /// </summary>
    public void NewConfig()
    {
        MgrMono.Mgr().StartCoroutine(NewConfigAsync());
    }

    // 创建新的配置文件 Create a new configuration file
    private IEnumerator NewConfigAsync()
    {
        config_dict = MgrJson.Mgr().ReadFile<Dictionary<string, string>>(CONFIG_FILE_REF);
        if(config_dict == null) config_dict = new Dictionary<string, string>();

        foreach (ISaveData mgr in mgr_to_config)
        {
            mgr.NewData();
            config_dict[mgr.GetType().Name] = mgr.SaveData();
            mgr.ApplyData();
            yield return null;
        }
    }

    /// <summary>
    /// 读取并应用配置数据 Read and apply configuration data
    /// </summary>
    public void LoadConfig()
    {
        MgrMono.Mgr().StartCoroutine(LoadConfigAsync());
    }

    // 读取配置文件 Read configuration file
    private IEnumerator LoadConfigAsync()
    {
        config_dict = MgrJson.Mgr().ReadFile<Dictionary<string, string>>(CONFIG_FILE_REF);
        if(config_dict == null) config_dict = new Dictionary<string, string>();
        // 遍历所有管理器，读取配置 Load configuration for all managers
        foreach (ISaveData mgr in mgr_to_config)
        {
            try{
                mgr.LoadData(config_dict[mgr.GetType().Name]);
            }catch(Exception e){ // 读取失败，新建配置文件 Read failed, create new configuration file
                Debug.LogWarning("ConfigMgr.ReadConfig Error: " + e.Message + 
                    ", Mgr: " + mgr.GetType().Name);
                mgr.NewData();
                config_dict[mgr.GetType().Name] = mgr.SaveData();
                mgr.LoadData(config_dict[mgr.GetType().Name]);
            }
            mgr.ApplyData();
            yield return null;
        }
        SaveConfig();
    }

    /// <summary>
    /// 读取全部游戏存档 Read all game saves
    /// </summary>
    public void SaveConfig()
    {
        MgrMono.Mgr().StartCoroutine( MgrGame.Mgr().ReadSavesAsync() );
    }
}