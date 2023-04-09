using System;
using System.Collections.Generic;
using System.IO;
using Ballance2.Base;
using Ballance2.Package;
using Ballance2.Services;
using Ballance2.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Ballance2.Game.GamePlay
{
  public class HighscoreManager : GameSingletonBehavior<HighscoreManager>
  {
    private const string TAG = "HighscoreManager";
    private int HighscoreManagerCommand = 0;

    private Dictionary<string, List<HighscoreDataItem>> Data = null;
    private Dictionary<string, bool> LevelPassStateData = null;
    private List<string> LevelNames = null;

    private string HighscoreDataPath;
    private string LevelPassStateDataPath;

    private void Start() {
      //指令
      InitCommand();
    }

    //加载。此函数由系统自动调用
    internal void Load() {
      HighscoreDataPath = Application.persistentDataPath + "/HighscoreData.json";
      LevelPassStateDataPath = Application.persistentDataPath + "/LevelPassStateData.json";
      
      //加载关卡高分数据
      if (GameManager.Instance.FileExists(HighscoreDataPath)) {
        using (StreamReader reader = File.OpenText(HighscoreDataPath))
        {
          JToken obj = JToken.ReadFrom(new JsonTextReader(reader));
          JToken dataObj = JToken.Parse(obj["data"].Value<string>());
          JToken namesObj = JToken.Parse(obj["names"].Value<string>());
          // do stuff
          if (dataObj.HasValues) {
            Data = dataObj.ToObject<Dictionary<string, List<HighscoreDataItem>>>();
            LevelNames = namesObj.ToObject<List<string>>();
          } else {
            Data = DefaultHighscoreDataGenerator.DefaultHighscoreData;
            LevelNames = new List<string>(DefaultHighscoreDataGenerator.DefaultHighscoreLevelNamesData);
          }
        }
      }
      else
      {
        Data = DefaultHighscoreDataGenerator.DefaultHighscoreData;
        LevelNames = new List<string>(DefaultHighscoreDataGenerator.DefaultHighscoreLevelNamesData);
      }
      //加载关卡过关状态数据
      if (GameManager.Instance.FileExists(LevelPassStateDataPath))
      { 
        using (StreamReader reader = File.OpenText(LevelPassStateDataPath))
        {
          JToken data = JToken.ReadFrom(new JsonTextReader(reader));
          LevelPassStateData = data.ToObject<Dictionary<string, bool>>();
        }
      } else {
        LevelPassStateData = new Dictionary<string, bool>();
      }
    }
    //保存。此函数由系统自动调用
    internal void Save() {
      //保存关卡高分数据
      JObject obj = new JObject();
      obj["data"] = JsonConvert.SerializeObject(Data);
      obj["names"] = JsonConvert.SerializeObject(LevelNames);
      GameManager.Instance.WriteFile(HighscoreDataPath, false, obj.ToString());

      //保存关卡过关状态数据
      GameManager.Instance.WriteFile(LevelPassStateDataPath, false, JsonConvert.SerializeObject(LevelPassStateData));
    }

    private static GameObject HighscoreManagerGameObject = null;

    internal static void Init() { 
      HighscoreManagerGameObject = CloneUtils.CreateEmptyObject("GameHighscoreManager");
      HighscoreManagerGameObject.AddComponent<HighscoreManager>();
    }
    internal static void Destroy() {
      if (HighscoreManagerGameObject != null) 
        UnityEngine.Object.Destroy(HighscoreManagerGameObject);
      HighscoreManagerGameObject = null;
    }

    //添加指令
    private void InitCommand() {
      if (HighscoreManagerCommand == 0) {
        HighscoreManagerCommand = GameManager.Instance.GameDebugCommandServer.RegisterCommand("highscore", (keyword, fullCmd, argsCount, args) => {
          var type = args[0];
          if (type == "clear") {
            Data = DefaultHighscoreDataGenerator.DefaultHighscoreData;
            LevelNames = new List<string>(DefaultHighscoreDataGenerator.DefaultHighscoreLevelNamesData);
            LevelPassStateData.Clear();
            Log.D(TAG, "Reset to default");
            return true;
          }
          else if (type == "open-all") {
            Log.D(TAG, "Open level01-level12");
            UnLockAllInternalLevel();
            return true;
          }
          else if (type == "load") { 
            Load();
            Log.D(TAG, " Load manually");
            return true;
          }
          else if (type == "save") { 
            Save();
            Log.D(TAG, "Save manually");
            return true;
          }
          else {
            Log.E(TAG, "Unknow type " + type);
            return false;
          }
        }, 1, "highscore <clear/open-all/load/save>\n"
           + "  clear    ▶ 清除当前本地存储的所有高分数据（慎用！不可恢复！）\n"
           + "  open-all ▶ 解锁全部内置关卡\n"
           + "  load     ▶ 手动加载高分数据\n"
           + "  save     ▶ 手动保存高分数据");
      }
    } 

    /// <summary>
    /// 解锁全部内置关卡
    /// </summary>
    public void UnLockAllInternalLevel() {
      foreach(var key in DefaultHighscoreDataGenerator.DefaultHighscoreLevelNamesData)
        LevelPassStateData[key] = true;
    }
    /// <summary>
    /// 获取指定关卡的分数列表
    /// </summary>
    /// <param name="levelName">关卡名称</param>
    /// <returns>指定关卡的分数列表，如果没有，则返回null</returns>
    public List<HighscoreDataItem> GetData(string levelName) {
      if (Data.TryGetValue(levelName, out var list))
        return list;
      return null;
    }
    /// <summary>
    /// 获取分数管理器中有存储数据的所有关卡名称
    /// </summary>
    /// <param name="levelName">关卡名称</param>
    /// <returns>所有关卡名称数组</returns>
    public List<string> GetLevelNames() {
      return LevelNames;
    }
    /// <summary>
    /// 在指定关卡添加用户的分数数据
    /// </summary>
    /// <param name="levelName">关卡名称</param>
    /// <param name="userName">名字</param>
    /// <param name="score">分数</param>
    public void AddItem(string levelName, string userName, int score) {
      //获取或者创建数据
      var list = GetData(levelName);
      if (list == null) {
        list = new List<HighscoreDataItem>();
        Data[levelName] = list;
      }
      
      //设置已经过关
      AddLevelPassState(levelName);

      //插入数据
      HighscoreDataItem item = new HighscoreDataItem(userName, score, DateTime.Now.ToString("yyyy/M/d"));

      for (var i = 0; i < list.Count; i++)
      {
        //插入到指定位置
        if (score > list[i].score) {
          list.Insert(i, item);
          return;
        }
      }
      //插入到最后
      list.Add(item);
    }
    /// <summary>
    /// 检查指定分数是否在关卡有新的高分
    /// </summary>
    /// <param name="levelName">关卡名称</param>
    /// <param name="score">分数</param>
    /// <returns>是否有新的高分</returns>
    public bool CheckLevelHighScore(string levelName, int score) {
      var list = GetData(levelName);
      if (list == null)
        return true;
      foreach (var data in list) {
        if (data.score > score)
          return false;
      }
      return true;
    }
    /// <summary>
    /// 设置指定关卡已经过关
    /// </summary>
    /// <param name="levelName">关卡名称</param>
    public void AddLevelPassState(string levelName) {
      LevelPassStateData[levelName] = true;
    }
    /// <summary>
    /// 检查指定关卡是否有过关
    /// </summary>
    /// <param name="levelName">关卡名称</param>
    /// <returns></returns>
    public bool CheckLevelPassState(string levelName) {
      if (LevelPassStateData.TryGetValue(levelName, out var pass))
        return pass;
      return false;
    }
    /// <summary>
    /// 添加默认分数至指定关卡的分数数据中
    /// </summary>
    /// <param name="levelName">关卡名称</param>
    /// <param name="data">默认数据，可为 null null 时根据在 DefaultHighscoreData 中定义的 defaultHighscoreData.DefaultHightScoreLev01_11Data 加载数据</param>
    public void TryAddDefaultLevelHighScore(string levelName, List<HighscoreDataItem> data) {
      if (!Data.ContainsKey(levelName)) {
        Data[levelName] = data != null ? data : DefaultHighscoreDataGenerator.DefaultHightScoreLev01To11Data;
        LevelNames.Add(levelName);
      }
    }
  }
}