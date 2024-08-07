using System.Collections;
using System.Collections.Generic;
using System.IO;
using Ballance2.Game.LevelBuilder;
using Ballance2.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace Ballance2.Game.LevelEditor
{
  [JsonObject]
  public class LevelDynamicAssetData
  {
    public int LevelObjectId = 0;
    public List<LevelDynamicModelAsset> LevelAssets = new List<LevelDynamicModelAsset>();
    public List<LevelDynamicModel> LevelModels = new List<LevelDynamicModel>();
  }
  public class LevelDynamicAssembe
  {
    public string LevelDirPath;
    public LevelJson LevelInfo;
    public LevelDynamicAssetData LevelData;

    public LevelDynamicAssembe(string path)
    {
      LevelDirPath = path;  
    }

    public void New()
    {
      LevelInfo = new LevelJson();
      LevelInfo.name = Path.GetFileName(LevelDirPath);
      LevelData = JsonConvert.DeserializeObject<LevelDynamicAssetData>(LevelEditorManager.Instance.LevelNewJson.text);
      Save();
    }
    public void Load()
    {
      LevelData = JsonConvert.DeserializeObject<LevelDynamicAssetData>(File.ReadAllText($"{LevelDirPath}/assets.json"));
      LevelInfo = JsonConvert.DeserializeObject<LevelJson>(File.ReadAllText($"{LevelDirPath}/level.json"));
    }
    public void Save()
    {
      if (!Directory.Exists(LevelDirPath)) Directory.CreateDirectory(LevelDirPath);
      if (!Directory.Exists(LevelDirPath + "/moduls")) Directory.CreateDirectory(LevelDirPath + "/moduls");

      foreach (var item in LevelData.LevelModels)
        item.Save();

      File.WriteAllText($"{LevelDirPath}/assets.json", JsonConvert.SerializeObject(LevelData, new VectorConverter()));
      File.WriteAllText($"{LevelDirPath}/level.json", JsonConvert.SerializeObject(LevelInfo, new VectorConverter()));
    }
  }
}
