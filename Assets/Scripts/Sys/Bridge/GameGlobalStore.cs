using Ballance.LuaHelpers;
using Ballance2.Sys.Debug;
using Ballance2.Sys.Package;
using SLua;
using System;
using System.Collections.Generic;
using UnityEngine;

/*
 * Copyright (c) 2020  mengyu
 * 
 * 模块名：     
 * GameGlobalStore.cs
 * 用途：
 * 用于提供全局数据共享功能。
 * 
 * 作者：
 * mengyu
 * 
 * 更改历史：
 * 2020-1-1 创建
 *
 */

namespace Ballance2.Sys.Bridge
{
    /// <summary>
    /// 全局数据共享存储类
    /// </summary>
    [CustomLuaClass]
    [LuaApiDescription("全局数据共享存储类")]
    [Serializable]
    public class StoreData
    {
        /// <summary>
        /// 空数据的静态实例
        /// </summary>
        [LuaApiDescription("空数据的静态实例")]
        public static StoreData Empty { get; } = new StoreData("Empty", StoreDataAccess.GetAndSet, StoreDataType.NotSet);

        /// <summary>
        /// 名称
        /// </summary>
        [LuaApiDescription("名称")]
        public string Name { get; private set; }
        /// <summary>
        /// 原始数据
        /// </summary>
        [LuaApiDescription("原始数据")]
        public object DataRaw
        {
            get
            {
                if (StoreDataProvider != null)
                {
                   object data = StoreDataProvider(true, null);
                    if(data == null)
                    {
                        _DataRaw = null;
                        return _DataRaw;
                    }
                    //类型检查
                    string typeName = data.GetType().Name;
                    if (_DataType != StoreDataType.Custom && typeName != _DataType.ToString())
                        return _DataRaw;

                    _DataRaw = data;
                }
                return _DataRaw;
            }
        }
        /// <summary>
        /// 数据类型
        /// </summary>
        [LuaApiDescription("数据类型")]
        public StoreDataType DataType
        {
            get { return _DataType; }
        }
        /// <summary>
        /// 数组类型
        /// </summary>
        [LuaApiDescription("数组类型")]
        public List<StoreData> DataArray
        {
            get { return _DataArray; }
        }
        
        private int currentHolderContext = 0;
        private object _DataRaw = null;
        private StoreDataType _DataType = StoreDataType.Custom;
        [NonSerialized]
        private List<StoreData> _DataArray = new List<StoreData>();
        private StoreDataAccess _StoreDataAccess = StoreDataAccess.Get;

        internal StoreData(string name, StoreDataAccess access, StoreDataType dataType)
        {
            Name = name;
            _StoreDataAccess = access;
            _DataType = dataType;
        }

        /// <summary>
        /// 判断当前数据是否为空
        /// </summary>
        /// <returns>如果当前数据类未设置数据，或是类型为 Empty，则返回 true</returns>
        [LuaApiDescription("判断当前数据是否为空", "如果当前数据类未设置数据，或是类型为 Empty，则返回 true")]
        public bool IsNull()
        {
            if (DataType == StoreDataType.NotSet)
                return true;
            if (DataType == StoreDataType.GameObject)
                return ((GameObject)DataRaw) == null;
            if (DataType == StoreDataType.Object)
                return ((UnityEngine.Object)DataRaw) == null;
            return DataRaw == null;
        }
        /// <summary>
        /// 释放
        /// </summary>
        [LuaApiDescription("释放")]
        public void Destroy()
        {
            _DataRaw = null;
            _DataType = StoreDataType.NotSet;
            if(DataArray != null)
            {
                foreach (var v in DataArray)
                    v.Destroy();
                _DataArray.Clear();
                _DataArray = null;
            }
        }

        #region 观察者和数据提供者

        private StoreOnDataChanged DataObserver;

        /// <summary>
        /// 注册数据更新观察者
        /// </summary>
        /// <param name="observer">观察者</param>
        [LuaApiDescription("注册数据更新观察者")]
        [LuaApiParamDescription("observer", "观察者")]
        public StoreOnDataChanged RegisterDataObserver(StoreOnDataChanged observer)
        {
            DataObserver += observer;
            return observer;
        }
        /// <summary>
        /// 移除已经注册的数据更新观察者
        /// </summary>
        /// <param name="observer">观察者</param>
        [LuaApiDescription("移除已经注册的数据更新观察者")]
        [LuaApiParamDescription("observer", "观察者")]
        public void UnRegisterDataObserver(StoreOnDataChanged observer)
        {
            DataObserver -= observer;
        }
        /// <summary>
        /// 通知数据更新观察者数据已经更改
        /// </summary>
        /// <param name="oldV">旧的值</param>
        /// <param name="newV">新的值</param>
        [LuaApiDescription("通知数据更新观察者数据已经更改")]
        [LuaApiParamDescription("oldV", "旧的值")]
        [LuaApiParamDescription("newV", "新的值")]
        public void NotificationDataObserver(object oldV, object newV)
        {
            if (DataObserver != null)
                DataObserver(this, oldV, newV);
        }
        /// <summary>
        /// 重新通知数据更新观察者数据已经更改
        /// </summary>
        [LuaApiDescription("重新通知数据更新观察者数据已经更改")]
        public void ReNotificationAllDataObserver() {
            if (DataObserver != null)
                DataObserver(this, null, Data());
        }

        private StoreDataProvider StoreDataProvider = null;

        /// <summary>
        /// 设置数据提供者
        /// </summary>
        /// <param name="context">数据安全上下文（注册成功后须使用此上下文才能进行私有数据更新）</param>
        /// <param name="provider">数据提供者</param>
        /// <returns></returns>
        [LuaApiDescription("设置数据提供者")]
        [LuaApiParamDescription("context", "数据安全上下文（注册成功后须使用此上下文才能进行私有数据更新）")]
        [LuaApiParamDescription("provider", "数据提供者")]
        public int SetDataProvider(int context, StoreDataProvider provider)
        {
            if(StoreDataProvider == null)
            {
                StoreDataProvider = provider;
                currentHolderContext = context;
                return context;
            }
            else if(context != currentHolderContext)
            {
                GameErrorChecker.SetLastErrorAndLog(GameError.ContextMismatch, "StoreData", 
                    "上下文 {0} 没有操作此数据的权限", context);
                return 0;
            }
            else
            {
                StoreDataProvider = provider;
                return context;
            }
        }
        /// <summary>
        /// 取消设置数据提供者
        /// </summary>
        /// <param name="context">数据安全上下文</param>
        [LuaApiDescription("取消设置数据提供者")]
        [LuaApiParamDescription("context", "数据安全上下文")]
        public void RemoveRegisterProvider(int context)
        {
            if (StoreDataProvider != null)
            {
                if (context != currentHolderContext)
                {
                    GameErrorChecker.SetLastErrorAndLog(GameError.ContextMismatch, "StoreData",
                        "上下文 {0} 没有操作此数据的权限", context);
                    return;
                }

                StoreDataProvider = null;
            }
        }

        #endregion

        #region 设置数据

        /// <summary>
        /// 所有者设置数据的值
        /// </summary>
        /// <param name="context">数据安全上下文</param>
        /// <param name="data">新值</param>
        /// <returns>返回设置是否成功</returns>
        [LuaApiDescription("所有者设置数据的值", "返回设置是否成功")]
        [LuaApiParamDescription("context", "数据安全上下文")]
        [LuaApiParamDescription("data", "新值")]
        public bool OwnerSetData(int context, object data)
        {
            if (_StoreDataAccess != StoreDataAccess.GetAndSet && context != currentHolderContext)
            {
                GameErrorChecker.SetLastErrorAndLog(GameError.ContextMismatch, "StoreData",
                    "上下文 {0} 没有操作此数据的权限", context);
                return false;
            }

            return SetDataInternal(data);
        }
        /// <summary>
        /// 设置数据的值
        /// </summary>
        /// <param name="data">新值</param>
        /// <returns>返回设置是否成功</returns>
        [LuaApiDescription("设置数据的值", "返回设置是否成功")]
        [LuaApiParamDescription("data", "新值")]
        public bool SetData(object data)
        {
            //只读检测
            if (_StoreDataAccess != StoreDataAccess.GetAndSet)
            {
                GameErrorChecker.SetLastErrorAndLog(GameError.ParamReadOnly, "StoreData", "参数 {0} 只读", Name);
                return false;
            }

            return SetDataInternal(data);
        }
        /// <summary>
        /// 获取数据的值
        /// </summary>
        /// <returns></returns>
        [LuaApiDescription("获取数据的值")]
        public object GetData() {
            return DataRaw;
        }

        private bool SetDataInternal(object data) {
            //类型检查
            if (_DataType != StoreDataType.Custom && data.GetType().Name != _DataType.ToString())
            {
                GameErrorChecker.SetLastErrorAndLog(GameError.ContextMismatch, "StoreData",
                  "输入 {0} 与设置的类型不符 {1}", data, _DataType.ToString());
                return false;
            }

            //设置值，分为已有Provider和原始数据两种情况的设置
            if (StoreDataProvider != null) {
                object old = StoreDataProvider(true, null);
                StoreDataProvider(false, data);
                NotificationDataObserver(old, data);
            } 
            else if (_DataRaw != data)
            {
                object old = _DataRaw;
                _DataRaw = data;
                NotificationDataObserver(old, data);
            }
            return true;
        }

        #endregion

        #region 数组操作

        public bool DataArrayAdd(StoreData data)
        {
            if (!DataArray.Contains(data))
            {
                DataArray.Add(data);
                return true;
            }
            return false;
        }
        public void DataArrayRemove(StoreData data)
        {
            DataArray.Remove(data);
        }
        public void DataArrayRemoveAt(int index)
        {
            DataArray.RemoveAt(index);
        }
        public void DataArrayRemoveAt(int index, int count)
        {
            DataArray.RemoveRange(index, count);
        }
        public void DataArrayInsert(int index, StoreData data)
        {
            DataArray.Insert(index, data);
        }
        public int DataArrayGetCount()
        {
            return DataArray.Count;
        }
        public StoreData DataArrayGet(int index)
        {
            return DataArray[index];
        }
        public void DataArrayClear()
        {
            DataArray.Clear();
        }

        #endregion

        #region 获取数据

        public int IntData() { return (int)DataRaw; }
        public long LongData() { return (long)DataRaw; }
        public float FloatData() { return (float)DataRaw; }
        public string StringData() { return (string)DataRaw; }
        public bool BoolData() { return (bool)DataRaw; }
        public double DoubleData() { return (double)DataRaw; }
        public object Data() { return DataRaw; }
        public T Data<T>() { return (T)DataRaw; }
        public StoreData[] ArrayData() { return DataArray.ToArray(); }
        public List<StoreData> ListArrayData() { return DataArray; }
        public Color ColorData() { return (Color)DataRaw; }
        public Material MaterialData() { return DataRaw == null ? null : (Material)DataRaw; }
        public Texture TextureData() { return DataRaw == null ? null : (Texture)DataRaw; }
        public Texture2D Texture2DData() { return DataRaw == null ? null : (Texture2D)DataRaw; }
        public Vector2 Vector2Data() { return (Vector2)DataRaw; }
        public Vector3 Vector3Data() { return (Vector3)DataRaw; }
        public Vector4 Vector4Data() { return (Vector4)DataRaw; }
        public Quaternion QuaternionData() { return (Quaternion)DataRaw; }
        public Sprite SpriteData() { return DataRaw == null ? null : (Sprite)DataRaw; }
        public Rigidbody RigidbodyData() { return DataRaw == null ? null : (Rigidbody)DataRaw; }
        public Rigidbody2D Rigidbody2DData() { return DataRaw == null ? null : (Rigidbody2D)DataRaw; }
        public RectTransform RectTransformData() { return DataRaw == null ? null : (RectTransform)DataRaw; }
        public Transform TransformData() { return DataRaw == null ? null : (Transform)DataRaw; }
        public Camera CameraData() { return DataRaw == null ? null : (Camera)DataRaw; }
        public GameObject GameObjectData() { return DataRaw == null ? null : (GameObject)DataRaw; }
        public UnityEngine.Object ObjectData() { return DataRaw == null ? null : (UnityEngine.Object)DataRaw; }
        public AudioClip AudioClipData() { return DataRaw == null ? null : (AudioClip)DataRaw; }
        public AudioSource AudioSourceData() { return DataRaw == null ? null : (AudioSource)DataRaw; }
        public MonoBehaviour MonoBehaviourData() { return DataRaw == null ? null : (MonoBehaviour)DataRaw; }
        public GamePackage GamePackageData() { return DataRaw == null ? null : (GamePackage)DataRaw; }

        #endregion

        /// <summary>
        /// 将当前数据转为字符串表达形式
        /// </summary>
        /// <returns>字符串</returns>
        [LuaApiDescription("将当前数据转为字符串表达形式", "字符串")]
        public override string ToString()
        {
            if (DataType == StoreDataType.NotSet)
                return " [StoreData NotSet]";
            if (DataType == StoreDataType.Array)
                return DataArray.Count + " [StoreData Array]";
            if (DataRaw == null)
                return "[StoreData Null]";
            return DataRaw.ToString() + " [StoreData " + DataType + "]";
        }
    }

    /// <summary>
    /// 数据改变回调
    /// </summary>
    /// <param name="data">当前数据类</param>
    /// <param name="oldV">旧值</param>
    /// <param name="newV">新值</param>
    [CustomLuaClass]
    [LuaApiDescription("数据改变回调")]
    public delegate void StoreOnDataChanged(StoreData data, object oldV, object newV);
    /// <summary>
    /// 用于自己提供数据的回调
    /// </summary>
    /// <returns>请返回当前数据的值</returns>
    [CustomLuaClass]
    [LuaApiDescription("用于自己提供数据的回调")]
    public delegate object StoreDataProvider(bool isGet, object newValue);

    /// <summary>
    /// 数据池所用的类型
    /// </summary>
    [CustomLuaClass]
    [LuaApiDescription("数据池所用的类型")]
    public enum StoreDataType
    {
        NotSet,
        /// <summary>
        /// object 类型
        /// </summary>
        [LuaApiDescription("object 类型")]
        Custom,
        Array,
        Integer,
        Long,
        Float,
        String,
        Boolean,
        Double,
        Color,
        Material,
        Texture,
        Texture2D,
        Vector2,
        Vector3,
        Vector4,
        Quaternion,
        Sprite,
        Rigidbody,
        RectTransform,
        Transform,
        Camera,
        GameObject,
        /// <summary>
        /// UnityEngine.Object
        /// </summary>
        [LuaApiDescription("UnityEngine.Object")]
        Object,
        AudioClip,
        AudioSource,
        MonoBehaviour,
    }

    /// <summary>
    /// 数据可访问类型
    /// </summary>
    [CustomLuaClass]
    [LuaApiDescription("数据可访问类型")]
    public enum StoreDataAccess
    {
        /// <summary>
        /// 仅获取
        /// </summary>
        [LuaApiDescription("仅获取")]
        Get,
        /// <summary>
        /// 获取和设置
        /// </summary>
        [LuaApiDescription("获取和设置")]
        GetAndSet,
    }

    /// <summary>
    /// 全局数据共享存储池类
    /// </summary>
    [CustomLuaClass]
    [Serializable]
    [LuaApiDescription("全局数据共享存储池类")]
    public class Store
    {
        [SerializeField, SetProperty("PoolName")]
        private string _PoolName;
        [SerializeField, SetProperty("PoolDatas")]
        private Dictionary<string, StoreData> _PoolDatas;

        /// <summary>
        /// 池的名称
        /// </summary>
        [LuaApiDescription("池的名称")]
        public string PoolName { get { return _PoolName; }  }
        /// <summary>
        /// 池中的数据
        /// </summary>
        [DoNotToLua]
        public Dictionary<string, StoreData> PoolDatas { get { return _PoolDatas; } }

        internal Store(string name)
        {
            _PoolName = name;
            _PoolDatas = new Dictionary<string, StoreData>();
        }

        [DoNotToLua]
        public void Destroy()
        {
            _PoolName = null;
            if(_PoolDatas != null)
            {
                foreach (var v in _PoolDatas)
                    v.Value.Destroy();
                _PoolDatas.Clear();
                _PoolDatas = null;
            }
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="name">数据名称</param>
        /// <returns>添加成功，则返回数据，如果数据已经存在，则返回存在的实例</returns>
        [LuaApiDescription("原始数据", "添加成功，则返回数据，如果数据已经存在，则返回存在的实例")]
        [LuaApiParamDescription("name", "数据名称")]
        public StoreData AddParameter(string name, StoreDataAccess access, StoreDataType storeDataType)
        {
            StoreData old;
            if (_PoolDatas.TryGetValue(name, out old))
                return old;

            old = new StoreData(name, access, storeDataType);
            _PoolDatas.Add(name, old);
            return old;
        }
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="name">数据名称</param>
        /// <returns>如果移除成功，返回true，如果数据不存在，返回false</returns>
        [LuaApiDescription("原始数据", "如果移除成功，返回true，如果数据不存在，返回false")]
        [LuaApiParamDescription("name", "数据名称")]
        public bool RemoveParameter(string name)
        {
            if (_PoolDatas != null && _PoolDatas.ContainsKey(name))
            {
                _PoolDatas.Remove(name);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取池中的数据
        /// </summary>
        /// <param name="name">数据名称</param>
        /// <returns>返回数据实例</returns>
        [LuaApiDescription("获取池中的数据", "返回数据实例")]
        [LuaApiParamDescription("name", "数据名称")]
        public StoreData GetParameter(string name)
        {
            StoreData old;
            _PoolDatas.TryGetValue(name, out old);
            return old;
        }

        /// <summary>
        /// 获取或设置指定参数的值
        /// </summary>
        /// <value>要设置的值</value>
        [LuaApiDescription("获取或设置指定参数的值")]
        public object this[string key] {
            get {
                var prop = this.GetParameter(key);
                if(prop != null) 
                    return prop.GetData();
                else
                    GameErrorChecker.SetLastErrorAndLog(GameError.ParamNotFound, "StoreData Get", "未找到参数 {0}", key);
                return null;
            }
            set {
                var prop = this.GetParameter(key);
                if(prop != null) 
                     prop.SetData(value);
                else
                    GameErrorChecker.SetLastErrorAndLog(GameError.ParamNotFound, "StoreData Set", "未找到参数 {0}", key);
            }
        }
    }
}