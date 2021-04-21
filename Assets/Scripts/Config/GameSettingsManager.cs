using System.Collections.Generic;
using Ballance.LuaHelpers;
using UnityEngine;

/*
* Copyright (c) 2020  mengyu
* 
* 模块名：     
* GameSettingsManager.cs
* 
* 用途：
* 游戏设置管理器，管理整个游戏的设置
* 
* 通过 GameSettingsManager.GetSettings("com.your.packagename") 获取设置执行器
* 设置执行器可调用 RegisterSettingsUpdateCallback 注册设置更改信息
* UI通过 NotifySettingsUpdate 通知设置更改信息
* 
* 作者：
* mengyu
* 
* 更改历史：
* 2020-1-12 创建
* 
*/

namespace Ballance2.Config
{
    /// <summary>
    /// 系统设置操作回调
    /// </summary>
    [SLua.CustomLuaClass]
    [LuaApiDescription("系统设置操作回调")]
    public delegate bool GameSettingsCallback(string groupName, int action);

    /// <summary>
    /// 系统设置管理器
    /// </summary>
    [SLua.CustomLuaClass]
    [LuaApiDescription("系统设置管理器")]
    public static class GameSettingsManager
    {
        private static Dictionary<string, GameSettingsActuator> settingsActuators = null;

        /// <summary>
        /// 获取设置执行器
        /// </summary>
        /// <param name="packageName">设置执行器所使用包名</param>
        /// <returns></returns>
        [LuaApiDescription("获取设置执行器")]
        [LuaApiParamDescription("packageName", "设置执行器所使用包名")]
        public static GameSettingsActuator GetSettings(string packageName)
        {
            GameSettingsActuator gameSettingsActuator = null;
            if (!settingsActuators.TryGetValue(packageName, out gameSettingsActuator))
            {
                gameSettingsActuator = new GameSettingsActuator(packageName);
                settingsActuators.Add(packageName, gameSettingsActuator);
            }
            return gameSettingsActuator;
        }
        /// <summary>
        /// 还原默认设置
        /// </summary>
        [LuaApiDescription("还原默认设置")]
        public static void ResetDefaultSettings()
        {
            PlayerPrefs.DeleteAll();
        }

        internal static void Init()
        {
            settingsActuators = new Dictionary<string, GameSettingsActuator>();
        }
        internal static void Destroy()
        {
            foreach(var key in settingsActuators.Keys)
                settingsActuators[key].Destroy();
            settingsActuators.Clear();
            settingsActuators = null;
        }
    }

    /// <summary>
    /// 设置执行器
    /// </summary>
    [SLua.CustomLuaClass]
    [LuaApiDescription("设置执行器")]
    public class GameSettingsActuator
    {
        /// <summary>
        /// 指示当前设置执行器执行更新操作
        /// </summary>
        [LuaApiDescription("指示当前设置执行器执行更新操作")]
        public const int ACTION_UPDATE = 1;
        /// <summary>
        /// 指示当前设置执行器执行加载操作
        /// </summary>
        [LuaApiDescription("指示当前设置执行器执行加载操作")]
        public const int ACTION_LOAD = 2;

        private string basePackName = "unknow";

        /// <summary>
        /// 创建设置执行器
        /// </summary>
        /// <param name="packageName">设置所在包名</param>
        [LuaApiDescription("创建设置执行器")]
        [LuaApiParamDescription("packageName", "设置所在包名")]
        public GameSettingsActuator(string packageName)
        {
            basePackName = packageName;
        }

        [LuaApiDescription("设置整型设置条目")]
        [LuaApiParamDescription("key", "设置键")]
        [LuaApiParamDescription("value", "设置值")]
        public virtual void SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(basePackName + "." + key, value);
        }
        [LuaApiDescription("获取整型设置条目")]
        [LuaApiParamDescription("key", "设置键")]
        [LuaApiParamDescription("defaultValue", "未找到设置时返回的默认值")]
        public virtual int GetInt(string key, int defaultValue)
        {
            return PlayerPrefs.GetInt(basePackName + "." + key, defaultValue);
        }
        [LuaApiDescription("设置字符串型设置条目")]
        [LuaApiParamDescription("key", "设置键")]
        [LuaApiParamDescription("value", "设置值")]
        public virtual void SetString(string key, string value)
        {
            PlayerPrefs.SetString(basePackName + "." + key, value);
        }
        [LuaApiDescription("获取字符串型设置条目")]
        [LuaApiParamDescription("key", "设置键")]
        [LuaApiParamDescription("defaultValue", "未找到设置时返回的默认值")]
        public virtual string GetString(string key, string defaultValue)
        {
            return PlayerPrefs.GetString(basePackName + "." + key, defaultValue);
        }
        [LuaApiDescription("设置浮点型设置条目")]
        [LuaApiParamDescription("key", "设置键")]
        [LuaApiParamDescription("value", "设置值")]
        public virtual void SetFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(basePackName + "." + key, value);
        }
        [LuaApiDescription("获取浮点型设置条目")]
        [LuaApiParamDescription("key", "设置键")]
        [LuaApiParamDescription("defaultValue", "未找到设置时返回的默认值")]
        public virtual float GetFloat(string key, float defaultValue)
        {
            return PlayerPrefs.GetFloat(basePackName + "." + key, defaultValue);
        }
        [LuaApiDescription("设置布尔型设置条目")]
        [LuaApiParamDescription("key", "设置键")]
        [LuaApiParamDescription("value", "设置值")]
        public virtual void SetBool(string key, bool value)
        {
            PlayerPrefs.SetString(basePackName + "." + key, value.ToString());
        }
        [LuaApiDescription("获取布尔型设置条目")]
        [LuaApiParamDescription("key", "设置键")]
        [LuaApiParamDescription("defaultValue", "未找到设置时返回的默认值")]
        public virtual bool GetBool(string key, bool defaultValue = false)
        {
            return bool.Parse(PlayerPrefs.GetString(basePackName + "." + key, defaultValue.ToString()));
        }

        private List<SettingUpdateCallbackData> settingUpdateCallbacks = new List<SettingUpdateCallbackData>();
        private struct SettingUpdateCallbackData
        {
            public string groupName;
            public GameSettingsCallback callback;

            public SettingUpdateCallbackData(string groupName, GameSettingsCallback callback)
            {
                this.groupName = groupName;
                this.callback = callback;
            }
        }

        internal void Destroy()
        {
            if (settingUpdateCallbacks != null)
            {
                settingUpdateCallbacks.Clear();
                settingUpdateCallbacks = null;
            }
        }

        /// <summary>
        /// 通知设置组加载更新
        /// </summary>
        /// <param name="groupName">组名称</param>
        [LuaApiDescription("通知设置组加载更新")]
        [LuaApiParamDescription("groupName", "组名称")]
        public void RequireSettingsLoad(string groupName)
        {
            foreach (var d in settingUpdateCallbacks)
                if (d.groupName == groupName)
                    if (d.callback(groupName, ACTION_LOAD)) break;
        }
        /// <summary>
        /// 通知设置组更新
        /// </summary>
        /// <param name="groupName">组名称</param>
        [LuaApiDescription("通知设置组更新")]
        [LuaApiParamDescription("groupName", "组名称")]
        public void NotifySettingsUpdate(string groupName)
        {
            foreach (var d in settingUpdateCallbacks)
                if (d.groupName == groupName)
                    d.callback(groupName, ACTION_UPDATE);
        }
        /// <summary>
        /// 注册设置组更新回调
        /// </summary>
        /// <param name="groupName">组名称</param>
        /// <param name="handler">回调</param>
        [LuaApiDescription("注册设置组更新回调")]
        [LuaApiParamDescription("groupName", "组名称")]
        [LuaApiParamDescription("handler", "回调")]
        public void RegisterSettingsUpdateCallback(string groupName, GameSettingsCallback callback)
        {
            settingUpdateCallbacks.Add(new SettingUpdateCallbackData(groupName, callback));
        }
    }
}
