
using Puerts;
using System;
using System.Text;

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
      return _GamePackageManager.CheckCodeAssetExists(filepath);
    }
    public string ReadFile(string filepath, out string debugpath)
    {
      return Encoding.UTF8.GetString(_GamePackageManager.GetCodeAsset(filepath, out debugpath));
    }
  }


}