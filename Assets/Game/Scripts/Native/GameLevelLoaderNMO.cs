using System.Collections;
using UnityEngine;
using static Ballance2.VirtoolsLoader;
using Ballance2.Package;

/*
 * Copyright (c) 2022  mengyu
 * 
 * 模块名：     
 * GameLevelLoaderNMO.cs
 * 
 * 用途：
 * 用于加载 nmo 关卡文件。
 * 
 * 作者：
 * mengyu
 */

namespace Ballance2.Game
{
  //NMO 资源
  public class LevelNMOAssets : LevelAssets 
  {
    public LevelNMOAssets(string path) : base(path, false) {}

    internal VirtoolsLoaderLoadNMOResult result;

    public override Texture GetTextureAsset(string name)
    {
      if (result.textureList.TryGetValue(name, out var v))
        return v;
      return null;
    }
    public override Texture2D GetTexture2DAsset(string name)
    {
      return GetTextureAsset(name) as Texture2D;
    }
    public override Mesh GetMeshAsset(string name)
    {
      if (result.meshList.TryGetValue(name, out var v))
        return v;
      return null;
    }
    public override AudioClip GetAudioClipAsset(string name)
    {
      return null;
    }
    public override GameObject GetPrefabAsset(string name)
    {
      if (result.objectNameList.TryGetValue(name, out var v))
        return v;
      return null;
    }
    public override Material GetMaterialAsset(string name)
    {
      if (result.materialList.TryGetValue(name, out var v))
        return v;
      return null;
    }
  }

  //NMO 关卡加载器
  public class GameLevelLoaderNMO 
  {
    private readonly string TAG = "GameLevelLoaderNMO";

#if UNITY_STANDALONE_WIN
    public static IEnumerator LoaderNMO(LevelAssets level, GameLevelLoaderNativeCallback callback, GameLevelLoaderNativeErrCallback errCallback) 
    {
      VirtoolsLoader.Init(Application.dataPath + "\\VirtoolsLoader\\CK2.dll");
      var result = VirtoolsLoader.LoadNMOToScense(level.Path, 
        MaterialCallback, 
        TextureCallback, 
        (err) => { errCallback("FAILED_LOAD_NMO", err); },
        GamePackage.GetCorePackage().GetAsset<Shader>("BlinnPhongSpeicalEmission"));

      if (result != null) {
        ((LevelNMOAssets)level).result = result;
        callback(result.mainObj, CreateJsonFromNMO(((LevelNMOAssets)level)), level);
      }
      yield break;
    }
    private static string CreateJsonFromNMO(LevelNMOAssets assets) {
      //生成JSON以供加载器加载
      return "{}";
    }
    private static Texture TextureCallback(string texName)
    {
      return GamePackage.GetCorePackage().GetTextureAsset(texName);
    }
    private static Material MaterialCallback (string matName) 
    {
      return GamePackage.GetCorePackage().GetMaterialAsset(matName);
    }
#endif
  }
}