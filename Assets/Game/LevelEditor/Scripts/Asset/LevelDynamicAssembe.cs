using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Ballance2.Game.LevelBuilder;
using Ballance2.Game.LevelEditor.Exceptions;
using Ballance2.Utils;
using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;
using UnityEngine;
using static Ballance2.Game.LevelEditor.LevelDynamicLoader;

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

    public bool IsZip = false;

    public LevelDynamicAssembe(string path)
    {
      LevelDirPath = path;
    }

    public async Task New()
    {
      LevelInfo = new LevelJson();
      LevelInfo.name = Path.GetFileName(LevelDirPath);
      LevelData = JsonConvert.DeserializeObject<LevelDynamicAssetData>(LevelEditorManager.Instance.LevelNewJson.text, new VectorConverter());
      await Save();
    }
    public async Task<LevelDynamicLoaderResult> Load()
    {
      var result = new LevelDynamicLoaderResult();
      try
      {
        if (LevelDirPath.EndsWith(".blevel") && File.Exists(LevelDirPath))
        {
          IsZip = true;
          var zip = ZipUtils.OpenZipFile(LevelDirPath);
          ZipEntry theEntry;
          while ((theEntry = zip.GetNextEntry()) != null)
          {
            if (ZipUtils.MatchRootName("assets.json", theEntry))
              LevelData = JsonConvert.DeserializeObject<LevelDynamicAssetData>(await ZipUtils.LoadStringInZip(zip, theEntry));
            else if (ZipUtils.MatchRootName("Level.json", theEntry))
              LevelInfo = JsonConvert.DeserializeObject<LevelJson>(await ZipUtils.LoadStringInZip(zip, theEntry));
          }

          zip.Close();
          zip.Dispose();
        }
        else if (Directory.Exists(LevelDirPath))
        {
          IsZip = false;
          LevelData = JsonConvert.DeserializeObject<LevelDynamicAssetData>(await File.ReadAllTextAsync($"{LevelDirPath}/assets.json"), new VectorConverter());
          LevelInfo = JsonConvert.DeserializeObject<LevelJson>(await File.ReadAllTextAsync($"{LevelDirPath}/level.json"), new VectorConverter());
        }
        result.Success = true;
      }
      catch (Exception e)
      {
        HandleExceptionAndGetMessages(e, result);
      }
      return result;
    }
    public async Task<LevelDynamicLoaderResult> Save()
    {
      var result = new LevelDynamicLoaderResult();
      try
      {
        if (LevelDirPath.EndsWith(".blevel") && File.Exists(LevelDirPath))
          throw new LevelDynamicLoaderPackLevelAlreadyPackedException();
        if (!Directory.Exists(LevelDirPath)) Directory.CreateDirectory(LevelDirPath);
        if (!Directory.Exists(LevelDirPath + "/assets")) Directory.CreateDirectory(LevelDirPath + "/assets");
        if (!Directory.Exists(LevelDirPath + "/screenshot")) Directory.CreateDirectory(LevelDirPath + "/screenshot");

        foreach (var item in LevelData.LevelModels)
        {
          if (item.ConfigueRef != null)
            item.ConfigueRef.OnSave(item);
          item.Save();
        }

        await File.WriteAllTextAsync($"{LevelDirPath}/assets.json", JsonConvert.SerializeObject(LevelData, new VectorConverter()));
        await File.WriteAllTextAsync($"{LevelDirPath}/level.json", JsonConvert.SerializeObject(LevelInfo, new VectorConverter()));
        result.Success = true;
      }
      catch (Exception e)
      {
        HandleExceptionAndGetMessages(e, result);
      }
      return result;
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
