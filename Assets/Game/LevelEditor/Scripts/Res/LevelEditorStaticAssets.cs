using AYellowpaper.SerializedCollections;
using Ballance2.Base;
using UnityEngine;
using System;
using System.Collections.Generic;
using SubjectNerd.Utilities;
using Ballance2.Res;

namespace Ballance2.Game.LevelEditor
{
  public class LevelEditorStaticAssets : MonoBehaviour
  {
    /// <summary>
    /// 静态资源引入
    /// </summary>
    [Reorderable("GameAssets", true, "Name")]
    public List<GameAssetsInfo> GameAssets = null;

    private static LevelEditorStaticAssets instance;
    private Dictionary<string, UnityEngine.Object> assetsMap = new Dictionary<string, UnityEngine.Object>();

    public LevelEditorStaticAssets() 
    {
      instance = this;
    }
    private void Awake() 
    {
      if (GameAssets != null)
        foreach (var item in GameAssets)
          assetsMap.Add(item.Name, item.Object);
    }

    public static T GetAssetByName<T>(string name) where T : UnityEngine.Object
    {
      if (instance.assetsMap.TryGetValue(name, out var a))
        return (T)a;
      return default;
    }
  }
}