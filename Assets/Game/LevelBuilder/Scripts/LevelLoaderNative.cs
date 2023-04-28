using System.Collections;
using System.IO;
using Ballance2.Config.Settings;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using Ballance2.Res;
using Ballance2.Utils;
using SimpleFileBrowser;

/*
 * Copyright (c) 2020  mengyu
 * 
 * 模块名：     
 * GameLevelLoaderNative.cs
 * 
 * 用途：
 * 关卡 AssetBundle 加载。
 * 因为 lua 中无法直接调用 AssetBundle 加载文件，因此将加载 AssetBundle 的代码抽离至 C# 中。
 * 
 * 作者：
 * mengyu
 */

namespace Ballance2.Game.LevelBuilder
{

  public delegate void GameLevelLoaderNativeCallback(GameObject mainObj, string jsonString, LevelAssets level);
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
    public void LoadLevel(string name, GameLevelLoaderNativeCallback callback, GameLevelLoaderNativeErrCallback errCallback)
    {
#if UNITY_EDITOR
      string realPackagePath = GamePathManager.DEBUG_LEVEL_FOLDER + "/" + name;
      //在编辑器中加载
      if (DebugSettings.Instance.PackageLoadWay == LoadResWay.InUnityEditorProject && Directory.Exists(realPackagePath))
      {
        Log.D(TAG, "Load package in editor : {0}", realPackagePath);
        StartCoroutine(Loader(new LevelAssets(realPackagePath, true), callback, errCallback));
      }
      else
#else
      if(true) 
#endif
      {
        //路径
        string path = name;
        if (!PathUtils.IsAbsolutePath(path))
          path = GamePathManager.GetLevelRealPath(name.ToLower(), false);

        //去除file://前缀
        path = PathUtils.FixFilePathScheme(path);
        //jar特殊处理
        if(!path.Contains("jar:file://") && !File.Exists(path))
        {
          Log.E(TAG, "File not exist : {0}", path);
          errCallback("FILE_NOT_EXISTS", "File not exist: \"" + path + "\"");
          return;
        }
        //osx前缀 file://
        #if UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX || UNITY_IOS
        if(!path.Contains("jar:file://"))
          path = "file://" + path;
        #endif

        //加载NMO
#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
        var ext = Path.GetExtension(path).ToLower();
        Log.D(TAG, "Load nmo file : {0} ext: {1}", path, ext);
        if (ext == ".nmo" || ext == ".cmo") {
          StartCoroutine(LevelLoaderNMO.LoaderNMO(new LevelNMOAssets(path), callback, errCallback));
          return;
        }
#endif
        Log.D(TAG, "Load package : {0}", path);
        //加载资源包
        StartCoroutine(Loader(new LevelAssets(path), callback, errCallback));
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

    private IEnumerator Loader(LevelAssets level, GameLevelLoaderNativeCallback callback, GameLevelLoaderNativeErrCallback errCallback)
    {
      if (!level.LoadInEditor)
      {
        UnityWebRequest request = UnityWebRequest.Get(level.Path);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
          if (request.responseCode == 404)
            errCallback("FILE_NOT_FOUND", "Level file not found");
          else if (request.responseCode == 403)
            errCallback("ACCESS_DENINED", "No permission to read file");
          else
            errCallback("REQUEST_ERROR", "Http error: " + request.responseCode);
          yield break;
        }

        AssetBundleCreateRequest assetBundleCreateRequest = AssetBundle.LoadFromMemoryAsync(request.downloadHandler.data);
        yield return assetBundleCreateRequest;
        var assetBundle = assetBundleCreateRequest.assetBundle;

        if (assetBundle == null)
        {
          errCallback("FAILED_LOAD_ASSETBUNDLE", "Wrong level, failed to load AssetBundle");
          yield break;
        }

        level.AssetBundle = assetBundle;
      }
      
      Log.D(TAG, "Level package {0} loaded", level.Path);

      TextAsset LevelJsonTextAsset = level.GetTextAssetAsset("Level.json");
      if (LevelJsonTextAsset == null || string.IsNullOrEmpty(LevelJsonTextAsset.text))
      {
        errCallback("BAD_LEVEL_JSON", "Level.json is empty or invalid");
        yield break;
      }
      GameObject LevelPrefab = level.GetPrefabAsset("Level.prefab");
      if (LevelPrefab == null)
      {
        errCallback("BAD_LEVEL", "The level is invalid. Level.prefab cannot be found");
        yield break;
      }
      GameObject LevelMainObj = CloneUtils.CloneNewObject(LevelPrefab, "");
      callback(LevelMainObj, LevelJsonTextAsset.text, level);
    }
  }
}