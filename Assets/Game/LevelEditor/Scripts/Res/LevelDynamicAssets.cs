using System.Collections.Generic;
using Ballance2.Package;
using Newtonsoft.Json;
using UnityEngine;

namespace Ballance2.Game.LevelEditor
{
  /// <summary>
  /// 动态关卡的资源定义
  /// </summary>
  [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
  public class LevelDynamicModelAsset
  {
    [JsonProperty]
    public LevelDynamicModelCategory Category;
    [JsonProperty]
    public string SubCategory;
    [JsonProperty]
    public LevelDynamicModelSource SourceType;
    [JsonProperty]
    public string SourcePath;
    [JsonProperty]
    public List<LevelProviderSubModelRefAsset> SubModelRefs = new List<LevelProviderSubModelRefAsset>();

    [JsonProperty]
    public string Name;
    [JsonProperty]
    public string Desc;
    [JsonProperty]
    public string Tag;
    [JsonProperty]
    public string ObjName;
    [JsonProperty]
    public string ObjTarget;
    [JsonProperty]
    public Vector3 IntitalScale = Vector3.one;
    [JsonProperty]
    public Vector3 IntitalEulerAngles = Vector3.zero;

    public Sprite PreviewImage;
    public Sprite ScenseGizmePreviewImage;
    public bool OnlyOne = false;
    public bool CanDelete = true;
    public bool HiddenInContentSelector = false;
    public bool HiddenPlaceholderRender = false;

    public GameObject Prefab;
    public bool Loaded = false;
    public bool PreviewImageLoaded = false;

    public override string ToString() 
    {
      return "LevelDynamicModelAsset:" + SourcePath;
    }

    public LevelDynamicModelAsset()
    {

    }
    public LevelDynamicModelAsset(LevelProviderAsset item, GamePackage package)
    {
      SourcePath = $"{package.PackageName}:{item.Name}";
      SourceType = LevelDynamicModelSource.Package;
      Category = item.Category;
      SubCategory = item.SubCategory;
      Tag = item.Tag;
      SubModelRefs = item.SubModelRefs;
      OnlyOne = item.OnlyOne;
      HiddenInContentSelector = item.HiddenInContentSelector;
      HiddenPlaceholderRender = item.HiddenPlaceholderRender;
      Loaded = true;
      Prefab = item.Prefab;
      Name = item.Name;
      Desc = item.Desc;
      PreviewImage = item.Preview;
      ObjName = item.ObjName;
      ObjTarget = item.ObjTarget;
      ScenseGizmePreviewImage = item.ScenseGizmePreviewImage;
      IntitalScale = item.IntitalScale;
    }
  }
  /// <summary>
  /// 资源模型来源
  /// </summary>
  public enum LevelDynamicModelSource
  {
    Package,
    Embed,
  }
  /// <summary>
  /// 资源模型分类
  /// </summary>
  public enum LevelDynamicModelCategory
  {
    UnSet,
    Floors,
    Rails,
    Moduls,
    Decoration,
  }
}
