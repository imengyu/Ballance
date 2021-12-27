
using Puerts;
using System.IO;
using System.Text;
using UnityEngine;

namespace Ballance2.Services.JSService.JSLoader {


  class GameJSLoader : ILoader
  {
    private GamePackageManager _GamePackageManager = null;
    private GamePackageManager GamePackageManager {
      get {
        if(_GamePackageManager == null)
          _GamePackageManager = GameManager.Instance.GetSystemService<GamePackageManager>();
        return _GamePackageManager;
      }
    }

    public bool FileExists(string filepath)
    {
      if(filepath.StartsWith("puerts/"))
        return Resources.Load<TextAsset>(filepath) != null;
      return GamePackageManager.CheckCodeAssetExists(filepath);
    }
    public string ReadFile(string filepath, out string debugpath)
    { 
      if(filepath.StartsWith("puerts/")) {
#if UNITY_EDITOR
        debugpath = Directory.GetCurrentDirectory() + "/Assets/Plugins/Puerts/Src/Resources/" + filepath;
#else
        debugpath = "puerts://" + filepath;
#endif
        return Resources.Load<TextAsset>(filepath).text;
      }
      return Encoding.UTF8.GetString(GamePackageManager.GetCodeAsset(filepath, out debugpath));
    }
  }


}