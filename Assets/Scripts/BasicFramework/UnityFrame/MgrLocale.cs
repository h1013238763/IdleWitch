using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 本地化管理器 Localization Manager
/// </summary>
public class MgrLocale: MgrBase<MgrLocale>, ISaveData
{
    private LocaleConfig saved_config = new LocaleConfig(); // 保存的语言配置 Saved language configuration
    private LocaleConfig curr_config = new LocaleConfig();   // 当前的语言配置 Current language configuration

    // tmp字体路径 tmp font path
    protected const string DEFAULT_FONT = "Font/TMPFont/NotoSansCJKsc-Regular SDFop";
    
    /// <summary>
    /// 获取所有语言列表 Get all language list
    /// </summary>
    /// <returns></returns>
    public List<Locale> GetLocaleList()
    {
        return LocalizationSettings.AvailableLocales.Locales;
    }

    /// <summary>
    /// 设置语言 Set language
    /// </summary>
    /// <param name="locale_id">语言ID language ID</param> 
    public void SetLocale(int locale_id)
    {
        MgrMono.Mgr().StartCoroutine(ISetLocale(locale_id));
    }

    // 设置语言具体执行 Set language execution
    // <param name="locale_id">语言ID language ID</param>
    private IEnumerator ISetLocale(int locale_id)
    {
        while(!LocalizationSettings.InitializationOperation.IsDone){
            yield return LocalizationSettings.InitializationOperation;
        }
        if(locale_id >= 0 && locale_id < LocalizationSettings.AvailableLocales.Locales.Count){
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[locale_id];
            curr_config.locale_id = locale_id;
        }
    }

    /// <summary>
    /// 本地化目标对象 Localize target object
    /// </summary>
    /// <param name="obj">目标对象 target object</param>
    /// <param name="table_name">表名 table name</param>
    /// <param name="key_name">键名 key name</param>
    /// <param name="update_event">更新事件 update event</param>
    public void ObjectLocalize(GameObject obj, string table_name, string key_name, UnityAction<string> update_event)
    {
        // 添加字符本地化事件组件 Add string localization event component
        LocalizeStringEvent localize_event = obj.GetComponent<LocalizeStringEvent>();
        if(localize_event == null)
            localize_event = obj.AddComponent<LocalizeStringEvent>();
        // 设置组件对应字符 Set component corresponding string
        localize_event.StringReference = new LocalizedString{
            TableReference = table_name,
            TableEntryReference = key_name
        };
        // 设置UpdateString事件 Set UpdateString event
        localize_event.OnUpdateString.AddListener(update_event);
    }

    /// <summary>
    /// 本地化Text组件 
    /// Localize Text component
    /// </summary>
    /// <param name="text_obj">Text组件 Text component</param>
    /// <param name="table_name">表名 table name</param>
    /// <param name="key_name">键名 key name</param>
    public void LocalizeText(Text text_obj, string table_name, string key_name)
    {     
        try{
            ObjectLocalize(text_obj.gameObject, table_name, key_name, (str) => {
                text_obj.text = str;
            });
            text_obj.text = GetLocaleText(table_name, key_name);
        }catch(Exception e){
            Debug.LogWarning("LocalizeText Error: " + e.Message);
        }
        
    }

    /// <summary>
    /// 本地化TMPText组件 
    /// Localize TMPText component
    /// </summary>
    /// <param name="text_obj">TMPText组件 TMPText component</param>
    /// <param name="table_name">表名 table name</param>
    /// <param name="key_name">键名 key name</param>
    public void LocalizeTMPText(TextMeshProUGUI text_obj, string table_name, string key_name)
    {
        try{
            text_obj.font = Resources.Load<TMP_FontAsset>(DEFAULT_FONT);
        
            ObjectLocalize(text_obj.gameObject, table_name, key_name, (str) => {
                text_obj.text = str;
            });
            text_obj.text = GetLocaleText(table_name, key_name);
        }catch(Exception e){
            Debug.LogWarning("LocalizeTMPText Error: " + e.Message);
        }
        
    }

    /// <summary>
    /// 获取特定的本地化文本 Get specific localized text 
    /// </summary>
    /// <param name="table_name"> 表名 Table name</param>
    /// <param name="key_name"> 键名 Key name</param>
    /// <returns> 本地化文本 Localized text</returns>
    public string GetLocaleText(string table_name, string key_name)
    {
        // 获取表格集合 Get the table collection
        var table_collect = LocalizationSettings.StringDatabase.GetTable(table_name);
        if (table_collect == null)
        {
            Debug.LogWarning($"Table {table_name} not found.");
            return null;
        }
        // 获取表格条目 Get the table entry
        var entry = table_collect.GetEntry(key_name);
        if (entry == null)
        {
            Debug.LogWarning($"Key {key_name} not found in table {table_name}.");
            return null;
        }
        // 返回本地化文本 Return the localized text
        return entry.GetLocalizedString();
    }

    // 本地化配置数据 Localize configuration data
    private class LocaleConfig
    {
        public int locale_id;

        public LocaleConfig()
        {
            locale_id = (int)LocaleID.en;
        }
    }

    /// <summary>
    /// 新建配置 New Configuration data
    /// </summary>
    public void NewData()
    {
        saved_config = new LocaleConfig();
        curr_config = new LocaleConfig();
    }

    /// <summary>
    /// 保存配置为json字符串 Save Configuration data as json string
    /// </summary>
    public string SaveData()
    {
        saved_config.locale_id = curr_config.locale_id;
        return MgrJson.Mgr().ObjectToJson<LocaleConfig>(saved_config);
    }

    /// <summary>
    /// 从json字符串加载配置 Load Configuration data from json string
    /// </summary>
    /// <param name="config_json"> 配置json字符串 Configuration json string</param> 
    public void LoadData(string config_json)
    {
        saved_config = MgrJson.Mgr().JsonToObject<LocaleConfig>(config_json);
        curr_config.locale_id = saved_config.locale_id;
    }

    /// <summary>
    /// 应用配置 Apply Configuration data
    /// </summary> 
    public void ApplyData()
    {
        SetLocale(curr_config.locale_id);
    }
}

public enum LocaleID
{
    zh = 0,
    en = 1
}