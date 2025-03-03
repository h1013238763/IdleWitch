/// <summary>
/// 用于配置数据的接口 Interface for configuration data handler
/// </summary>
public interface ISaveData
{
    /// <summary>
    /// 新建配置 New Configuration data
    /// </summary>
    void NewData();

    /// <summary>
    /// 保存配置为json字符串 Save Configuration data as json string
    /// </summary>
    /// <returns></returns>
    string SaveData();

    /// <summary>
    /// 从json字符串加载配置 Load Configuration data from json string
    /// </summary>
    /// <param name="config"></param>
    void LoadData(string config);

    /// <summary>
    /// 应用配置 Apply Configuration data
    /// </summary>
    void ApplyData();
}