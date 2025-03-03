using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;

public class MgrGame : MgrBaseMono<MgrGame>
{
    private const string SAVE_PATH = "GameSaves/";
    private const string SAVE_EXT = ".savedata";

    private List<ISaveData> mgr_to_save;

    private GameSave current_save;
    private Dictionary<string, GameSave> game_saves;
    private GameSave recent_save;

    public void SaveGame()
    {
        
    }

    public void LoadGame(int save_index = 0)
    {
        
    }

    /// <summary>
    /// 读取游戏存档 Read game saves
    /// </summary>
    /// <returns> IEnumerator </returns>
    public IEnumerator ReadSavesAsync()
    {
        // 读取存档路径 Read save path
        List<string> save_paths = MgrJson.Mgr().ReadFolder(SAVE_PATH);
        if(save_paths == null) save_paths = new List<string>();
        if(game_saves == null) game_saves = new Dictionary<string, GameSave>();
        else game_saves.Clear();
        DateTime last_date = DateTime.MinValue;
        // 遍历存档路径，读取存档 Read save
        foreach( string path in save_paths)
        {
            string file_path = path.Replace(Application.persistentDataPath + "/", "");
            try{
                // 读取存档 Read save
                string name = file_path.Replace(SAVE_PATH, "").Replace(SAVE_EXT, "");
                game_saves[name] = MgrJson.Mgr().ReadFile<GameSave>(file_path);
                // 最后保存时间 Last save time
                int result = DateTime.Compare(game_saves[name].last_save_time, last_date);
                if(result > 0)
                {
                    last_date = game_saves[name].last_save_time;
                    recent_save = game_saves[name];
                }
            }catch(Exception e){
                // 读取失败，删除文件 Read failed, delete file
                Debug.LogWarning("ReadSavesAsync Error: " + e.Message);
                MgrJson.Mgr().DeleteFile(path);
            }
            yield return null;
        }
    }

    private class GameSave
    {
        public string save_name;
        public DateTime create_time;
        public DateTime last_save_time;
        public TimeSpan play_time;
        public int player_level;
        public Dictionary<string, string> save_data;
    }
}