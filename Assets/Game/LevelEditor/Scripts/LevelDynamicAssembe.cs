using System.Collections;
using System.Collections.Generic;
using System.IO;
using Ballance2.Game.LevelBuilder;
using Newtonsoft.Json;
using UnityEngine;

namespace Ballance2.Game.LevelEditor
{
  public class LevelDynamicAssembe
  {
    public string LevelDirPath;
    public LevelJson LevelInfo;
    public List<LevelDynamicModel> LevelModels = new List<LevelDynamicModel>();

    public void New(string path)
    {
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);

      LevelInfo = new LevelJson();
      LevelInfo.name = Path.GetFileName(path);
      LevelModels = new List<LevelDynamicModel>();

      


      File.WriteAllText($"{path}/Level.json", JsonConvert.SerializeObject(LevelInfo));
    }
    public void Load(string path)
    {

    }
  }
}
