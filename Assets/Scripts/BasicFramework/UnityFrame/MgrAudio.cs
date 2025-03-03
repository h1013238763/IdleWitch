using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 音频管理器 
/// Audio Manager
/// </summary>
public class MgrAudio : MgrBaseMono<MgrAudio>, ISaveData
{
    
    private AudioConfig saved_config;   // 已保存配置数据 Saved configuration data
    private AudioConfig curr_config;    // 当前配置数据 Current configuration data

    private GameObject master_obj;                  // 主控件挂载对象 master audio object
    private Dictionary<AudioClass, GameObject> audio_obj_list;        // 音效片挂载对象 audio clip object list

    private Dictionary<AudioClass, List<AudioSource>> audio_clip_list;// 音效片表 audio clip list
    private Dictionary<AudioClass, List<AudioSource>> audio_pool;     // 音效片对象池 audio clip object pool

    private List<(AudioClass key, AudioSource value)> remove_list; // 移除列表 remove list

    // 初始化函数 Initialize function
    private void Awake(){
        // 设置对象 Set object
        if(master_obj == null){ master_obj = gameObject; }   // 设定根对象 Set root object
        curr_config = new AudioConfig();                     // 初始化当前配置 Initialize current configuration
        audio_obj_list = new Dictionary<AudioClass, GameObject>();         // 初始化音效片挂载对象列表 Initialize audio clip object list
        audio_clip_list = new Dictionary<AudioClass, List<AudioSource>>(); // 初始化音效片表 Initialize audio clip list
        audio_pool = new Dictionary<AudioClass, List<AudioSource>>();            // 初始化音效片对象池 Initialize audio clip object pool
        remove_list = new List<(AudioClass key, AudioSource value)>(); // 初始化移除列表 Initialize remove list
        foreach(AudioClass class_name in Enum.GetValues(typeof(AudioClass))) // 设置音效对象 Set audio object
            AddAudioClass(class_name);
    }

    // 更新函数 Update function
    private void Update()
    {
        // 遍历音效片表，清除已停止播放的音效片 Remove stopped audio clips
        foreach( AudioClass audio_class in audio_clip_list.Keys )
        {
            for( int audio_source = audio_clip_list[audio_class].Count-1; 
                audio_source >= 0; audio_source -- )
            {
                if(!audio_clip_list[audio_class][audio_source].isPlaying)
                    remove_list.Add((
                        key: audio_class, 
                        value: audio_clip_list[audio_class][audio_source]));
            }
        }

        foreach(var pair in remove_list)
        {
            StopAudio(pair.key, pair.value);
        }

        remove_list.Clear();
    }

    /// <summary>
    /// 播放音效片 Play audio clip
    /// </summary>
    /// <param name="audio_class">音效片类型 Audio clip type</param>
    /// <param name="name">音效片名称 Audio clip name</param>
    /// <param name="is_loop">是否循环播放 Whether to loop</param>
    /// <param name="callBack">回调函数 Callback function</param>
    public void PlayAudio(AudioClass audio_class, string name, bool is_loop, UnityAction<AudioSource> callBack = null){
        AudioSource source = PoolPopSource(audio_class);
        //异步加载 加载完成后播放 Load asynchronously and play after loading
        try{
            MgrResource.Mgr().LoadAsync<AudioClip>("Audio/"+ audio_class.ToString()+"/"+name, (clip) =>
            {
                source.clip = clip;
                source.volume = curr_config.master_vol * 
                    curr_config.audio_vol_list[audio_class];
                source.loop = (audio_class == AudioClass.Music || is_loop);
                source.Play();
                audio_clip_list[audio_class].Add(source);
                if(callBack != null)
                    callBack.Invoke(source);
            });
        }catch(Exception e){
            Debug.LogWarning("AudioMgr: PlayAudio Error: "+e.Message);
            return;
        }
    }

    /// <summary>
    /// 修改主音量
    /// Change master volume
    /// </summary>
    /// <param name="volume">修改值 (0~1) Change value (0~1)</param>
    public void ChangeMasterVolume(float volume){
        volume = (float)Math.Round(volume, 2);
        if(volume < 0 || volume > 1){  // 排除错误输入 Exclude wrong input
            Debug.LogWarning("AudioMgr: ChangeMasterVolume Error: Volume: "+volume+", should be 0~1");
            return;
        }
        // 设置主音量值，遍历以修改所有音量 Set master volume value and change all volumes
        curr_config.master_vol = volume;
        foreach(AudioClass audio_class in Enum.GetValues(typeof(AudioClass))){
            ChangeVolume(audio_class, curr_config.audio_vol_list[audio_class]);
        }
    }

    /// <summary>
    /// 修改音量
    /// Change volume
    /// </summary>
    /// <param name="audio_class">音效片类型 Audio clip type</param>
    /// <param name="volume">修改值(0~1) Change value (0~1)</param>
    public void ChangeVolume(AudioClass audio_class, float volume){
        volume = (float)Math.Round(volume, 2);
        if(volume < 0 || volume > 1){   // 排除错误输入 Exclude wrong input
            Debug.LogWarning("AudioMgr: ChangeVolume Error: Volume: "+volume+", should be 0~1");
            return;
        }
        curr_config.audio_vol_list[audio_class] = volume;     // 修改音量数值 Change volume value
        // 修改音效片实际音量 Change actual volume of audio clip
        foreach(AudioSource audio_source in audio_clip_list[audio_class]){
            audio_source.volume = curr_config.master_vol * volume;
        }
    }

    /// <summary>
    /// 读取主音量 Read master volume
    /// </summary>
    /// <returns>音量 Volume</returns>
    public float GetMasterVolume(){
        return curr_config.master_vol;
    }

    /// <summary>
    /// 读取音量 Read volume
    /// </summary>
    /// <param name="audio_class">音效片类型 Audio clip type</param>
    /// <returns>音量 Volume</returns>
    public float GetVolume(AudioClass audio_class){
        return curr_config.audio_vol_list[audio_class];
    }

    /// <summary>
    /// 暂停音效片 Pause audio clip
    /// </summary>
    /// <param name="audio_source">音效片 Audio clip</param>
    public void PasueAudio(AudioSource audio_source){
        try{
            audio_source.Pause();
        }catch(Exception e){
            Debug.LogWarning("AudioMgr: PauseAudio Error: "+e.Message);
            return;
        }
    }

    /// <summary>
    /// 停止音效片
    /// Stop audio clip
    /// </summary>
    /// <param name="audio_class">音效片类型 Audio clip type</param>
    /// <param name="audio_source">音效片 Audio clip</param>
    public void StopAudio(AudioClass audio_class, AudioSource audio_source){
        try{
            audio_source.Stop();
            PoolPushSource(audio_class, audio_source);
            audio_clip_list[audio_class].Remove(audio_source);
        }catch(Exception e){
            Debug.LogWarning("AudioMgr: StopAudio Error: "+e.Message);
            return;
        }
    }

    /// <summary>
    /// 停止所有对应类型音效片 Stop all audio clips of corresponding type
    /// </summary>
    /// <param name="audio_class">音效片类型 Audio clip type</param>
    public void StopAudio(AudioClass audio_class){
        try{
            foreach(AudioSource audio_source in audio_clip_list[audio_class]){
                audio_source.Stop();
                PoolPushSource(audio_class, audio_source);
            }
            audio_clip_list[audio_class].Clear();
        }catch(Exception e){
            Debug.LogWarning("AudioMgr: StopAudio Error: "+e.Message);
            return;
        }
    }

    /// <summary>
    /// 停止所有音效片 Stop all audio clips
    /// </summary> 
    public void StopAudio(){
        try{
            foreach(AudioClass audio_class in audio_clip_list.Keys){
                foreach(AudioSource audio_source in audio_clip_list[audio_class]){
                    audio_source.Stop();
                    PoolPushSource(audio_class, audio_source);
                }
                audio_clip_list[audio_class].Clear();
            }
        }catch(Exception e){
            Debug.LogWarning("AudioMgr: StopAudio Error: "+e.Message);
            return;
        }
    }


    // 初始化音效片对象 Initialize audio clip object
    // <param name="class_name"> 音效片类型 Audio clip type </param>
    private void AddAudioClass(AudioClass class_name){
        // 设置音效片挂载对象 Set audio clip parent and root object
        GameObject obj = new GameObject(class_name.ToString());
        GameObject.DontDestroyOnLoad(obj);
        obj.transform.SetParent(master_obj.transform);
        audio_obj_list[class_name] = obj;
        // 设置音效片表 Set audio clip list
        audio_clip_list[class_name] = new List<AudioSource>();
        // 设置音效片对象池 Set audio clip object pool
        audio_pool[class_name] = new List<AudioSource>();
    }

    // 将音效片压入对象池 Push audio clip into object pool
    // <param name="clip">音效片 Audio clip</param>
    private void PoolPushSource(AudioClass audio_class, AudioSource source){
        audio_pool[audio_class].Add(source);
    }

    // 从对象池中弹出音效片 Pop audio clip from object pool
    // <returns>音效片 Audio clip</returns>
    private AudioSource PoolPopSource(AudioClass audio_class){

        AudioSource source;
        if(audio_pool[audio_class].Count == 0)
            source = audio_obj_list[audio_class].AddComponent<AudioSource>();
        else
        {
            source = audio_pool[audio_class][0];
            audio_pool[audio_class].RemoveAt(0);
        }
        return source;
    }

    // 清空对象池 Clear object pool
    private void PoolClear(){
        foreach(AudioClass audio_class in audio_pool.Keys){
            audio_pool[audio_class].Clear();
        }
    }

    // 音频配置 Audio configuration
    private class AudioConfig{
        public float master_vol;
        public Dictionary<AudioClass, float> audio_vol_list;

        // 构造函数 Constructor
        public AudioConfig(){
            master_vol = 0.5f;
            audio_vol_list = new Dictionary<AudioClass, float>();
            foreach(AudioClass audio_class in Enum.GetValues(typeof(AudioClass))){
                audio_vol_list[audio_class] = 0.5f;
            }
        }
    }

    /// <summary>
    /// 创建新配置数据 Create new configuration data
    /// </summary>
    public void NewData(){
        saved_config = new AudioConfig();
        curr_config = new AudioConfig();
    }

    /// <summary>
    /// 保存配置数据 Save configuration data
    /// </summary>
    /// <returns>配置数据Json Configuration data Json</returns>
    public string SaveData(){
        saved_config.master_vol = curr_config.master_vol;
        saved_config.audio_vol_list = new Dictionary<AudioClass, float>(curr_config.audio_vol_list);
        return MgrJson.Mgr().ObjectToJson<AudioConfig>(saved_config);
    }

    /// <summary>
    /// 加载配置数据 Load configuration data
    /// </summary>
    /// <param name="config_json">配置Json Config Json</param>
    public void LoadData(string config_json){
        saved_config = MgrJson.Mgr().JsonToObject<AudioConfig>(config_json);
        curr_config.master_vol = saved_config.master_vol;
        curr_config.audio_vol_list = new Dictionary<AudioClass, float>(saved_config.audio_vol_list);
    }

    /// <summary>
    /// 应用配置数据 Apply config data
    /// </summary>
    public void ApplyData()
    {
        try{
            ChangeMasterVolume(curr_config.master_vol);
            foreach(AudioClass audio_class in Enum.GetValues(typeof(AudioClass))){
                ChangeVolume(audio_class, curr_config.audio_vol_list[audio_class]);
            }
        }catch(Exception e){
            Debug.LogWarning("AudioMgr: ApplyData Error: "+e.Message);
            return;
        }
    }
}

/// <summary>
/// 音效类型枚举 Audio class enumeration
/// </summary>
public enum AudioClass{
    /// <summary>
    /// 背景音乐 background music
    /// </summary>
    Music = 0,

    /// <summary>
    /// 音效 sound effect
    /// </summary>
    Sound = 1,

    /// <summary>
    /// 环境音 environment sound
    /// </summary>
    Environment = 2,
}