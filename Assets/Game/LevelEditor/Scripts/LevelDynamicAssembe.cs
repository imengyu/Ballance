using System.Collections;
using System.Collections.Generic;
using System.IO;
using Ballance2.Game.LevelBuilder;
using Ballance2.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace Ballance2.Game.LevelEditor
{
  [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
  public class LevelDynamicAssetData
  {
    [JsonProperty]
    public int LevelObjectId = 0;
    [JsonProperty]
    public List<LevelDynamicModelAsset> LevelAssets = new List<LevelDynamicModelAsset>();
    [JsonProperty]
    public List<LevelDynamicModel> LevelModels = new List<LevelDynamicModel>();

    public List<LevelDynamicModel> FindModulsByModulName(string objName)
    {
      var result = new List<LevelDynamicModel>();
      foreach (var item in LevelModels)
      {
        if (item.AssetRef.ObjName == objName)
          result.Add(item);
      }
      return result;
    }   
    public LevelDynamicModel FindModulByModulName(string objName)
    {
      foreach (var item in LevelModels)
      {
        if (item.AssetRef.ObjName == objName)
          return item;
      }
      return null;
    }
  }

  public class LevelDynamicAssembe
  {
    public readonly string LevelDirPath;
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
      LevelData = JsonConvert.DeserializeObject<LevelDynamicAssetData>(LevelEditorManager.Instance.LevelNewJson.text, new VectorConverter());
      Save();
    }
    public void Load()
    {
      LevelData = JsonConvert.DeserializeObject<LevelDynamicAssetData>(File.ReadAllText($"{LevelDirPath}/assets.json"), new VectorConverter());
      LevelInfo = JsonConvert.DeserializeObject<LevelJson>(File.ReadAllText($"{LevelDirPath}/level.json"), new VectorConverter());
    }
    public void Save()
    {
      if (!Directory.Exists(LevelDirPath)) Directory.CreateDirectory(LevelDirPath);
      if (!Directory.Exists(LevelDirPath + "/moduls")) Directory.CreateDirectory(LevelDirPath + "/moduls");
      if (!Directory.Exists(LevelDirPath + "/assets")) Directory.CreateDirectory(LevelDirPath + "/assets");
      if (!Directory.Exists(LevelDirPath + "/screenshot")) Directory.CreateDirectory(LevelDirPath + "/screenshot");

      foreach (var item in LevelData.LevelModels)
      {
        item.ConfigueRef.OnSave(item);
        item.Save();
      }

      File.WriteAllText($"{LevelDirPath}/assets.json", JsonConvert.SerializeObject(LevelData, new VectorConverter()));
      File.WriteAllText($"{LevelDirPath}/level.json", JsonConvert.SerializeObject(LevelInfo, new VectorConverter()));
    }

    public int GetSectorCount()
    {
      return LevelInfo.level.sectorCount;
    }  
    public void SetSectorCountToFitModuls()
    {
      var sectorCount = 0;
      foreach (var model in LevelData.LevelModels)
      {
        if (model.Asset == "core:PS_FourFlames")
          sectorCount++;
        else if (model.Asset == "core:PC_TwoFlames")
          sectorCount++;
      }
      LevelInfo.level.sectorCount = sectorCount;
    }

  }
}
