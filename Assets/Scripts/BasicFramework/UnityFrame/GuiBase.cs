using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using TMPro;

/// <summary>
/// GUI基类 GUI Base Class
/// </summary>
public abstract class GuiBase : MonoBehaviour {
    /// <summary>
    /// 子物体字典 Child Object Dictionary
    /// </summary>
    protected Dictionary<string, List<UIBehaviour>> child_dict;        

    /// <summary>
    /// 初始化 Awake
    /// </summary>
    protected virtual void Awake(){
        child_dict = new Dictionary<string, List<UIBehaviour>>();
        AddChildrenControl<Button>();
        AddChildrenControl<Image>();
        AddChildrenControl<Text>();
        AddChildrenControl<Toggle>();
        AddChildrenControl<Slider>();
        AddChildrenControl<ScrollRect>();
        AddChildrenControl<InputField>();
        AddChildrenControl<Dropdown>();
        AddChildrenControl<TextMeshProUGUI>();
        AddChildrenControl<TMP_Dropdown>();
    }

    /// <summary>
    /// 显示事件 Show Event
    /// </summary>
    public virtual void Show(){}

    /// <summary>
    /// 隐藏事件 Hide Event
    /// </summary>
    public virtual void Hide(){}
       
    /// <summary>
    /// 获取子物体组件 Get child component
    /// </summary>
    /// <typeparam name="T"> 组件类型 Component Type </typeparam>
    /// <param name="child_name"> 子物体名称 Child Object Name </param>
    /// <returns> 组件 Component </returns>
    protected T GetComponent<T>(string child_name) where T : Object
    {
        if(child_dict.ContainsKey(child_name))
        {
            for(int i = 0; i < child_dict[child_name].Count; ++i)
            {
                if(child_dict[child_name][i] is T)
                    return child_dict[child_name][i] as T;
            }
        }
        return null;
    }

    /// <summary>
    /// 添加组件事件监听 Add component event listener
    /// </summary>
    /// <returns></returns>
    protected void AddChildrenControl<T>() where T : UIBehaviour
    {
        T[] components = this.GetComponentsInChildren<T>();
        
        for (int i = 0; i < components.Length; ++i)
        {
            string obj_name = components[i].gameObject.name;

            if (child_dict.ContainsKey(obj_name))
                child_dict[obj_name].Add(components[i]);
            else
                child_dict.Add(obj_name, new List<UIBehaviour>() { components[i] });
            
            // 添加事件监听 Add event listener
            // 如果是按钮 If button
            if(components[i] is Button)
            {
                (components[i] as Button).onClick.AddListener(()=>
                {
                    OnButtonClick(obj_name);
                });
            }
            // 如果是选框 If toggle
            else if(components[i] is Toggle)
            {
                (components[i] as Toggle).onValueChanged.AddListener((value) => 
                { 
                    OnToggleValueChanged(obj_name, value); 
                });
            }
            // 如果是滑动条 If slider
            else if(components[i] is Slider)
            {
                (components[i] as Slider).onValueChanged.AddListener((value) => 
                { 
                    OnSliderValueChanged(obj_name, value); 
                });
            }
            // 如果是输入框 If input field
            else if(components[i] is InputField)
            {
                (components[i] as InputField).onEndEdit.AddListener((value) => 
                { 
                    OnInputFieldEndEdit(obj_name, value); 
                });
            }
            // 如果是下拉框 If dropdown
            else if(components[i] is Dropdown)
            {
                (components[i] as Dropdown).onValueChanged.AddListener((value) => 
                { 
                    OnDropdownValueChanged(obj_name, value); 
                });
            }
            // 如果是TMP下拉框 If TMP dropdown
            else if(components[i] is TMP_Dropdown)
            {
                (components[i] as TMP_Dropdown).onValueChanged.AddListener((value) => 
                { 
                    OnDropdownValueChanged(obj_name, value); 
                });
            }
        }
    }

    /// <summary>
    /// 处理按钮点击事件 Handle button click event
    /// </summary>
    /// <param name="btnName"> 按钮名称 Button Name </param>
    protected virtual void OnButtonClick(string btn_name) {}

    /// <summary>
    /// 处理开关状态变化事件 Handle toggle state change event
    /// </summary>
    /// <param name="toggleName"> 开关名称 Toggle Name </param>
    /// <param name="value"> 开关状态 Toggle State </param>
    protected virtual void OnToggleValueChanged(string toggle_name, bool value) {}

    /// <summary>
    /// 处理滑动条值变化事件 Handle slider value change event
    /// </summary>
    /// <param name="sliderName"> 滑动条名称 Slider Name </param>
    /// <param name="value"> 滑动条值 Slider Value </param>
    protected virtual void OnSliderValueChanged(string slider_name, float value) {}

    /// <summary>
    /// 处理输入框结束编辑事件 Handle input field end edit event
    /// </summary>
    /// <param name="fieldName"> 输入框名称 Input Field Name </param>
    /// <param name="value"> 输入框值 Input Field Value </param>
    protected virtual void OnInputFieldEndEdit(string field_name, string value) {}

    /// <summary>
    /// 处理下拉框值变化事件 Handle dropdown value change event
    /// </summary>
    /// <param name="dropdown_name"> 下拉框名称 Dropdown Name </param>
    /// <param name="value"> 下拉框值 Dropdown Value </param>
    protected virtual void OnDropdownValueChanged(string dropdown_name, int value) {}
}