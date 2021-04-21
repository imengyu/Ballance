using System.Collections.Generic;
using Ballance.LuaHelpers;
using Ballance2.Sys.Bridge;
using Ballance2.Sys.Bridge.LuaWapper;
using Ballance2.Utils;
using SLua;
using UnityEngine;

/*
* Copyright(c) 2021  mengyu
*
* 模块名：     
* GameUIMessageUtils.cs
* 
* 用途：
* UI 消息中心，方便Lua层处理UI事件。
*
* 作者：
* mengyu
*
* 更改历史：
* 2021-4-19 创建
*
*/

namespace Ballance2.Sys.UI
{
    /// <summary>
    /// UI 消息中心，方便Lua层处理UI事件
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    [AddComponentMenu("Ballance/UI/MessageCenter")]
    [SLua.CustomLuaClass]
    [LuaApiDescription("UI 消息中心，方便Lua层处理UI事件")]
    public class GameUIMessageCenter : MonoBehaviour
    {
        private static Dictionary<string, GameUIMessageCenter> messageCenters = new Dictionary<string, GameUIMessageCenter>();

        /// <summary>
        /// 查找系统中的 UI 消息中心
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>找到则返回 UI 消息中心实例，否则返回null</returns>
        [LuaApiDescription("查找系统中的 UI 消息中心", "找到则返回 UI 消息中心实例，否则返回null")]
        [LuaApiParamDescription("name", "名字")]
        public static GameUIMessageCenter FindGameUIMessageCenter(string name) {
            if(!messageCenters.ContainsKey(name)) 
                return messageCenters[name];
            return null;
        }

        private Dictionary<string, GameUIControlValueBinder> valueBinders = new Dictionary<string, GameUIControlValueBinder>();
        private Dictionary<string, List<VoidDelegate>> events = new Dictionary<string, List<VoidDelegate>>();

        /// <summary>
        /// 注册数据更新器（该方法无需手动调用）
        /// </summary>
        /// <param name="binder"></param>
        /// <returns></returns>
        [LuaApiDescription("注册数据更新器（该方法无需手动调用）", "")]
        [LuaApiParamDescription("binder", "")]
        public bool RegisterValueBinder(GameUIControlValueBinder binder) {
            if(!valueBinders.ContainsKey(binder.Name)) {
                valueBinders[binder.Name] = binder;
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// 取消注册数据更新器（该方法无需手动调用）
        /// </summary>
        /// <param name="binder"></param>
        /// <returns></returns>
        [LuaApiDescription("取消注册数据更新器（该方法无需手动调用）", "")]
        [LuaApiParamDescription("binder", "")]
        public bool UnRegisterValueBinder(GameUIControlValueBinder binder) {
            if(valueBinders.ContainsKey(binder.Name)) {
                valueBinders.Remove(binder.Name);
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// 订阅数据更新器
        /// </summary>
        /// <param name="binderName">数据更新器名称</param>
        /// <param name="callbackFun">数据更新回调</param>
        /// <returns>返回一个可供更新数据的回调，调用此回调更新控件上的数据</returns>
        [LuaApiDescription("订阅数据更新器", "返回一个可供更新数据的回调，调用此回调更新控件上的数据")]
        [LuaApiParamDescription("binderName", "数据更新器名称")]
        [LuaApiParamDescription("callbackFun", "数据更新回调")]
        public GameUIControlValueBinderSupplierCallback SubscribeValueBinder(string binderName, GameUIControlValueBinderUserUpdateCallback callbackFun) {
            if(valueBinders.TryGetValue(binderName, out GameUIControlValueBinder binder)) {
                binder.UserUpdateCallbacks.Add(callbackFun);
                return binder.BinderSupplierCallback;
            }
            return null;
        }
        
        /// <summary>
        /// 取消订阅数据更新器
        /// </summary>
        /// <param name="binderName">数据更新器名称</param>
        /// <param name="callbackFun">数据更新回调</param>
        /// <returns>返回是否成功</returns>
        [LuaApiDescription("取消订阅数据更新器", "返回是否成功")]
        [LuaApiParamDescription("binderName", "数据更新器名称")]
        [LuaApiParamDescription("callbackFun", "数据更新回调")]
        public bool UnSubscribeValueBinder(string binderName, GameUIControlValueBinderUserUpdateCallback callbackFun) {
            if(valueBinders.TryGetValue(binderName, out GameUIControlValueBinder binder)) {
                binder.UserUpdateCallbacks.Remove(callbackFun);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 订阅单一消息
        /// </summary>
        /// <param name="evtName">消息名称</param>
        /// <param name="callBack">消息回调</param>
        [LuaApiDescription("订阅单一消息", "")]
        [LuaApiParamDescription("evtName", "消息名称")]
        [LuaApiParamDescription("callBack", "消息回调")]
        public void SubscribeEvent(string evtName, VoidDelegate callBack) {
            if(!events.TryGetValue(evtName, out var handlers)) {
                handlers = new List<VoidDelegate>();
                events.Add(evtName, handlers);
            }
            handlers.Add(callBack);
        }
        
        /// <summary>
        /// 取消订阅单一消息
        /// </summary>
        /// <param name="evtName">消息名称</param>
        /// <param name="callBack">消息回调</param>
        [LuaApiDescription("取消订阅单一消息", "")]
        [LuaApiParamDescription("evtName", "消息名称")]
        [LuaApiParamDescription("callBack", "消息回调")]
        public bool UnSubscribeEvent(string evtName, VoidDelegate callBack) {
            if(events.TryGetValue(evtName, out var handlers)) {
                handlers.Remove(callBack);
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// 发送单一消息
        /// </summary>
        /// <param name="evtName">消息名称</param>
        [LuaApiDescription("发送单一消息", "")]
        [LuaApiParamDescription("evtName", "消息名称")]
        public void NotifyEvent(string evtName) {
            if(events.TryGetValue(evtName, out var handlers)) 
                handlers.ForEach((h) => h.Invoke());
        }

        private GameLuaObjectHost GameLuaObjectHost;
        private LuaFunction LuaBinderBegin;
        private bool LuaBinderErrLogged = false;

        private void Start() {
            GameLuaObjectHost = GetComponent<GameLuaObjectHost>();
            messageCenters.Add(Name, this);
            if(GameLuaObjectHost != null) 
                GameLuaObjectHost.LuaInitFinished = InitLua;
        }
        private void InitLua() {
            LuaBinderBegin = GameLuaObjectHost.GetLuaFun("LuaBinderBegin");
        }
        private void OnDestroy() {
            messageCenters.Remove(Name);
        }

        [Tooltip("消息中心名字")]
        public string Name = "";

        /// <summary>
        /// 调用数据更新器的Lua处理器（勿手动调用该方法）
        /// </summary>
        /// <param name="binder"></param>
        [SLua.DoNotToLua]
        public void CallLuaBinderBegin(GameUIControlValueBinder binder) {
            if(LuaBinderBegin == null) {
                if(!LuaBinderErrLogged) {
                    Log.E("GameUIMessageCenter", "No lua or lua function LuaBinderBegin was found in this component. This message only log once in this component.");
                    LuaBinderErrLogged = true;
                }
                return;
            }
            LuaBinderBegin.call(GameLuaObjectHost.LuaSelf, binder.Name, binder);
        }
    }
}
