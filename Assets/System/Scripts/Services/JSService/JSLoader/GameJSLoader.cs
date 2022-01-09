
using Puerts;
using System.IO;
using System.Text;
using UnityEngine;

namespace Ballance2.Services.JSService.JSLoader {

  /// <summary>
  /// js 代码加载器
  /// </summary>
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

    private string PathToUse(string filepath)
    {
      return 
        // .cjs asset is only supported in unity2018+
        filepath.EndsWith(".cjs") || filepath.EndsWith(".mjs")  ? 
            filepath.Substring(0, filepath.Length - 4) : 
            filepath;
    }

    public bool FileExists(string filepath)
    {
      if(filepath.StartsWith("puerts/"))
        return true;
      return GamePackageManager.CheckCodeAssetExists(filepath);
    }
    public string ReadFile(string filepath, out string debugpath)
    { 
      if(filepath.StartsWith("puerts/")) {
#if UNITY_EDITOR
        debugpath = Directory.GetCurrentDirectory() + "/Assets/Plugins/Puerts/Src/Resources/" + filepath;
        return File.ReadAllText(debugpath);
#else
        debugpath = "puerts://" + filepath;
        TextAsset asset = Resources.Load<TextAsset>(PathToUse(filepath));
        if(asset == null)
          throw new FileNotFoundException("Filed to load js file: \"" + filepath + "\" !");
        return asset.text;
#endif
      }

      //PACK
      var code = GamePackageManager.GetCodeAsset(filepath, out var pack);
      debugpath = code.realPath;
      return Encoding.UTF8.GetString(code.data);
    }
  }


}