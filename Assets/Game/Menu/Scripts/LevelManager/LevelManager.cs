using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Ballance2.Base;
using Ballance2.Res;
using Ballance2.Services;
using Ballance2.Game.LevelBuilder;
using Ballance2.Services.I18N;
using Ballance2.Utils;
using Newtonsoft.Json;
using ICSharpCode.SharpZipLib.Zip;
using System.Text.RegularExpressions;

namespace Ballance2.Menu.LevelManager
{
  public class LevelManager : GameSingletonBehavior<LevelManager>
  {
    private const string TAG = "LevelManager";

    [SerializeField]
    private List<LevelDefineInternal> internalLevel;

    /// <summary>
    /// 获取在系统中注册过的关卡
    /// </summary>
    public List<LevelRegistedItem> RegisteredLevels { get; } = new List<LevelRegistedItem>();

    public IEnumerable<LevelRegistedItem> LocalLevels => from Type in RegisteredLevels where Type.Type == LevelRegistedType.Local select Type;
    public IEnumerable<LevelRegistedItem> MineLevels => from Type in RegisteredLevels where Type.Type == LevelRegistedType.Mine select Type;

    private void Awake()
    {
      foreach (var level in internalLevel)
        AddInternalLevel(level);
      ScanLevelsDir();
    }

    public void DeleteLevel(LevelRegistedItem v)
    {
      if (v.Type == LevelRegistedType.Local || v.Type == LevelRegistedType.Mine)
      {
        var path = ((LevelRegistedLocalItem)v).path;
        if (Directory.Exists(path))
          FileUtils.RemoveDirectory(path);
        else if (File.Exists(path))
          File.Delete(path);
      }
      //TODO: 订阅
      RegisteredLevels.Remove(v);
    }

    private void ScanLevelsDir()
    {
#if UNITY_STANDALONE
      string dir = GamePathManager.GetLevelRealPath("", false);
      if(Directory.Exists(dir)) {
        var dirInfo = new DirectoryInfo(dir);
        var files = dirInfo.GetFiles("*.blevel", SearchOption.TopDirectoryOnly);
        Log.D(TAG, "Scan Level dir \"" + dir + "\" found " + files.Length + " level files");
        for (int i = 0; i < files.Length; i++) 
          RegisteredLevels.Add(new LevelRegistedLocalItem(files[i].FullName, false));
        DirectoryInfo[] subdirs = dirInfo.GetDirectories("*", SearchOption.TopDirectoryOnly);
        foreach (var item in subdirs)
        {
          if (File.Exists($"{item.FullName}/level.json") && File.Exists($"{item.FullName}/assets.json"))
            RegisteredLevels.Add(new LevelRegistedLocalItem(item.FullName, false));
        }
      } else {
        Log.W(TAG, "Level dir " + dir + " not exists!");
      }
#endif
#if UNITY_EDITOR
      DirectoryInfo direction = new DirectoryInfo(GamePathManager.DEBUG_LEVEL_FOLDER);
      DirectoryInfo[] dirs = direction.GetDirectories("*", SearchOption.TopDirectoryOnly);
      for (int i = 0; i < dirs.Length; i++)
        if (dirs[i].Name != "MakerAssets" && !Regex.IsMatch(dirs[i].Name, "^level\\d{2}$"))
          RegisteredLevels.Add(new LevelRegistedLocalItem(dirs[i].FullName, true));
#endif
    }

    public void AddInternalLevel(LevelDefineInternal levelDefine)
    {
      RegisteredLevels.Add(new LevelRegistedInternalItem(levelDefine));
    }
    public LevelRegistedItem GetInternalLevel(string name)
    {
      return RegisteredLevels.Find(p => p.Type == LevelRegistedType.Internal && ((LevelRegistedInternalItem)p).Name == name);
    }
    public LevelRegistedItem GetLevelByName(string name)
    {
      return RegisteredLevels.Find(p => p.GetName() == name);
    }
  }

  public enum LevelRegistedType
  {
    Internal,
    Local,
    Mine,
    Subscribe,
  }

  public class LevelRegistedItem
  {
    public Sprite Logo;
    public Sprite Preview;
    public LevelJson InfoJson;
    public LevelRegistedType Type;
    public List<GameLevelDependencies> RequiredPackages = new List<GameLevelDependencies>();
    public bool DependsSuccess;
    public string DependsStatus;

    public bool IsLoaded { get; private set; } = false;

    public virtual string GetName()
    {
      throw new NotImplementedException();
    }
    public virtual string GetLocalPath()
    {
      throw new NotImplementedException();
    }
    public virtual Task LoadInfo()
    {
      RequiredPackages = new List<GameLevelDependencies>(InfoJson.requiredPackages);
      if (RequiredPackages.Count == 0)
      {
        //没有依赖
        DependsSuccess = true;
        DependsStatus = I18N.Tr("core.ui.Menu.LevelSelect.LevelNoDepends");
      }
      else
      {
        //加载当前关卡的依赖状态
        int successCount = 0;
        for (var i = 0; i < RequiredPackages.Count; i++)
        {
          var p = RequiredPackages[i];
          var package = GamePackageManager.Instance.FindRegisteredPackage(p.name);
          if (package == null)
            p.loaded = "not";
          else
          {
            if (package.PackageVersion < p.minVersion)
              p.loaded = "vermis";
            else
            {
              successCount++;
              p.loaded = "true";
            }
          }
        }
        //设置状态文字
        if (successCount == RequiredPackages.Count)
        {
          DependsSuccess = true;
          DependsStatus = I18N.TrF("core.ui.Menu.LevelSelect.LevelHasDepends", "", successCount);
        }
        else
        {
          DependsSuccess = false;
          DependsStatus = I18N.TrF("core.ui.Menu.LevelSelect.LevelHasBadDepends", "", RequiredPackages.Count, RequiredPackages.Count - successCount);
        }
      }
      IsLoaded = true;
      return Task.CompletedTask;
    }
    public virtual Task DoLoad()
    {
      return Task.CompletedTask;
    }
  }
  public class LevelRegistedLocalItem : LevelRegistedItem
  {
    public readonly string path;
    public readonly bool isEditor;
    public LevelRegistedLocalItem(string path, bool isEditor)
    {
      this.path = path;
      this.isEditor = isEditor;
      if (path.EndsWith(".blevel") || isEditor)
        Type = LevelRegistedType.Local;
      else
        Type = LevelRegistedType.Mine;
    }
    public override async Task LoadInfo()
    {
      if (path.EndsWith(".blevel"))
      {
        var zip = ZipUtils.OpenZipFile(path);
        ZipEntry theEntry;
        while ((theEntry = zip.GetNextEntry()) != null)
        {
          if (ZipUtils.MatchRootName("Level.json", theEntry))
            InfoJson = JsonConvert.DeserializeObject<LevelJson>(await ZipUtils.LoadStringInZip(zip, theEntry));
          else if (ZipUtils.MatchRootName("LevelLogo.png", theEntry))
            Logo = await ZipUtils.LoadSpriteInZip(zip, theEntry);
          else if (ZipUtils.MatchRootName("LevelPreview.png", theEntry))
            Preview = await ZipUtils.LoadSpriteInZip(zip, theEntry);
        }
        zip.Close();
        zip.Dispose();
      }
      else
      {
        Logo = null;
        var screenshotDir = path + "/screenshot";
        if (Directory.Exists(screenshotDir))
        {
          var files = new DirectoryInfo(screenshotDir).GetFiles("*.png", SearchOption.TopDirectoryOnly);
          if (files.Length > 0)
            Logo = TextureUtils.LoadSpriteFromFile(files[0].FullName, 256, 256);
        }
        Preview = Logo;
        InfoJson = JsonConvert.DeserializeObject<LevelJson>(await File.ReadAllTextAsync(path + "/level.json"));
      }
      await base.LoadInfo();
    }
    public override string GetName()
    {
      return Path.GetFileNameWithoutExtension(path);
    }
    public override string GetLocalPath()
    {
      return path;
    }
  }
  public class LevelRegistedInternalItem : LevelRegistedItem
  {
    public LevelDefineInternal LevelDefineInternal;
    public readonly string Name;

    public LevelRegistedInternalItem(LevelDefineInternal internalDefine)
    {
      LevelDefineInternal = internalDefine;
      Type = LevelRegistedType.Internal;
      Name = internalDefine.Name;
    }

    public override async Task LoadInfo()
    {
      Logo = LevelDefineInternal.Logo;
      Preview = LevelDefineInternal.Preview;
      InfoJson = JsonConvert.DeserializeObject<LevelJson>(LevelDefineInternal.Json.text);
      await base.LoadInfo();
    }
    public override string GetName()
    {
      return Name;
    }
    public override string GetLocalPath()
    {
      return "internal";
    }
  }

  [Serializable]
  public class LevelDefineInternal
  {
    public string Name;
    public Sprite Logo;
    public Sprite Preview;
    public TextAsset Json;
    public GameObject Prefab;
  }
}
