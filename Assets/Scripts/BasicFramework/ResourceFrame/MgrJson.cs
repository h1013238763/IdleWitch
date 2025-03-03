using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

/// <summary>
/// Json文件管理器 
/// Json File Manager
/// </summary>
public class MgrJson: MgrBase<MgrJson>{

    // 应用持久化数据路径 Application persistent data path
    private string app_presist_data_path = Application.persistentDataPath;

    /// <summary>
    /// 写入对象为文件
    /// Write Object to File
    /// </summary>
    /// <param name="obj">写入对象 write object</param>
    /// <param name="file_name">文件名+后缀 file name + suffix</param>
    /// <param name="file_path">写入路径(默认为空) write path (default is empty)</param>
    /// <typeparam name="T">写入对象类型 write object type</typeparam>
    public void WriteFile<T>(T obj, string file_name, string file_path = null)
    {
        string path = app_presist_data_path + "/" + file_path + file_name;
        string content = ObjectToJson<T>(obj);

        try{
            if(!File.Exists(path)){
                File.Create(path).Dispose();
            }
            File.WriteAllText(path, content);
        }
        catch(Exception e){ 
            Debug.LogWarning(e); 
        }
    }

    /// <summary>
    /// 读取文件为对象
    /// Read File to Object
    /// </summary>
    /// <param name="file_name">文件名+后缀 file name + suffix</param>
    /// <param name="file_path">读取路径(默认为空) read path (default is empty)</param>
    /// <typeparam name="T">读取对象类型 read object type</typeparam>
    /// <returns>读取对象 read object</returns>
    public T ReadFile<T>(string file_name, string file_path = null)
    {
        string path = app_presist_data_path + "/" + file_path + file_name;

        try{
            string content = File.ReadAllText(path);
            return JsonToObject<T>(content);
        }
        catch(Exception e){
            Debug.LogWarning(e);
            return default(T);
        }
    }

    /// <summary>
    /// 删除文件 delete file
    /// </summary>
    /// <param name="file_name"> 文件名+后缀 file name + suffix</param>
    /// <param name="file_path"> 删除路径(默认为空) delete path (default is empty)</param>
    public void DeleteFile(string file_name, string file_path = null)
    {
        string path = app_presist_data_path + "/" + file_path + file_name;

        try{
            if(File.Exists(path)){
                File.Delete(path);
            }
        }
        catch(Exception e){
            Debug.LogWarning(e);
        }
    }

    /// <summary>
    /// 返回文件夹下所有文件路径 return all file paths under the folder
    /// </summary>
    /// <param name="folder_path">文件夹路径 folder path</param>
    /// <returns> 文件路径列表 file path list</returns>
    public List<string> ReadFolder(string folder_path)
    {
        string path = app_presist_data_path + "/" + folder_path;
        List<string> files = new List<string>();

        try{
            if(Directory.Exists(path)){
                string[] file_paths = Directory.GetFiles(path);
                for(int i = 0; i < file_paths.Length; i ++){
                    files.Add(file_paths[i].Replace(app_presist_data_path + "/", ""));
                }
            }
        }
        catch(Exception e){
            Debug.LogWarning(e);
        }
        return files;
    }

    /// <summary>
    /// 转换Json为对象
    /// Convert Json to Object
    /// </summary>
    /// <param name="text">Json字符串 Json string</param>
    /// <typeparam name="T">对象类型 object type</typeparam>
    /// <returns>转换结果对象 convert result object</returns>
    public T JsonToObject<T>(string text)
    {
        try{
            var obj = JsonConvert.DeserializeObject<T>(text);
            return obj;
        }
        catch(Exception e){
            Debug.LogWarning(e);
            return default(T);
        }
    }

    /// <summary>
    /// 转换对象为Json
    /// Convert Object to Json
    /// </summary>
    /// <typeparam name="T">对象类型 object type</typeparam>
    /// <param name="obj">对象 object</param>
    /// <returns>转换结果Json convert result Json</returns>
    public string ObjectToJson<T>(T obj)
    {
        try{
            var json = JsonConvert.SerializeObject(obj);
            return json;
        }
        catch(Exception e){
            Debug.LogWarning(e);
            return null;
        }
    }
}