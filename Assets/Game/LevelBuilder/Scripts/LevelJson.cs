
using System;
using System.Collections.Generic;
using Ballance2.Game.GamePlay;
using Newtonsoft.Json;
using static Ballance2.Game.LevelBuilder.LevelBuilder;

namespace Ballance2.Game.LevelBuilder
{
  /// <summary>
  /// 关卡JSON定义
  /// </summary>
  [JsonObject]
  public class LevelJson {
    [JsonProperty]
    public string name = "";
    [JsonProperty]
    public string author = "";
    [JsonProperty]
    public string version = "1.0";
    [JsonProperty]
    public string introduction = "";
    [JsonProperty]
    public string url = "";
    [JsonProperty]
    public bool allowPreview = true;
    [JsonProperty]
    public List<GameLevelDependencies> requiredPackages = new List<GameLevelDependencies>();
    [JsonProperty]
    public LevelData level = new LevelData();
  }
  [JsonObject]
  [System.Serializable]
  public class GameLevelDependencies
  {
    public string name;
    public int minVersion;

    [JsonIgnore]
    public string loaded { get;set; }
  }
  [JsonObject]
  public class LevelData {
    [JsonProperty]
    public string firstBall = "BallWood";
    [JsonProperty]
    public int levelScore = 100;
    [JsonProperty]
    public int startPoint = 1000;
    [JsonProperty]
    public int startLife = 3;
    [JsonProperty]
    public int musicTheme = 0;
    [JsonProperty]
    public string skyBox = "A";
    [JsonProperty]
    public string skyLayer = "";
    [JsonProperty]
    public string nextLevel = "";
    [JsonProperty]
    public MusicThemeDataDefine customMusicTheme;
    [JsonProperty]
    public LevelCustomSkyBoxData customSkyBox;
    [JsonProperty]
    public List<HighscoreDataItem> defaultHighscoreData;
    [JsonProperty]
    public string lightColor = "#ffffff";
    [JsonProperty]
    public string customModEventName = "";
    [JsonProperty]
    public int sectorCount = 1;
    [JsonProperty]
    public bool endWithUFO = false;
    [JsonProperty]
    public bool autoGroup = false;
    [JsonProperty]
    public LevelInternalObjectsData internalObjects;
    [JsonProperty]
    public Dictionary<string, List<string>> sectors;
    [JsonProperty]
    public List<LevelGroupData> floors;
    [JsonProperty]
    public List<string> depthTestCubes;
    [JsonProperty]
    public List<LevelGroupData> groups;
  }

  [JsonObject]
  public class MusicThemeDataDefine {
    [JsonProperty]
    public int id = 0;
    [JsonProperty]
    public float maxInterval = 5;
    [JsonProperty]
    public float baseInterval = 30;
    [JsonProperty]
    public float atmoInterval = 6;
    [JsonProperty]
    public float atmoMaxInterval = 15;
    [JsonProperty]
    public List<string> musics = null;
    [JsonProperty]
    public List<string> atmos = null;
  }

  [JsonObject]
  public class LevelInternalObjectsData {
    [JsonProperty]
    public string PS_LevelStart;
    [JsonProperty]
    public LevelBuilderModulRotationCorrecting PS_LevelStartRotationCorrecting;
    [JsonProperty]
    public LevelBuilderModulRotationCorrecting PC_CheckPointsRotationCorrecting;
    [JsonProperty]
    public string PE_LevelEnd;
    [JsonProperty]
    public LevelBuilderModulRotationCorrecting PE_LevelEndRotationCorrecting;
    [JsonProperty]
    public Dictionary<string, string> PC_CheckPoints;
    [JsonProperty]
    public Dictionary<string, string> PR_ResetPoints;
  }
  [JsonObject]
  public class LevelGroupData {
    public string name;
    [JsonProperty]
    public LevelBuilderModulRotationCorrecting rotationCorrecting;
    [JsonProperty]
    public List<string> objects;
  }
  [JsonObject]
  public class LevelCustomSkyBoxData {
    [JsonProperty]
    public string B;
    [JsonProperty]
    public string F;
    [JsonProperty]
    public string L;
    [JsonProperty]
    public string R;
    [JsonProperty]
    public string T;
    [JsonProperty]
    public string D;
  }
  

}