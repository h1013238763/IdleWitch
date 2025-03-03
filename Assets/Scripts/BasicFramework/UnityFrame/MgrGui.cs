using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

/// <summary>
/// GUI管理器 GUI Manager
/// </summary>
public class MgrGui : MgrBaseMono<MgrGui>
{
    private const string GUI_PATH = "Prefabs/GUI/";   // gui基础路径 gui base path
    private const string LAYER_PREFIX = "Layer ";         // gui前缀 gui prefix


    private GameObject canvas_obj;              // 画布 unity canvas
    private GameObject event_system_obj;        // 事件系统 unity event system


    // GUI界面字典 GUI panel dictionary
    private Dictionary<string, GuiBase> gui_dict = new Dictionary<string, GuiBase>();    
    // GUI层级对象字典 GUI layer object dictionary
    private List<GameObject> gui_layer_obj = new List<GameObject>(); 

    private int total_layer = 0;                // 总层级 total layer
    
    // 初始化 Awake
    private void Awake(){
        // 创建并初始化GUI画布
        canvas_obj = Object.Instantiate(Resources.Load<GameObject>(GUI_PATH+"Canvas"));
        event_system_obj = Object.Instantiate(Resources.Load<GameObject>(GUI_PATH+"EventSystem"));

        canvas_obj.name = "Canvas";
        event_system_obj.name = "EventSystem";

        canvas_obj.transform.SetParent(gameObject.transform);
        event_system_obj.transform.SetParent(gameObject.transform);

        AddLayer(3);
    }

    /// <summary>
    /// 加载GUI界面
    /// load GUI panel
    /// </summary>
    /// <param name="gui_name">GUI界面名称 GUI panel name with path</param>
    /// <param name="layer">显示层级 display layer</param>
    /// <typeparam name="T">BaseGUI类 BaseGUI class</typeparam>
    public void ShowGui<T>(string gui_name, int layer, UnityAction<T> callBack = null) where T : GuiBase{
        AddLayer(layer);    // 添加层级 add layers
        GuiBase gui_base;
        
        if(gui_dict.ContainsKey(gui_name)){ // 如果已经加载 from GUI dictionary
            gui_base = gui_dict[gui_name];  // 获取GUI对象
            // 移动到指定层级 move to target layer
            if(gui_base.gameObject.transform.parent.name != LAYER_PREFIX + layer){
                gui_base.gameObject.transform.SetParent(gui_layer_obj[layer].transform);    
            }
            gui_base.Show();            // 显示GUI show GUI
            if (callBack != null)       // 处理回调函数 handle callback
                callBack(gui_dict[gui_name] as T);
            return;
        }
        // 异步加载GUI  load GUI asynchronously
        MgrResource.Mgr().LoadAsync<GameObject>(GUI_PATH + gui_name, (obj) => {
            // 创建GUI对象 create GUI object
            GameObject gui_obj = Object.Instantiate(obj);   // 创建GUI对象 create GUI object
            GameObject.DontDestroyOnLoad(gui_obj);  // 设置不销毁 set not destroy
            gui_obj.transform.SetParent(gui_layer_obj[layer].transform);    // 设置父对象 set parent object

            gui_obj.name = gui_name;                // 设置GUI名称 set GUI name

            gui_base = gui_obj.GetComponent<T>();   // 获取GUI组件 get GUI component
            gui_dict[gui_name] = gui_base;        // 添加到字典 add to dictionary

            gui_base.Show();    // 显示GUI show GUI
            if (callBack != null)   // 处理回调函数 handle callback
                callBack(gui_base as T);
            return;
        });
    }

    /// <summary>
    /// 隐藏GUI界面
    /// hide GUI panel
    /// </summary>
    /// <param name="gui_name">GUI界面名称 GUI panel name</param>
    /// <returns>GUI界面对象 GUI panel object</returns>
    public void HideGui(string gui_name){
        if(!gui_dict.ContainsKey(gui_name)){
            return;
        }
        var gui_base = gui_dict[gui_name];  // 获取GUI对象 get GUI object
        gui_base.Hide();  // 隐藏GUI事件 hide GUI Event

        GameObject.Destroy(gui_dict[gui_name].gameObject); // 销毁GUI对象 destroy GUI object
        gui_dict.Remove(gui_name); // 从字典中移除 remove from dictionary
    }

    /// <summary>
    /// 获取已显示面板
    /// </summary>
    public T GetGui<T>(string gui_name) where T : GuiBase{
        if (gui_dict.ContainsKey(gui_name))
            return gui_dict[gui_name] as T;
        return null;
    }


    // 添加层级至目标层级
    // add layer to target layer 
    // <param name="layer">目标层级 target layer</param>
    private void AddLayer(int layer){
        while(total_layer <= layer){
            GameObject layer_obj = new GameObject(LAYER_PREFIX + total_layer);
            GameObject.DontDestroyOnLoad(layer_obj);
            gui_layer_obj.Add(layer_obj);
            layer_obj.transform.SetParent(canvas_obj.transform);
            total_layer ++;
        }
    }
}