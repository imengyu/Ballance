using System.Collections;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using Ballance2.Res;
using Ballance2.Utils;
using SimpleFileBrowser;
using Ballance2.Menu.LevelManager;
using ICSharpCode.SharpZipLib.Zip;

/*
 * Copyright (c) 2024  mengyu
 * 
 * 模块名：     
 * GameLevelLoaderNative.cs
 * 
 * 用途：
 * 关卡 AssetBundle 加载。
 * 
 * 作者：
 * mengyu
 */

namespace Ballance2.Game.LevelBuilder
{

  public delegate void GameLevelLoaderNativeCallback(GameObject mainPrefab, string jsonString, LevelAssets level, string dynamicLevelPath);
  public delegate void GameLevelLoaderNativeErrCallback(string code, string err);

  public class LevelAssets
  {
    public AssetBundle AssetBundle;
    public bool LoadInEditor = false;
    public string Path;

    public LevelAssets(string path, bool loadInEditor = false)
    {
      LoadInEditor = loadInEditor;
      Path = path;
#if UNITY_EDITOR
      LoadAllFileNames();
#endif
    }

    public virtual Texture GetTextureAsset(string name)
    {
      return GetLevelAsset<Texture>(name);
    }
    public virtual Mesh GetMeshAsset(string name)
    {
      return GetLevelAsset<Mesh>(name);
    }
    public virtual TextAsset GetTextAssetAsset(string name)
    {
      return GetLevelAsset<TextAsset>(name);
    }
    public virtual Texture2D GetTexture2DAsset(string name)
    {
      return GetLevelAsset<Texture2D>(name);
    }
    public virtual AudioClip GetAudioClipAsset(string name)
    {
      return GetLevelAsset<AudioClip>(name);
    }
    public virtual GameObject GetPrefabAsset(string name)
    {
      return GetLevelAsset<GameObject>(name);
    }
    public virtual Material GetMaterialAsset(string name)
    {
      return GetLevelAsset<Material>(name);
    }

#if UNITY_EDITOR
    private Dictionary<string, string> fileList = new Dictionary<string, string>();
    private void LoadAllFileNames()
    {
      if (string.IsNullOrEmpty(Path))
        return; 
      DirectoryInfo theFolder = new DirectoryInfo(Path);
      FileInfo[] thefileInfo = theFolder.GetFiles("*.*", SearchOption.AllDirectories);
      foreach (FileInfo NextFile in thefileInfo)
      { 
        //遍历文件
        string path = NextFile.FullName.Replace("\\", "/");
        if(path.EndsWith(".meta")) continue;
        int index = path.IndexOf("Assets/");
        if (index > 0)
          path = path.Substring(index);

        fileList.Add(NextFile.Name, path);
      }
    }
    private string GetFullPathByName(string name)
    {
      if (fileList.TryGetValue(name, out string fullpath))
        return fullpath;
      return null;
    }
#endif
    private T GetLevelAsset<T>(string name) where T : Object
    {
#if UNITY_EDITOR
      if (LoadInEditor)
      {
        if (name.StartsWith("Assets/"))
          return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(name);
        else
        {
          var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(Path + "/" + name);
          if (asset == null && !name.Contains("/") && !name.Contains("\\"))
          {
            string fullPath = GetFullPathByName(name);
            if (fullPath != null)
              asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(fullPath);
          }
          return asset;
        }
      }
      else
#endif
      if (AssetBundle != null)
        return AssetBundle.LoadAsset<T>(name);
      return null;
    }
  }

  public class LevelLoaderNative : MonoBehaviour
  {
    private readonly string TAG = "LevelLoaderNative";

    /// <summary>
    /// 加载关卡
    /// </summary>
    /// <param name="name">名称或者路径</param>
    /// <param name="callback">成功回调</param>
    /// <param name="errCallback">失败回调</param>
    public void LoadLevel(LevelRegistedItem _level, GameLevelLoaderNativeCallback callback, GameLevelLoaderNativeErrCallback errCallback)
    {
      if (_level.Type == LevelRegistedType.Internal)
      {
        var internalLevel = (LevelRegistedInternalItem)_level;
        callback(internalLevel.LevelDefineInternal.Prefab, internalLevel.LevelDefineInternal.Json.text, null, null);
        return;
      }
      else if (_level.Type == LevelRegistedType.Local)
      {
        var level = (LevelRegistedLocallItem)_level;
#if UNITY_EDITOR
        string realPackagePath = GamePathManager.DEBUG_LEVEL_FOLDER + "/" + Path.GetFileNameWithoutExtension(name);
        //在编辑器中加载
        if (level.isEditor && Directory.Exists(realPackagePath))
        {
          Log.D(TAG, "Load package in editor : {0}", realPackagePath);
          StartCoroutine(Loader(level, new LevelAssets(realPackagePath, true), callback, errCallback));
        }
        else
#else
        if(true) 
#endif
        {
          //加载资源包
          StartCoroutine(Loader(level, new LevelAssets(""), callback, errCallback));
        }
      }
    }
    /// <summary>
    /// 卸载关卡
    /// </summary>
    /// <param name="level">LoadLevel 返回的关卡实例</param>
    public void UnLoadLevel(LevelAssets level)
    {
      if (level != null && level.AssetBundle != null) {
        level.AssetBundle.Unload(true);
        level.AssetBundle = null;
      }
    }
    /// <summary>
    /// 打开文件选择器选择一个关卡文件
    /// </summary>
    /// <param name="ext">文件后缀名，包括点</param>
    /// <param name="callback">成功回调</param>
    public static void PickLevelFile(string ext, GameLevelLoaderNativeErrCallback callback) {
      FileBrowser.SetFilters(false, ext);
      FileBrowser.ShowLoadDialog((paths) => {
        callback(paths[0], "");
      }, () => {}, FileBrowser.PickMode.Files);
    }

    private IEnumerator Loader(LevelRegistedLocallItem level, LevelAssets asset, GameLevelLoaderNativeCallback callback, GameLevelLoaderNativeErrCallback errCallback)
    {
      var levelJson = "";
      var isDynamic = false;
      GameObject LevelPrefab = null;
      if (asset.LoadInEditor)
      {
        levelJson = asset.GetTextAssetAsset("Level.json").text;
        LevelPrefab = asset.GetPrefabAsset("Level.prefab");
      }
      else
      { 
        var zip = ZipUtils.OpenZipFile(level.GetLocalPath());
        ZipEntry theEntry;
        while ((theEntry = zip.GetNextEntry()) != null)
        {
          if (ZipUtils.MatchRootName("assets.json", theEntry))
          {
            isDynamic = true;
          }
          else if (ZipUtils.MatchRootName("Level.json", theEntry))
          {
            //定义文件
            var task = ZipUtils.LoadStringInZip(zip, theEntry);
            yield return task.AsIEnumerator();
            levelJson = task.Result;
          }
          else if (ZipUtils.MatchRootName("assets/packed.assetbundle", theEntry))
          {
            //已打包的ab包
            var task = ZipUtils.ReadZipFileToMemoryAsync(zip);
            yield return task.AsIEnumerator();

            AssetBundleCreateRequest assetBundleCreateRequest = AssetBundle.LoadFromMemoryAsync(task.Result.ToArray());
            yield return assetBundleCreateRequest;
            var assetBundle = assetBundleCreateRequest.assetBundle;

            if (assetBundle == null)
            {
              errCallback("FAILED_LOAD_ASSETBUNDLE", "Wrong level, failed to load AssetBundle");
              yield break;
            }

            asset.AssetBundle = assetBundle;
          }
        }

        zip.Close();
        zip.Dispose();
      }
      
      Log.D(TAG, "Level package {0} loaded", level.GetLocalPath());

      if (levelJson == null)
      {
        errCallback("BAD_LEVEL_JSON", "Level.json is empty or invalid");
        yield break;
      }

      if (!isDynamic)
      {
        LevelPrefab = asset.GetPrefabAsset("Level.prefab");

        if (LevelPrefab == null)
        {
          errCallback("BAD_LEVEL", "The level is invalid. Level.prefab cannot be found");
          yield break;
        }
      }
      callback(LevelPrefab, levelJson, asset, isDynamic ? level.GetLocalPath() : null);
    }
  }
}