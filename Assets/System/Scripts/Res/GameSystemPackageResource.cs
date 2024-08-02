using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using JetBrains.Annotations;
using SubjectNerd.Utilities;
using UnityEngine;

namespace Ballance2 
{
  public class GameSystemPackageResource : MonoBehaviour 
  {
    public GameSystemPackageResource()
    {
      Instance = this;
    }

    [Reorderable("GameAssets", true, "Name")]
    public List<Object> GameAssets = null;
    public TextAsset SystemPackageLanguageRes;
    public TextAsset SystemPackageDef;

    private static Dictionary<string, Object> namedObjects = new Dictionary<string, Object>();

    public static Object GetAssetByName(string name)
    {
      if (namedObjects.TryGetValue(name, out var a))
        return a;
      return null;
    }
    public static GameSystemPackageResource Instance;
    
    private void Awake() 
    {
      foreach(var o in GameAssets)
        namedObjects.Add(o.name, o);
    }
  }
}