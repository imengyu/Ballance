using System.Collections;
using System.IO;
using Ballance2.Config.Settings;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using Ballance2.Res;

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

namespace Ballance2.Game
{

  [SLua.CustomLuaClass]
  public delegate void GameLevelLoaderNativeCallback(GameObject prefab, string jsonString, LevelAssets level);
  [SLua.CustomLuaClass]
  public delegate void GameLevelLoaderNativeErrCallback(string code, string err);

  [SLua.CustomLuaClass]
  public class LevelAssets
  {
    public AssetBundle AssetBundle;
    public bool LoadInEditor = false;
    public string Path;

    public LevelAssets(string path, bool poadInEditor = false)
    {
      LoadInEditor = poadInEditor;
      Path = path;
#if UNITY_EDITOR
      LoadAllFileNames();
#endif
    }

    public Texture GetTextureAsset(string name)
    {
      return GetLevelAsset<Texture>(name);
    }
    public Texture2D GetTexture2DAsset(string name)
    {
      return GetLevelAsset<Texture2D>(name);
    }
    public AudioClip GetAudioClipAsset(string name)
    {
      return GetLevelAsset<AudioClip>(name);
    }
    public GameObject GetPrefabAsset(string name)
    {
      return GetLevelAsset<GameObject>(name);
    }
    public Material GetMaterialAsset(string name)
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
    
    public T GetLevelAsset<T>(string name) where T : Object
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
      return AssetBundle.LoadAsset<T>(name);
    }
  }

  [SLua.CustomLuaClass]
  public class GameLevelLoaderNative : MonoBehaviour
  {
    private readonly string TAG = "GameLevelLoaderNative";

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
        string path = GamePathManager.GetLevelRealPath(name);
        if (!File.Exists(path))
        {
          errCallback("FILE_NOT_EXISTS", "文件 " + name + " 不存在");
          return;
        }
        Log.D(TAG, "Load package : {0}", path);
        //加载资源包
        StartCoroutine(Loader(new LevelAssets(path), callback, errCallback));
      }
    }
    public void UnLoadLevel(LevelAssets level)
    {
      if (level != null && level.AssetBundle != null) {
        level.AssetBundle.Unload(true);
        level.AssetBundle = null;
      }
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
      }

      TextAsset LevelJsonTextAsset = level.GetLevelAsset<TextAsset>("Level.json");
      if (LevelJsonTextAsset == null || string.IsNullOrEmpty(LevelJsonTextAsset.text))
      {
        errCallback("BAD_LEVEL_JSON", "Level.json is empty or invalid");
        yield break;
      }
      GameObject LevelPrefab = level.GetLevelAsset<GameObject>("Level.prefab");
      if (LevelPrefab == null)
      {
        errCallback("BAD_LEVEL", "The level is invalid. Level.prefab cannot be found");
        yield break;
      }

      callback(LevelPrefab, LevelJsonTextAsset.text, level);
    }
  }
}