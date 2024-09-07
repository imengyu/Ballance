using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ballance2.Game.GamePlay
{
  /// <summary>
  /// 用于 高分管理器生成默认数据
  /// </summary>
  public static class DefaultHighscoreDataGenerator {

    /// <summary>
    /// 默认关卡名字数据
    /// </summary>
    /// <value></value>
    public static readonly string[] DefaultHighscoreLevelNamesData = new string[] {
      "Level01",
      "Level02",
      "Level03",
      "Level04",
      "Level05",
      "Level06",
      "Level07",
      "Level08",
      "Level09",
      "Level10",
      "Level11",
      "Level12",
      "Level13"
    };

    private static Dictionary<string, List<HighscoreDataItem>> _DefaultHighscoreData = null;

    /// <summary>
    /// 默认关卡高分数据
    /// </summary>
    /// <value></value>
    public static List<HighscoreDataItem> DefaultHightScoreLev01To11Data {
      get {
        var data = new List<HighscoreDataItem>();
        data.Add(new HighscoreDataItem("Mr. Default", 4000, "2004/8/8"));
        data.Add(new HighscoreDataItem("Mr. Default", 3600, "2004/8/8"));
        data.Add(new HighscoreDataItem("Mr. Default", 3200, "2004/8/8"));
        data.Add(new HighscoreDataItem("Mr. Default", 2800, "2004/8/8"));
        data.Add(new HighscoreDataItem("Mr. Default", 2400, "2004/8/8"));
        data.Add(new HighscoreDataItem("Mr. Default", 2000, "2004/8/8"));
        data.Add(new HighscoreDataItem("Mr. Default", 1600, "2004/8/8"));
        data.Add(new HighscoreDataItem("Mr. Default", 1200, "2004/8/8"));
        data.Add(new HighscoreDataItem("Mr. Default", 800, "2004/8/8"));
        data.Add(new HighscoreDataItem("Mr. Default", 400, "2004/8/8"));
        return data;
      }
    }
    /// <summary>
    /// 12关高分数据
    /// </summary>
    /// <value></value>
    public static List<HighscoreDataItem> DefaultHightScoreLev12Data {
      get {
        var data = new List<HighscoreDataItem>();
        data.Add(new HighscoreDataItem("Mr. Default", 7000, "2004/8/8"));
        data.Add(new HighscoreDataItem("Mr. Default", 6600, "2004/8/8"));
        data.Add(new HighscoreDataItem("Mr. Default", 6200, "2004/8/8"));
        data.Add(new HighscoreDataItem("Mr. Default", 5800, "2004/8/8"));
        data.Add(new HighscoreDataItem("Mr. Default", 5400, "2004/8/8"));
        data.Add(new HighscoreDataItem("Mr. Default", 5000, "2004/8/8"));
        data.Add(new HighscoreDataItem("Mr. Default", 4600, "2004/8/8"));
        data.Add(new HighscoreDataItem("Mr. Default", 4200, "2004/8/8"));
        data.Add(new HighscoreDataItem("Mr. Default", 3800, "2004/8/8"));
        data.Add(new HighscoreDataItem("Mr. Default", 3600, "2004/8/8"));
        return data;
      }
    }
    /// <summary>
    /// 所有的默认关卡高分数据
    /// </summary>
    /// <value></value>
    public static Dictionary<string, List<HighscoreDataItem>> DefaultHighscoreData {
      get {
        if (_DefaultHighscoreData != null)
          return _DefaultHighscoreData;
        _DefaultHighscoreData = new Dictionary<string, List<HighscoreDataItem>>();
        for(int i = 1; i <= 11; i++)
          _DefaultHighscoreData.Add($"Level{(i < 10 ? '0' : "")}{i}", DefaultHightScoreLev01To11Data);
        _DefaultHighscoreData.Add($"Level12", DefaultHightScoreLev12Data);
        _DefaultHighscoreData.Add($"Level13", DefaultHightScoreLev01To11Data);
        return _DefaultHighscoreData;
      }
    }
  }

  [JsonObject]
  public class HighscoreDataItem 
  {
    public HighscoreDataItem(string name, int score, string date) 
    {
      this.name = name;
      this.score = score;
      this.date = date;
    }

    [JsonProperty]
    public string name { get; set; }
    [JsonProperty]
    public int score { get; set; }
    [JsonProperty]
    public string date { get; set; }
  }
}