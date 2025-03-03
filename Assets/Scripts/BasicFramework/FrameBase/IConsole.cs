
/// <summary>
/// 控制台接口 Console interface
/// </summary>
public interface IConsole
{
    /// <summary>
    /// 注册主命令至控制台 Register the main command to the console
    /// </summary>
    /// <param name="command_class"> 主命令语句 Main command statement </param>
    void RegisterCommand(string command_class);

    /// <summary>
    /// 删除命令至控制台 Unregister the main command to console
    /// </summary>
    /// <param name="command_class"> 主命令语句 Main command statement </param>
    void UnregisterCommand(string command_class);

    /// <summary>
    /// 处理命令 Handle command
    /// </summary>
    /// <param name="command"> 命令语句 Command statement </param>
    void HandleCommand(string[] command);
}