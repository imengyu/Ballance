using System.Collections.Generic;
using System.Text;
using Ballance2.Utils;
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
*/

namespace Ballance2.Services
{
  /// <summary>
  /// 系统设置操作回调
  /// </summary>
  
  
  public delegate bool GameSettingsCallback(string groupName, int action);

  /// <summary>
  /// 系统设置管理器
  /// </summary>
  public static class GameSettingsManager
  {
    private static Dictionary<string, GameSettingsActuator> settingsActuators = null;

    /// <summary>
    /// 获取设置执行器
    /// </summary>
    /// <param name="packageName">设置执行器所使用包名</param>
    /// <returns>返回设置执行器实例</returns>
    public static GameSettingsActuator GetSettings(string packageName, GameSettingsActuator customActuator = null)
    {
      GameSettingsActuator gameSettingsActuator = null;
      if (!settingsActuators.TryGetValue(packageName, out gameSettingsActuator))
      {
         if (customActuator != null) {
          settingsActuators.Add(packageName, gameSettingsActuator);
          return customActuator;
        }
        gameSettingsActuator = new GameSettingsActuator(packageName);
        settingsActuators.Add(packageName, gameSettingsActuator);
      }
      return gameSettingsActuator;
    }
    /// <summary>
    /// 还原默认设置
    /// </summary>
    public static void ResetDefaultSettings()
    {
      PlayerPrefs.DeleteAll();

      foreach (var actuator in settingsActuators)
        actuator.Value.NotifyAll();
    }

    internal static void ListActuators()
    {
      foreach (var i in settingsActuators)
        Log.V("GameSettingsManager", string.Format("{0} => {1}", i.Key, i.Value.ListCallbacks()));
    }
    internal static void Init()
    {
      if(settingsActuators == null)
        settingsActuators = new Dictionary<string, GameSettingsActuator>();
    }
    internal static void Destroy()
    {
      if(settingsActuators != null) {
        foreach (var key in settingsActuators.Keys)
          settingsActuators[key].Destroy();
        settingsActuators.Clear();
        settingsActuators = null;
      }
    }
  }

  /// <summary>
  /// 设置执行器，用于管理具体的设置条目
  /// </summary>
  public class GameSettingsActuator
  {
    /// <summary>
    /// 指示当前设置执行器执行更新操作
    /// </summary>
    public const int ACTION_UPDATE = 1;
    /// <summary>
    /// 指示当前设置执行器执行加载操作
    /// </summary>
    public const int ACTION_LOAD = 2;

    private string basePackName = "unknow";

    /// <summary>
    /// 创建设置执行器
    /// </summary>
    /// <param name="packageName">设置所在包名</param>
    public GameSettingsActuator(string packageName)
    {
      basePackName = packageName;
    }
    /// <summary>
    /// 设置整型设置条目
    /// </summary>
    /// <param name="key">设置键</param>
    /// <param name="value">设置值</param>
    public virtual void SetInt(string key, int value)
    {
      PlayerPrefs.SetInt(basePackName + "." + key, value);
    }
    /// <summary>
    /// 获取整型设置条目
    /// </summary>
    /// <param name="key">设置键</param>
    /// <param name="defaultValue">未找到设置时返回的默认值</param>
    /// <returns></returns>
    public virtual int GetInt(string key, int defaultValue)
    {
      return PlayerPrefs.GetInt(basePackName + "." + key, defaultValue);
    }
    /// <summary>
    /// 获取KeyCode型设置条目
    /// </summary>
    /// <param name="key">设置键</param>
    /// <param name="defaultValue">未找到设置时返回的默认值</param>
    /// <returns></returns>
    public virtual KeyCode GetKeyCode(string key, KeyCode defaultValue)
    {
      return (KeyCode)PlayerPrefs.GetInt(basePackName + "." + key, (int)defaultValue);
    }
    /// <summary>
    /// 设置字符串型设置条目
    /// </summary>
    /// <param name="key">设置键</param>
    /// <param name="value">设置值</param>
    public virtual void SetString(string key, string value)
    {
      PlayerPrefs.SetString(basePackName + "." + key, value);
    }
    /// <summary>
    /// 获取字符串型设置条目
    /// </summary>
    /// <param name="key">设置键</param>
    /// <param name="defaultValue">未找到设置时返回的默认值</param>
    /// <returns></returns>
    public virtual string GetString(string key, string defaultValue)
    {
      return PlayerPrefs.GetString(basePackName + "." + key, defaultValue);
    }
    /// <summary>
    /// 设置浮点型设置条目
    /// </summary>
    /// <param name="key">设置键</param>
    /// <param name="value">设置值</param>
    public virtual void SetFloat(string key, float value)
    {
      PlayerPrefs.SetFloat(basePackName + "." + key, value);
    }
    /// <summary>
    /// 获取浮点型设置条目
    /// </summary>
    /// <param name="key">设置键</param>
    /// <param name="defaultValue">未找到设置时返回的默认值</param>
    /// <returns></returns>
    public virtual float GetFloat(string key, float defaultValue)
    {
      return PlayerPrefs.GetFloat(basePackName + "." + key, defaultValue);
    }
    /// <summary>
    /// 设置布尔型设置条目
    /// </summary>
    /// <param name="key">设置键</param>
    /// <param name="value">设置值</param>
    public virtual void SetBool(string key, bool value)
    {
      PlayerPrefs.SetString(basePackName + "." + key, value.ToString());
    }
    /// <summary>
    /// 获取布尔型设置条目
    /// </summary>
    /// <param name="key">设置键</param>
    /// <param name="defaultValue">未找到设置时返回的默认值</param>
    /// <returns></returns>
    public virtual bool GetBool(string key, bool defaultValue = false)
    {
      return bool.Parse(PlayerPrefs.GetString(basePackName + "." + key, defaultValue.ToString()));
    }

    public virtual string GetString(string key)
    {
      return PlayerPrefs.GetString(basePackName + "." + key, "");
    }
    public virtual float GetFloat(string key)
    {
      return PlayerPrefs.GetFloat(basePackName + "." + key, 0);
    }    
    public virtual bool GetBool(string key)
    {
      return bool.Parse(PlayerPrefs.GetString(basePackName + "." + key, "False"));
    }
    public virtual int GetInt(string key)
    {
      return PlayerPrefs.GetInt(basePackName + "." + key, 0);
    }
    public virtual KeyCode GetKeyCode(string key)
    {
      return (KeyCode)GetInt(key);
    }

    private static int settingUpdateCallbackID = 0;
    private List<SettingUpdateCallbackData> settingUpdateCallbacks = new List<SettingUpdateCallbackData>();
    private struct SettingUpdateCallbackData
    {
      public string groupName;
      public GameSettingsCallback callback;
      public int id;

      public SettingUpdateCallbackData(string groupName, GameSettingsCallback callback)
      {
        this.id = settingUpdateCallbackID++;
        this.groupName = groupName;
        this.callback = callback;
      }
    }

    internal void NotifyAll()
    {
      foreach (var v in settingUpdateCallbacks)
        v.callback(v.groupName, ACTION_UPDATE);
    }
    internal string ListCallbacks()
    {
      StringBuilder sb = new StringBuilder("Callbacks: ");
      sb.Append(settingUpdateCallbacks.Count);
      sb.Append(" => ");
      foreach (var v in settingUpdateCallbacks)
      {
        sb.Append(",");
        sb.Append(v.groupName);
      }
      return sb.ToString();
    }
    internal void Destroy()
    {
      settingUpdateCallbacks.Clear();
    }

    /// <summary>
    /// 通知设置组加载更新
    /// </summary>
    /// <param name="groupName">组名称，为*表示所有</param>
    public void RequireSettingsLoad(string groupName)
    {
      if (groupName == "*")
      {
        foreach (var d in settingUpdateCallbacks)
          if (d.callback(d.groupName, ACTION_LOAD)) break;
      }
      else foreach (var d in settingUpdateCallbacks)
          if (d.groupName == groupName)
            if (d.callback(groupName, ACTION_LOAD)) break;
    }
    /// <summary>
    /// 通知设置组更新
    /// </summary>
    /// <param name="groupName">组名称，为*表示所有</param>
    public void NotifySettingsUpdate(string groupName)
    {
      if (groupName == "*")
      {
        foreach (var d in settingUpdateCallbacks)
          if (d.callback(d.groupName, ACTION_UPDATE)) break;
      }
      else foreach (var d in settingUpdateCallbacks)
          if (d.groupName == groupName)
            d.callback(groupName, ACTION_UPDATE);
    }
    /// <summary>
    /// 注册设置组更新回调
    /// </summary>
    /// <param name="groupName">组名称</param>
    /// <param name="handler">回调</param>
    public int RegisterSettingsUpdateCallback(string groupName, GameSettingsCallback callback)
    {
      var v = new SettingUpdateCallbackData(groupName, callback);
      settingUpdateCallbacks.Add(v);
      return v.id;
    }
    /// <summary>
    /// 取消注册设置组更新回调
    /// </summary>
    /// <param name="id">回调ID</param>
    public void UnRegisterSettingsUpdateCallback(int id)
    {
      for (var i = 0; i < settingUpdateCallbacks.Count; i++)
      {
        if (settingUpdateCallbacks[i].id == id)
        {
          settingUpdateCallbacks.RemoveAt(i);
          break;
        }
      }
    }
  }
}
