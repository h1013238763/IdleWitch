using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 对象池管理器 
/// Object Pool Manager
/// </summary>
public class MgrPool : MgrBase<MgrPool>{

    // 池挂载父对象 Pool objs Parent Object
    private GameObject pool_obj;

    // 对象池池字典 Object Pool pool Dictionary
    private Dictionary<string, PoolData> pool_dict = new Dictionary<string, PoolData>();

    /// <summary>
    /// 往对象池中放入对象
    /// Put object into object pool
    /// </summary>
    /// <param name="name"> 对象名 object name</param>
    /// <param name="obj"> 对象 object</param>
    public void PushObj(string name, GameObject obj){
        try{
            if(pool_obj == null)    // 创建对象池 Create object pool
                pool_obj = new GameObject("PoolMgr");
            if(obj == null)     // 无效对象 Invalid object
                return; 

            if(pool_dict.ContainsKey(name))
                pool_dict[name].PushObj(obj);
            else
                pool_dict.Add(name, new PoolData(obj, pool_obj));
        }
        catch(System.Exception e){
            Debug.LogWarning("PushObj Error: " + e.Message);
        }
    }

    /// <summary>
    /// 从对象池中取出对象
    /// Get object from object pool
    /// </summary>
    /// <param name="name">对象名 object name</param>
    /// <param name="callBack">回调函数 callback function</param>
    /// <returns>被取出的对象 The object that was pop</returns>
    public void PopObj(string name, UnityAction<GameObject> callBack)
    {
        // 判断是否有这个池子和池子中是否有对象
        // Determine if there is a pool and if there are objects in the pool
        if (pool_dict.ContainsKey(name) && pool_dict[name].count > 0)
        {
            callBack(pool_dict[name].PopObj());
        }
        else
        {
            //异步创建对象 返回给外部 use async to create object and return to outside
            MgrResource.Mgr().LoadAsync<GameObject>(name, (obj) =>
            {
                obj.name = name;
                callBack(obj);
            });
        }
    }

    /// <summary>
    /// 清理对象池
    /// Clear object pool
    /// </summary>//  
    public void Clear(){
        pool_obj = null;
        pool_dict.Clear();
    }


    // 池数据类 Pool Data Class
    private class PoolData{

        // 子对象池挂载对象 Sub Object Pool parent Object
        private GameObject pool_obj;

        /// <summary>
        /// 对象数量 Object count
        /// </summary>
        public int count{ get; private set; }

        // 对象列表 Object List
        private List<GameObject> pool_list; // 对象池列表 Object Pool List

        /// <summary>
        /// 构造函数 Constructor
        /// </summary>
        /// <param name="obj"> 对象 object</param>
        /// <param name="parent_obj"> 父对象 parent object</param>
        public PoolData(GameObject obj, GameObject parent_obj){
            pool_obj = new GameObject(obj.name);
            pool_obj.transform.parent = parent_obj.transform;
            pool_list = new List<GameObject>();

            PushObj(obj);
        }

        /// <summary>
        /// 往对象池中放入对象
        /// Put object into object pool
        /// </summary>
        /// <param name="obj"> 对象 object</param> 
        public void PushObj(GameObject obj){
            obj.SetActive(false);
            obj.transform.parent = pool_obj.transform;
            pool_list.Add(obj);
            count ++;
        }

        /// <summary>
        /// 从对象池中取出对象
        /// Get object from object pool 
        /// </summary>
        /// <returns>被取出的对象 The object that was pop</returns>
        public GameObject PopObj(){
            if(pool_list.Count == 0)
                throw new System.Exception("No object in pool");
            GameObject obj = pool_list[0];
            return obj;
        }
    }
}

